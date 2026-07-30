namespace CyberEvolution.Commands
{
    public class AttackCommand : CommandBase
    {
        private readonly bool _isAggressiveTowardsFriendly;

        public AttackCommand(ICommandLogger logger, ICommandListener listener, float energyCost, bool isAggressiveTowardsFriendly) : base(logger, listener, energyCost)
        {
            _isAggressiveTowardsFriendly = isAggressiveTowardsFriendly;
        }

        public override void Execute()
        {
            base.Execute();
            _listener.AttackForward(_isAggressiveTowardsFriendly, _energyCost);
        }
    }
}