using CyberEvolution.Commands;

namespace CyberEvolution.Simulation.Genomes
{
    public interface IGenome
    {
        CommandType GetNextCommand(ref int ptr);
        GenomeBase Mutate(int id);
    }
}