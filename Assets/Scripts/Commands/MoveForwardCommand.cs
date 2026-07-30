
using UnityEngine;

namespace CyberEvolution.Commands
{
    public class MoveForwardCommand : CommandBase
    {
        public MoveForwardCommand(ICommandLogger logger, ICommandListener listener, float energyCost) : base(logger, listener, energyCost)
        {
        }

        public override void Execute()
        {
            base.Execute();
            _listener.MoveForward(_energyCost);
        }
    }
}