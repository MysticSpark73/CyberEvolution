
namespace CyberEvolution.Commands
{
    public class DoNothingCommand : CommandBase
    {
        public DoNothingCommand(ICommandLogger logger, float energyCost) : base(logger, energyCost)
        {
        }
    }
}