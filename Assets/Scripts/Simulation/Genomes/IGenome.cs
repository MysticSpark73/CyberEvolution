namespace CyberEvolution.Simulation.Genomes
{
    public interface IGenome
    {
        void GetNextCommand(ref int ptr);
        GenomeBase Mutate(int id);
    }
}