using System;
using CyberEvolution.Pooling;
using UnityEngine;

namespace CyberEvolution.Entities
{
    public class Mob : MonoBehaviour, IPoolable
    {
        public event Action OnReturned;
        public BasicPooler Pooler { get; protected set; }
        
        private int _generationId;
        private int _currentCommandPointer;
        private int _energy;

        public void Initialize(BasicPooler pooler)
        {
            Pooler = pooler;
        }

        public void OnSpawn()
        {
            gameObject.SetActive(true);
        }

        public void OnReturn()
        {
            gameObject.SetActive(false);
            OnReturned?.Invoke();
        }

        public void SetupOnSpawned(Vector2Int position, int generationId, int energy)
        {
            //todo: transform grid pos to world and set gameObject
            _generationId = generationId;
            _currentCommandPointer = 0;
            _energy = energy;
        }

        public void ExecuteCommand()
        {
            _currentCommandPointer++;
        }
    }
}