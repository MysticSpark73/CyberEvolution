using CyberEvolution.Commands;
using UnityEngine;

namespace CyberEvolution.Simulation.Genomes
{
    public interface IGenome
    {
        CommandType GetNextCommand(ref int ptr, SensorData sensorData);
        GenomeBase Mutate(int id, Color color);
    }
}