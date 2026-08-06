using UnityEngine;

namespace CyberEvolution.Simulation.Colors
{
    public class DiscreteColorProvider : IColorProvider
    {
        private readonly Color[] _colors;
        private int _currentIndex;

        public DiscreteColorProvider(Color[] colors)
        {
            _colors = colors;
        }

        public Color GetNext()
        {
            Color color = _colors[_currentIndex];
            _currentIndex = (_currentIndex + 1) % _colors.Length;
            return color;
        }
    }
}