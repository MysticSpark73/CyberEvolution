using UnityEngine;

namespace CyberEvolution.Grid
{
    public class Cell : MonoBehaviour
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        
        //can anything be spawned here
        public bool IsEmpty => _isWall;
        //can mob walk into here
        public bool IsWalkable => !_isWall;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private Sprite _tileSprite;
        private Sprite _wallSprite;
        private bool _isWall;


        public void Initialize(int x, int y, Sprite tileSprite, Sprite wallSprite, bool initializeAsWall = false)
        {
            X = x;
            Y = y;
            _wallSprite = wallSprite;
            _tileSprite = tileSprite;
            _isWall = initializeAsWall;
            _spriteRenderer.sprite = _isWall ? _wallSprite : _tileSprite;
        }
    }
}