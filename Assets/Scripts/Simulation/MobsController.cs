using System.Collections.Generic;
using CyberEvolution.Entities;
using CyberEvolution.Grid;
using CyberEvolution.Pooling;
using UnityEngine;

namespace CyberEvolution.Simulation
{
    public class MobsController
    {
        private BasicPooler _pooler;
        private GridController _gridController;
        
        private List<Mob> _mobs = new();
        private float _updateTime;
        private float _timeFromLastUpdate;
        private bool _isPaused;

        public MobsController(BasicPooler pooler, GridController gridController)
        {
            _pooler = pooler;
            _gridController = gridController;
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
            
            mob.SetupOnSpawned(cell.GridPosition, 0, 50);
            cell.SetMob(mob);
            _mobs.Add(mob);
        }

        public void Update(float deltaTime)
        {
            if (_isPaused) return;
            
            _timeFromLastUpdate += deltaTime;

            if (_timeFromLastUpdate >= _updateTime)
            {
                _timeFromLastUpdate = 0;
                foreach (var mob in _mobs)
                {
                    //todo: mob.Update()
                }
            }
        }
    }
}