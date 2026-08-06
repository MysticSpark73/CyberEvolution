using UnityEngine;

namespace CyberEvolution.Simulation.Colors
{
    public interface IColorProvider
    {
        Color GetNext();
    }
}