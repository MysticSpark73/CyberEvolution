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

        public void InitializePoolable(BasicPooler pooler)
        {
            Pooler = pooler;
            gameObject.SetActive(false);
        }

        public void OnSpawn()
        {
            gameObject.SetActive(true);
            //todo: set text to generation ID
            //todo: get generation color from SO
        }

        public void OnReturn()
        {
            gameObject.SetActive(false);
            _mobsController.RemoveMob(this);
            OnReturned?.Invoke();
        }

        public void Move(Vector2Int direction, float energyCost)
        {
            DepleteEnergy(energyCost);
            
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
            DepleteEnergy(energyCost);
            transform.Rotate(Vector3.forward * degrees);
        }

        public void DoNothing(float energyCost) => DepleteEnergy(energyCost);

        public void Consume(FoodType foodType, float energyCost)
        {
            DepleteEnergy(energyCost);
            
            Vector2Int targetPosition = _gridPosition + GetForwardDirection();
            Cell targetCell = _gridController.GetCell(targetPosition);

            if (targetCell == null)
            {
                Debug.Log($"[Mob][Consume] cell position {targetPosition} is outside of the grid!");
                return;
            }

            Food food = targetCell.Food;

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
            DepleteEnergy(energyCost);
            
            Vector2Int targetPosition = _gridPosition + direction;
            Cell targetCell = _gridController.GetCell(targetPosition);

            if (targetCell == null)
            {
                Debug.Log($"[Mob][Attack] cell position {targetPosition} is outside of the grid!");
                return;
            }

            Mob target = targetCell.Mob;

            if (target == null)
            {
                Debug.Log("[Mob][Attack] cell has nothing to attack!");
                return;
            }

            target.TakeDamage(_attackDamage);
        }

        public void AttackForward(bool isAggressiveTowardsFriendly, float energyCost) =>
            Attack(GetForwardDirection(), isAggressiveTowardsFriendly, energyCost);

        public void TakeDamage(float damage) => DepleteEnergy(damage);

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
            
            UpdateView();
            
            SetPosition(position);
        }

        public void ExecuteCommand()
        {
            CommandType commandType = _genomeCache.GetNextCommand(_generationId, ref _currentCommandPointer);
            var command = _commandsFactory.Create(commandType, this);
            command.Execute();
            TryReproduce();
        }

        private void TryReproduce()
        {
            if (_energy < _energyToReproduce) return;

            Cell targetCell = _gridController.GetFirstEmptyCellNearby(_gridPosition);

            if (targetCell == null)
            {
                Debug.LogError($"[Mob][TryReproduce] can't reproduce! No emptyCells nearby!", this);
                return;
            }
            
            DepleteEnergy(_energyToReproduce);

            _genomeCache.TryMutateGenome(_generationId, out int mutatedId);
            
            _mobsController.SpawnMob(targetCell.GridPosition, mutatedId, _energyToReproduce);
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

        private void DepleteEnergy(float energyCost)
        {
            _energy = Mathf.Max(0, _energy - energyCost);
            if (_energy <= 0) Die();
        }

        private void RestoreEnergy(float energy)
        {
            _energy += energy;
        }

        private Vector2Int GetForwardDirection() => Vector2Int.RoundToInt(new Vector2(transform.up.x, transform.up.y));

        private void Die()
        {
            Pooler.Return(this);
        }
    }
}