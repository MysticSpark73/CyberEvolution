using System;
using CyberEvolution.Entities;
using UnityEngine;

namespace CyberEvolution.Data.Food
{
    [CreateAssetMenu(menuName = "CyberEvolution/Data/Food", fileName = "FoodData")]
    public class FoodData : ScriptableObject
    {
        public FoodDataItem PlantData;
        public FoodDataItem MeatData;

        public FoodDataItem GetFoodDataByType(FoodType foodType) => foodType switch
        {
            FoodType.Plant => PlantData,
            FoodType.Meat => MeatData,
            _ => throw new ArgumentOutOfRangeException(nameof(foodType), foodType, null)
        };
    }
}