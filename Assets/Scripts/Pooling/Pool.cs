using System;
using UnityEngine;

namespace CyberEvolution.Pooling
{
    [Serializable]
    public struct Pool
    {
        public GameObject Prefab;
        public int PoolSize;
        public Transform Container;
        public bool UseDynamicPoolSize;
    }
}