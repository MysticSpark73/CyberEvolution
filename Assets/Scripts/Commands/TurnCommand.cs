namespace CyberEvolution.Commands
{
    public class TurnCommand : CommandBase
    {
        private readonly int _turnDegrees;

        public TurnCommand(ICommandLogger logger, ICommandListener listener, float energyCost, int turnDegrees) : base(logger, listener, energyCost)
        {
            _turnDegrees = turnDegrees;
        }

        public override void Execute()
        {
            base.Execute();
            _listener.Turn(_turnDegrees, _energyCost);
        }
    }
}