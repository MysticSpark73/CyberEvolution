using System;

namespace CyberEvolution.Commands
{
    public abstract class CommandBase : ICommand
    {
        private readonly ICommandLogger _logger;
        protected readonly ICommandListener _listener;
        protected readonly float _energyCost;

        protected CommandBase(ICommandLogger logger, ICommandListener listener, float energyCost)
        {
            _logger = logger;
            _listener = listener;
            _energyCost = energyCost;
        }

        public virtual void Execute()
        {
            _logger.Log(this, GetLogMessage());
        }

        public virtual void Undo()
        {
            throw new NotImplementedException();
        }

        protected virtual string GetLogMessage()
        {
            return $"Executing {GetType().Name}";
        }
    }
}