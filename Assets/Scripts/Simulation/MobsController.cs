using System.Collections.Generic;
using CyberEvolution.Commands;
using CyberEvolution.Entities;
using CyberEvolution.Grid;
using CyberEvolution.Pooling;
using CyberEvolution.Simulation.Genomes;
using UnityEngine;

namespace CyberEvolution.Simulation
{
    public class MobsController : IUpdatable
    {
        private readonly BasicPooler _pooler;
        private readonly GridController _gridController;
        private readonly GenomeCache _genomeCache;
        private readonly CommandsFactory _commandsFactory;
        
        private readonly float _energyToReproduce;
        private readonly float _attackDamage;

        private List<Mob> _mobs = new();

        public MobsController(BasicPooler pooler, GridController gridController, GenomeCache genomeCache,
            CommandsFactory commandsFactory, float energyToReproduce, float attackDamage)
        {
            _pooler = pooler;
            _gridController = gridController;
            _genomeCache = genomeCache;
            _commandsFactory = commandsFactory;
            _energyToReproduce = energyToReproduce;
            _attackDamage = attackDamage;
        }

        public void SpawnMob(Vector2Int position, int generationId, float energy)
        {
            Cell cell = _gridController.GetCell(position);
            if (cell.Mob != null)
            {
                Debug.LogError($"[MobsController][SpawnMob] Can't spawn mob at {position.x}, {position.y} because cell is not Empty!");
                return;
            }

            Mob mob = _pooler.Get<Mob>();
            if (mob == null)
            {
                Debug.LogError($"[MobsController][SpawnMob] Failed to retrieve mob from the pool!");
                return;
            }
            
            mob.InitializeDependencies(this, _genomeCache, _commandsFactory, _gridController);
            mob.SetupOnGrid(cell.GridPosition, generationId, energy, _energyToReproduce, _attackDamage);
            cell.SetMob(mob);
            _mobs.Add(mob);
        }

        public void RemoveMob(Mob mob) => _mobs.Remove(mob);

        public void Update()
        {
            for (int i = 0; i < _mobs.Count; i++)
            {
                _mobs[i].ExecuteCommand();
            }
        }
    }
}