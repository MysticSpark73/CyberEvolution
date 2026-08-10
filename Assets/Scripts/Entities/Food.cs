using System;
using CyberEvolution.Data.Food;
using CyberEvolution.Pooling;
using UnityEngine;

namespace CyberEvolution.Entities
{
    public class Food : MonoBehaviour, IPoolable
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        public event Action OnReturned;
        public BasicPooler Pooler { get; private set; }
        
        private Vector2Int _gridPosition;
        public FoodType Type { get; private set; }

        public void InitializePoolable(BasicPooler pooler)
        {
            Pooler = pooler;
            gameObject.SetActive(false);
        }

        public void OnSpawn()
        {
            gameObject.SetActive(true);
        }

        public void OnReturn()
        {
            gameObject.SetActive(false);
            OnReturned?.Invoke();
        }
        
        public void Consume()
        {
            Pooler.Return(this);
        }

        public void SetupOnGrid(Vector2Int gridPosition,FoodDataItem data)
        {
            _gridPosition = gridPosition;
            transform.position = new Vector3(gridPosition.x, gridPosition.y, 0);
            
            Type = data.FoodType;
            _spriteRenderer.color = data.Color;
        } 
    }
}