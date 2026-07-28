using System;
using UnityEngine;

namespace CyberEvolution.Commands
{
    public class MoveToCommand : CommandBase
    {
        private readonly Vector2Int _targetCellPosition;

        public MoveToCommand(ICommandLogger logger, float energyCost, Vector2Int targetCellPosition) : base(logger, energyCost)
        {
            _targetCellPosition = targetCellPosition;
        }

        public override void Execute(Action<float> callback = null)
        {
            //todo: get cell at target position
            //todo: validate move
            //todo: perform move if possible
            base.Execute(callback);
        }
    }
}