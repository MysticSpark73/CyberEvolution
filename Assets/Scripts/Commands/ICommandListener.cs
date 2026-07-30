using CyberEvolution.Entities;
using UnityEngine;

namespace CyberEvolution.Commands
{
    public interface ICommandListener
    {
        void Move(Vector2Int direction, float energyCost);
        void MoveForward(float energyCost);
        void Turn(int degrees, float energyCost);
        void DoNothing(float energyCost);
        void Consume(FoodType foodType, float energyCost);
        void Attack(Vector2Int direction, bool isAggressiveTowardsFriendly, float energyCost);
        void AttackForward(bool isAggressiveTowardsFriendly, float energyCost);
    }
}