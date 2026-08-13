using System;
using CyberEvolution.Commands;
using CyberEvolution.Grid;
using CyberEvolution.Pooling;
using CyberEvolution.Simulation;
using CyberEvolution.Simulation.Genomes;
using TMPro;
using UnityEngine;

namespace CyberEvolution.Entities
{
    public class Mob : MonoBehaviour, IPoolable, ICommandListener, IDamageable
    {
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private GameObject _outline;
        [SerializeField] private GameObject _consumeActionObject;
        [SerializeField] private GameObject _attackActionObject;
        [SerializeField] private TextMeshPro _generationLabel;
        
        public event Action OnReturned;
        public BasicPooler Pooler { get; protected set; }
        private GenomeCache _genomeCache;
        private MobsController _mobsController;
        private FoodController _foodController;
        
        private int _generationId;
        private int _currentCommandPointer;
        private float _attackDamage;
        private float _energyToReproduce;
        private float _energy;
        private bool _areDependenciesInitialized;
        private CommandsFactory _commandsFactory;
        private GridController _gridController;
        private Vector2Int _gridPosition;
        private SensorData _sensorData;

        public void InitializePoolable(BasicPooler pooler)
        {
            Pooler = pooler;
            gameObject.SetActive(false);
        }

        public void OnSpawn()
        {
            gameObject.SetActive(true);
        }

        public void OnReturn()
        {
            gameObject.SetActive(false);
            _mobsController.RemoveMob(this);
            OnReturned?.Invoke();
        }

        public void Move(Vector2Int direction, float energyCost)
        {
            if (!TryDepleteEnergy(energyCost)) return;
            
            Vector2Int targetPosition = _gridPosition + direction;
            Cell targetCell = _gridController.GetCell(targetPosition);
            if (targetCell == null)
            {
                Debug.Log($"[Mob][Move] cell position {targetPosition} is outside of the grid!");
                return;
            }
            
            if (!targetCell.IsWalkable)
            {
                Debug.Log($"[Mob][Move] cell [{targetCell.GridPosition}] is not walkable!");
                return;
            }
            
            _gridController.GetCell(_gridPosition)?.RemoveMob();
            SetPosition(targetPosition);
            targetCell.SetMob(this);
        }

        public void MoveForward(float energyCost) => Move(GetForwardDirection(), energyCost);

        public void Turn(int degrees, float energyCost)
        {
            if (!TryDepleteEnergy(energyCost)) return;
 
            transform.Rotate(Vector3.forward * degrees);
        }

        public void DoNothing(float energyCost) => TryDepleteEnergy(energyCost);

        public void Consume(FoodType foodType, float energyCost)
        {
            if (!TryDepleteEnergy(energyCost)) return;
            
            Vector2Int targetPosition = _gridPosition + GetForwardDirection();
            Cell targetCell = _gridController.GetCell(targetPosition);

            if (targetCell == null)
            {
                Debug.Log($"[Mob][Consume] cell position {targetPosition} is outside of the grid!");
                return;
            }

            Food food = targetCell.Food;
            _consumeActionObject.SetActive(true);

            if (food == null)
            {
                Debug.Log("[Mob][Consume] there is no food there :(");
                return;
            }

            if (food.Type != foodType) return;
            
            RestoreEnergy(_foodController.GetEnergyByType(food.Type));
            food.Consume();
        }

        public void Attack(Vector2Int direction, bool isAggressiveTowardsFriendly, float energyCost)
        {
            if (!TryDepleteEnergy(energyCost)) return;
            
            Vector2Int targetPosition = _gridPosition + direction;
            Cell targetCell = _gridController.GetCell(targetPosition);

            if (targetCell == null)
            {
                Debug.Log($"[Mob][Attack] cell position {targetPosition} is outside of the grid!");
                return;
            }

            _attackActionObject.SetActive(true);
            Mob target = targetCell.Mob;

            if (target == null)
            {
                Debug.Log("[Mob][Attack] cell has nothing to attack!");
                return;
            }
            
            target.TakeDamage(_attackDamage, _gridPosition);
        }

