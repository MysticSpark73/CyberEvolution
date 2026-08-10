using System;
using CyberEvolution.Entities;
using UnityEngine;

namespace CyberEvolution.Data.Food
{
    [Serializable]
    public struct FoodDataItem
    {
        public FoodType FoodType;
        public Color Color;
        public float EnergyValue;
        public float SpawnRate;
    }
}