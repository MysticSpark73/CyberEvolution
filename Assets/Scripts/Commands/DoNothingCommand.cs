namespace CyberEvolution.Commands
{
    public class DoNothingCommand : CommandBase
    {
        public DoNothingCommand(ICommandLogger logger, ICommandListener listener, float energyCost) : base(logger, listener, energyCost)
        {
        }

        public override void Execute()
        {
            base.Execute();
            _listener.DoNothing(_energyCost);
        }
    }
}