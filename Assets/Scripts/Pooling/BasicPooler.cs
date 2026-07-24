using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace CyberEvolution.Pooling
{
    public class BasicPooler : MonoBehaviour
    {
        public static BasicPooler Instance;

        [SerializeField] private Pool[] _pools;

        private Dictionary<Type, Queue<IPoolable>> _activePools;

        [CanBeNull]
        public T Get<T>() where T : class, IPoolable 
        {
            if (_activePools.TryGetValue(typeof(T), out Queue<IPoolable> pool))
            {
                ExtendPoolIfNeeded<T>(pool);

                T item = pool.Dequeue() as T;
                if (item != null)
                {
                    item.OnSpawn();
                }

                return item;
            }

            return null;
        }

        public void Return(IPoolable poolable)
        {
            if (poolable == null) return;
            
            if (_activePools.TryGetValue(poolable.GetType(), out Queue<IPoolable> pool))
            {
                poolable.OnReturn();
                pool.Enqueue(poolable);
            }
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            CreatePools();
        }

        private void ExtendPoolIfNeeded<T>(Queue<IPoolable> poolables) where T : class, IPoolable
        {
            if (poolables.Count > 0) return;

            var pool = GetPoolByType<T>();
            
            if (pool == null) return;

            IPoolable poolable = Instantiate(pool.Value.Prefab, pool.Value.Container)?.GetComponent<IPoolable>();
            
            if (poolable == null)
            {
                Debug.LogError(
                    $"[Get] Prefab of type {pool.Value.Prefab.GetType()} does not implement the IPoolable interface!");
                return;
            }

            AddToPool(poolable);
        }

        private Pool? GetPoolByType<T>() where T : class, IPoolable
        {
            foreach (var pool in _pools)
            {
                if (pool.Prefab.GetType() == typeof(T))
                {
                    return pool;
                }
            }

            Debug.LogError($"[GetPoolByType] There is no pool for items of type {typeof(T)} ");

            return null;
        }
        

        private void CreatePools()
        {
            _activePools = new Dictionary<Type, Queue<IPoolable>>();
            foreach (var pool in _pools)
            {
                IPoolable poolable = Instantiate(pool.Prefab, pool.Container).GetComponent<IPoolable>();
                
                if (poolable == null)
                {
                    Debug.LogError($"[CreatePools] Prefab of type {pool.Prefab.GetType()} does not implement the IPoolable interface thus current pool is being skipped!");
                    continue;
                }
                
                AddToPool(poolable);
            }
        }
        

        private void AddToPool(IPoolable poolable)
        {
            poolable.Initialize(this);
            
            if (!_activePools.ContainsKey(poolable.GetType()))
            {
                _activePools.Add(poolable.GetType(), new Queue<IPoolable>());
            } 
            _activePools[poolable.GetType()].Enqueue(poolable);
        }
        
    }
}