using UnityEngine;

namespace CyberEvolution.Simulation.Colors
{
    public class LinearColorProvider : IColorProvider
    {
        private readonly float _step;
        private float _currentValue;

        public LinearColorProvider(int  linearColorsSetSize)
        {
            _step = 1f / linearColorsSetSize;
        }
        
        public Color GetNext()
        {
            Color color = Color.HSVToRGB(_currentValue, 1f, 1f);
            _currentValue = (_currentValue + _step) % 1;
            return color;
        }
    }
}