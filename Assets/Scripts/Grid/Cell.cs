using CyberEvolution.Entities;
using UnityEngine;

namespace CyberEvolution.Grid
{
    public class Cell : MonoBehaviour
    {
        public Vector2Int GridPosition { get; private set; }
        
        public Mob Mob { get; private set; }
        public Food Food { get; private set; }
        //can anything be spawned here
        public bool IsEmpty => !_isWall && Mob == null && Food == null;
        //can mob walk into here
        public bool IsWalkable => !_isWall && Mob == null;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private Sprite _tileSprite;
        private Sprite _wallSprite;
        private bool _isWall;


        public void Initialize(Vector2Int position, Sprite tileSprite, Sprite wallSprite, bool initializeAsWall = false)
        {
            GridPosition = position;
            _wallSprite = wallSprite;
            _tileSprite = tileSprite;
            _isWall = initializeAsWall;
            _spriteRenderer.sprite = _isWall ? _wallSprite : _tileSprite;
        }

        public void SetMob(Mob mob)
        {
            if (mob == null) return;
            
            Mob = mob;
            Mob.OnReturned += RemoveMob;
        }

        public void SetFood(Food food)
        {
            if (food == null) return;
            
            Food = food;
            Food.OnReturned += RemoveFood;
        }

        public void RemoveMob()
        {
            if (Mob == null) return;
            
            Mob.OnReturned -= RemoveMob;
            Mob = null;
        }

        private void RemoveFood()
        {
            if (Food == null) return;
            
            Food.OnReturned -= RemoveFood;
            Food = null;
        }
    }
}