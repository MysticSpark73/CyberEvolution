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
        private readonly float _plantSpawnRate;
        private readonly float _meatSpawnRate;

        private float _plantSpawnTimer;
        private float _meatSpawnTimer;

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
            TrySpawnPlant(deltaTime);
            TrySpawnMeat(deltaTime);
        }

        public float GetEnergyByType(FoodType type) => type switch
        {
            FoodType.Plant => _foodData.PlantData.EnergyValue,
            FoodType.Meat => _foodData.MeatData.EnergyValue,
            _ => throw new ArgumentOutOfRangeException(),
        };

        private void TrySpawnPlant(float deltaTime)
        {
            if (_plantSpawnRate <= 0f) return;
            
            _plantSpawnTimer += deltaTime;

            if (_plantSpawnTimer < _plantSpawnRate) return;
            
            _plantSpawnTimer -= _plantSpawnRate;
            
            SpawnFood(FoodType.Plant);
        }

        private void TrySpawnMeat(float deltaTime)
        {
            if (_meatSpawnRate <= 0f) return;
            
            _meatSpawnTimer += deltaTime;

            if (_meatSpawnTimer < _meatSpawnRate) return;
            
            _meatSpawnTimer -= _meatSpawnRate;
            
            SpawnFood(FoodType.Meat);
        }

        private void SpawnFood(FoodType type)
        {
            Cell cell = _gridController.GetRandomEmptyCell();
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