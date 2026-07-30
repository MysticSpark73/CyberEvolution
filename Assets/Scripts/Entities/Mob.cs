using System;
using CyberEvolution.Commands;
using CyberEvolution.Grid;
using CyberEvolution.Pooling;
using CyberEvolution.Simulation;
using CyberEvolution.Simulation.Genomes;
using UnityEngine;

namespace CyberEvolution.Entities
{
    public class Mob : MonoBehaviour, IPoolable, ICommandListener, IDamageable
    {
        public event Action OnReturned;
        public BasicPooler Pooler { get; protected set; }
        private GenomeCache _genomeCache;
        private MobsController _mobsController;
        
        private int _generationId;
        private int _currentCommandPointer;
        private float _energy;
        private float _energyToReproduce;
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
            _gridPosition = targetPosition;
            targetCell.SetMob(this);
            
        }

        public void MoveForward(float energyCost) => Move(GetForwardDirection(), energyCost);

        public void Turn(int degrees, float energyCost)
        {
            DepleteEnergy(energyCost);
            transform.Rotate(Vector3.up * degrees);
        }

        public void DoNothing(float energyCost) => DepleteEnergy(energyCost);

        public void Consume(FoodType foodType, float energyCost)
        {
            //todo: getCell
            //todo: getFoodByType
            //todo: eatIfPossible
            DepleteEnergy(energyCost);
            throw new NotImplementedException();
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
                Debug.Log($"[Mob][Attack] cell has nothing to attack!");
                return;
            }
            
            target.TakeDamage(25);
            throw new NotImplementedException();
        }

        public void AttackForward(bool isAggressiveTowardsFriendly, float energyCost) =>
            Attack(GetForwardDirection(), isAggressiveTowardsFriendly, energyCost);

        public void TakeDamage(float damage) => DepleteEnergy(damage);

        public void InitializeDependencies(MobsController mobsController, GenomeCache genomeCache,
            CommandsFactory commandsFactory, GridController gridController)
        {
            if (_areDependenciesInitialized) return;
            
            _mobsController = mobsController;
            _genomeCache = genomeCache;
            _commandsFactory = commandsFactory;
            _gridController = gridController;
            _areDependenciesInitialized = true;
        }

        public void SetupOnGrid(Vector2Int position, int generationId, int energy, float energyToReproduce)
        {
            _generationId = generationId;
            _currentCommandPointer = 0;
            _energy = energy;
            _energyToReproduce = energyToReproduce;
            _gridPosition = position;
            
            transform.position = new Vector3(_gridPosition.x, _gridPosition.y, 0);
        }

        public void ExecuteCommand(Vector2Int gridPosition)
        {
            CommandType commandType = _genomeCache.GetNextCommand(_generationId, ref _currentCommandPointer);
            var command = _commandsFactory.Create(commandType, this);
            command.Execute();
            TryReproduce();
        }

        private void TryReproduce()
        {
            if (_energy < _energyToReproduce) return;
         
            //todo: _gridController.TryFindEmptyNeighbourCell(out Cell cell)
            //todo: _genomeCache.TryMutate();
            //todo: _mobsController.SpawnMob(Cell.GridPosition, generationId, _energy/2, _energyToReproduce);
        }

        private void DepleteEnergy(float energyCost)
        {
            _energy = Mathf.Max(0, _energy - energyCost);
            if (_energy <= 0) Die();
        }

        private Vector2Int GetForwardDirection() => Vector2Int.RoundToInt(new Vector2(transform.forward.x, transform.forward.y));

        private void Die()
        {
            Pooler.Return(this);
        }
    }
}