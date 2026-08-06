using CyberEvolution.Data.Colors;

namespace CyberEvolution.Simulation.Colors
{
    public class ColorsProviderFactory
    {
        private ColorsData _data;

        public ColorsProviderFactory(ColorsData data)
        {
            _data = data;
        }

        public IColorProvider Create()
        {
            if (_data.useSetColors)
            {
                return new DiscreteColorProvider(_data.Colors);
            }

            return new LinearColorProvider(_data.linearColorsSetSize);
        }
    }
}