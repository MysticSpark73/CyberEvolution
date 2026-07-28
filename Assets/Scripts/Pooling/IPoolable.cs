using System;

namespace CyberEvolution.Pooling
{
    public interface IPoolable
    {
        event Action OnReturned;
        BasicPooler Pooler { get; }

        void InitializePoolable(BasicPooler pooler);

        void OnSpawn();

        void OnReturn();
    }
}