        public void AttackForward(bool isAggressiveTowardsFriendly, float energyCost) =>
            Attack(GetForwardDirection(), isAggressiveTowardsFriendly, energyCost);

        public void TakeDamage(float damage, Vector2Int from)
        {
            if (!TryDepleteEnergy(damage)) return;
            
            _sensorData.WasAttacked = true;
            _sensorData.AttackDirection = from - _gridPosition;
        }

        public void InitializeDependencies(MobsController mobsController, FoodController foodController, GenomeCache genomeCache,
            CommandsFactory commandsFactory, GridController gridController)
        {
            if (_areDependenciesInitialized) return;
            
            _mobsController = mobsController;
            _foodController = foodController;
            _genomeCache = genomeCache;
            _commandsFactory = commandsFactory;
            _gridController = gridController;
            _areDependenciesInitialized = true;
        }

        public void SetupOnGrid(Vector2Int position, int generationId, float energy, float energyToReproduce, float attackDamage)
        {
            _generationId = generationId;
            _currentCommandPointer = 0;
            _energy = energy;
            _energyToReproduce = energyToReproduce;
            _attackDamage = attackDamage;
            
            InitializeSensorData();

            DisableActions();
            UpdateView();
            
            SetPosition(position);
            transform.rotation = Quaternion.identity;
        }

        public bool IsFriendly(int generationId) => _generationId == generationId;

        public void ExecuteCommand()
        {
            DisableActions();
            UpdateSensorData();
            CommandType commandType = _genomeCache.GetNextCommand(_generationId, ref _currentCommandPointer, _sensorData);
            var command = _commandsFactory.Create(commandType, this);
            command.Execute();
            TryReproduce();
            ResetSensorData();
        }

        private void TryReproduce()
        {
            if (_energy < _energyToReproduce) return;

            Cell targetCell = _gridController.GetFirstWalkableCellNearby(_gridPosition);

            if (targetCell == null)
            {
                Debug.LogError($"[Mob][TryReproduce] can't reproduce! No emptyCells nearby!", this);
                return;
            }
            
            TryDepleteEnergy(_energyToReproduce * .5f);

            _genomeCache.TryMutateGenome(_generationId, out int mutatedId);
            
            _mobsController.SpawnMob(targetCell.GridPosition, mutatedId, _energyToReproduce * .5f);
        }

        private void SetPosition(Vector2Int position)
        {
            _gridPosition = position;
            transform. position = new Vector3(_gridPosition.x, _gridPosition.y, 0);
        }

        private void UpdateView()
        {
            _generationLabel.text = _generationId.ToString();
            _sprite.color = _genomeCache.GetColor(_generationId);
        }

        private bool TryDepleteEnergy(float energyCost)
        {
            _energy = Mathf.Max(0, _energy - energyCost);
            
            if (_energy <= 0)
            {
                Die();
                return false;
            }
            return true;
        }

        private void RestoreEnergy(float energy)
        {
            _energy += energy;
        }

        private Vector2Int GetForwardDirection() => Vector2Int.RoundToInt(new Vector2(transform.up.x, transform.up.y));

        private void Die()
        {
            Vector2Int gridPosition = _gridPosition;
            Pooler.Return(this);
            _foodController.OnMobDied(gridPosition);
        }

        private void InitializeSensorData()
        {
            _sensorData.SelfGenerationId = _generationId;
            _sensorData.WasAttacked = false;
            _sensorData.AttackDirection = Vector2Int.zero;
            _sensorData.CellInFront = null;
        }

        private void UpdateSensorData()
        {
            _sensorData.CellInFront = _gridController.GetCell(_gridPosition + GetForwardDirection());
        }

        private void ResetSensorData()
        {
            _sensorData.WasAttacked = false;
        }

        private void DisableActions()
        {
            _consumeActionObject.SetActive(false);
            _attackActionObject.SetActive(false);
        }
    }
}