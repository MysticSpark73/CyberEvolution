using System;
using CyberEvolution.Pooling;
using CyberEvolution.Simulation;
using CyberEvolution.Simulation.Genomes;
using UnityEngine;

namespace CyberEvolution.Entities
{
    public class Mob : MonoBehaviour, IPoolable
    {
        public event Action OnReturned;
        public BasicPooler Pooler { get; protected set; }
        private GenomeCache _genomeCache;
        private MobsController _mobsController;
        
        private int _generationId;
        private int _currentCommandPointer;
        private int _energy;

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

        public void Initialize(Vector2Int position, int generationId, int energy, MobsController mobsController,
            GenomeCache genomeCache)
        {
            _mobsController = mobsController;
            //todo: transform grid pos to world and set gameObject
            _generationId = generationId;
            _currentCommandPointer = 0;
            _energy = energy;
            _genomeCache = genomeCache;
        }

        public void ExecuteCommand(Vector2Int gridPosition)
        {
            _genomeCache.GetNextCommand(_generationId, ref _currentCommandPointer);
            //todo: get energy cost from config
            //todo: after execution implement TryReproduce() or smt
        }
    }
}