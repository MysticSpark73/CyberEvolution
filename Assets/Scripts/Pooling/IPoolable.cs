namespace CyberEvolution.Pooling
{
    public interface IPoolable
    {
        BasicPooler Pooler { get; set; }

        void Initialize(BasicPooler pooler);

        void OnSpawn();

        void OnReturn();
    }
}