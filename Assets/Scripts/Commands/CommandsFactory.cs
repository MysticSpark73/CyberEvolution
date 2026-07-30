using System;
using CyberEvolution.Data.Commands;
using CyberEvolution.Entities;

namespace CyberEvolution.Commands
{
    public class CommandsFactory
    {
        private CommandsData _data;
        private readonly ICommandLogger _logger;

        public CommandsFactory(CommandsData data, ICommandLogger logger)
        {
            _data = data;
            _logger = logger;
        }

        public CommandBase Create(CommandType commandType, ICommandListener listener)
        {
            CommandData commandData = _data.GetDataByType(commandType);
            switch (commandType)
            {
                case CommandType.DoNothing:
                    return new DoNothingCommand(_logger, listener, commandData.EnergyCost);
                case CommandType.TurnCW45:
                    return new TurnCommand(_logger, listener, commandData.EnergyCost, 45);
                case CommandType.TurnCCW45:
                    return new TurnCommand(_logger, listener, commandData.EnergyCost, -45);
                case CommandType.MoveForward:
                    return new MoveForwardCommand(_logger, listener, commandData.EnergyCost);
                case CommandType.ConsumePlant:
                    return new ConsumeCommand(_logger, listener, commandData.EnergyCost, FoodType.Plant);
                case CommandType.ConsumeMeat:
                    return new ConsumeCommand(_logger, listener, commandData.EnergyCost, FoodType.Meat);
                case CommandType.AttackEnemy:
                    return new AttackCommand(_logger, listener, commandData.EnergyCost, false);
                case CommandType.AttackAll:
                    return new AttackCommand(_logger, listener, commandData.EnergyCost, true);
                default:
                    throw new ArgumentOutOfRangeException(nameof(commandType), commandType, null);
            }
        }
    }
}