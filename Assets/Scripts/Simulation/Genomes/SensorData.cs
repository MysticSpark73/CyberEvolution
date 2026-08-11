using CyberEvolution.Grid;
using UnityEngine;

namespace CyberEvolution.Simulation.Genomes
{
    public struct SensorData
    {
        public int SelfGenerationId;
        public bool WasAttacked;
        public Vector2Int AttackDirection;
        public Cell CellInFront;
    }
}