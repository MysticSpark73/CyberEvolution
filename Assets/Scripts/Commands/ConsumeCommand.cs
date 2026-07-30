using CyberEvolution.Entities;

namespace CyberEvolution.Commands
{
    public class ConsumeCommand : CommandBase
    {
        private readonly FoodType _foodType;

        public ConsumeCommand(ICommandLogger logger, ICommandListener listener, float energyCost, FoodType foodType) : base(logger, listener, energyCost)
        {
            _foodType = foodType;
        }

        public override void Execute()
        {
            base.Execute();
            _listener.Consume(_foodType, _energyCost);
        }
    }
}