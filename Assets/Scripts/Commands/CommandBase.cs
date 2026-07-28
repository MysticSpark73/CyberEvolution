using System;

namespace CyberEvolution.Commands
{
    public abstract class CommandBase : ICommand
    {
        protected ICommandLogger _logger;
        protected float _energyCost;

        public CommandBase(ICommandLogger logger, float energyCost)
        {
            _logger = logger;
            _energyCost = energyCost;
        }

        public virtual void Execute(Action<float> callback = null)
        {
            _logger.Log(GetLogMessage());
            callback?.Invoke(_energyCost);
        }

        protected virtual string GetLogMessage()
        {
            return $"Executing {GetType().Name}";
        }
    }
}