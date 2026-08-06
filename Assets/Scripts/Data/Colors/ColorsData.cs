using UnityEngine;

namespace CyberEvolution.Data.Colors
{
    [CreateAssetMenu(menuName = "CyberEvolution/Data/Colors", fileName = "ColorsData")]
    public class ColorsData : ScriptableObject
    {
        public bool useSetColors;
        public int linearColorsSetSize = 32;
        public Color[] Colors;
        
    }
}