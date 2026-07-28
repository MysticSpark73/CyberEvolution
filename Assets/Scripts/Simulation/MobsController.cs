using System.Collections.Generic;
using CyberEvolution.Entities;
using CyberEvolution.Grid;
using CyberEvolution.Pooling;
using CyberEvolution.Simulation.Genomes;
using UnityEngine;

namespace CyberEvolution.Simulation
{
    public class MobsController
    {
        private readonly BasicPooler _pooler;
        private readonly GridController _gridController;
        private readonly GenomeCache _genomeCache;
        
        private List<Mob> _mobs = new();


        public MobsController(BasicPooler pooler, GridController gridController, GenomeCache genomeCache)
        {
            _pooler = pooler;
            _gridController = gridController;
            _genomeCache = genomeCache;
        }

        public void SpawnMob(Vector2Int position)
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
            
            mob.Initialize(cell.GridPosition, 0, 50, this, _genomeCache);
            cell.SetMob(mob);
            _mobs.Add(mob);
        }
    }
}