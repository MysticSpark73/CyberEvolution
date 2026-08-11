using System;
using CyberEvolution.Data.Food;
using CyberEvolution.Entities;
using CyberEvolution.Grid;
using CyberEvolution.Pooling;
using UnityEngine;

namespace CyberEvolution.Simulation
{
    public class FoodController : IUpdatable
    {
        private readonly BasicPooler _pooler;
        private readonly GridController _gridController;
        private readonly FoodData _foodData;
        private readonly int _plantSpawnRate;
        private readonly int _meatSpawnRate;

        private int _plantSpawnTimer;
        private int _meatSpawnTimer;

        public FoodController(BasicPooler pooler, GridController gridController, FoodData foodData)
        {
            _pooler = pooler;
            _gridController = gridController;
            _foodData = foodData;
            _plantSpawnRate = foodData.PlantData.SpawnRate;
            _meatSpawnRate = foodData.MeatData.SpawnRate;
        }

        public void Update(float deltaTime)
        {
            TrySpawnPlant();
            TrySpawnMeat();
        }

        public float GetEnergyByType(FoodType type) => type switch
        {
            FoodType.Plant => _foodData.PlantData.EnergyValue,
            FoodType.Meat => _foodData.MeatData.EnergyValue,
            _ => throw new ArgumentOutOfRangeException(),
        };

        public void OnMobDied(Vector2Int position)
        {
            Cell cell = _gridController.GetCell(position);
            if (cell == null || !cell.IsEmpty)
            {
                Debug.Log($"[FoodController][OnMobDied] Can't spawn food at {position}");
                return;
            }
            
            SpawnFood(FoodType.Meat, cell);
        }

        public void CreateInitialFood()
        {
            for (int i = 0; i < _foodData.PlantData.InitialAmount; i++)
            {
                SpawnFood(FoodType.Plant, _gridController.GetRandomEmptyCell());
            }

            for (int i = 0; i < _foodData.MeatData.InitialAmount; i++)
            {
                SpawnFood(FoodType.Meat, _gridController.GetRandomEmptyCell());
            }
        }

        private void TrySpawnPlant()
        {
            if (_plantSpawnRate <= 0f) return;

            _plantSpawnTimer++;

            if (_plantSpawnTimer < _plantSpawnRate) return;
            
            _plantSpawnTimer -= _plantSpawnRate;
            
            SpawnFood(FoodType.Plant, _gridController.GetRandomEmptyCell());
        }

        private void TrySpawnMeat()
        {
            if (_meatSpawnRate <= 0f) return;

            _meatSpawnTimer++;

            if (_meatSpawnTimer < _meatSpawnRate) return;
            
            _meatSpawnTimer -= _meatSpawnRate;
            
            SpawnFood(FoodType.Meat, _gridController.GetRandomEmptyCell());
        }

        private void SpawnFood(FoodType type, Cell cell)
        {
            FoodDataItem data = _foodData.GetFoodDataByType(type);
            
            if (cell == null)
            {
                Debug.LogError("[FoodController] [SpawnFood] There are no free cells!");
                return;
            }

            Food food = _pooler.Get<Food>();
            if (food == null)
            {
                Debug.LogError("[FoodController][SpawnFood] Failed to retrieve food from the pool!");
                return;
            }
            
            food.SetupOnGrid(cell.GridPosition, data);
            cell.SetFood(food);
        }
    }
}