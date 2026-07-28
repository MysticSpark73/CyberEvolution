using System;
using CyberEvolution.Data.Commands;

namespace CyberEvolution.Commands
{
    public class CommandsFactory
    {
        private CommandsData _data;
        private ICommandLogger _logger;

        public CommandsFactory(CommandsData data, ICommandLogger logger)
        {
            _data = data;
            _logger = logger;
        }

        public T CreateCommand<T>() where T : class, ICommand
        {
            throw new NotImplementedException();
        }
    }
}