using UnityEngine;

namespace CyberEvolution.Data.Visuals
{
    [CreateAssetMenu(menuName = "CyberEvolution/Data/Textures", fileName = "TexturePack")]
    public class TexturePack : ScriptableObject
    {
        public Sprite TileSprite;
        public Sprite WallSprite;
        public Sprite MobSprite;
        public Sprite FoodSprite;
        public Sprite OutlineSprite;
    }
}