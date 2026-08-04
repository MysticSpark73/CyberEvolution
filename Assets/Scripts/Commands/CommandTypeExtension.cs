using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace CyberEvolution.Commands
{
    public static class CommandTypeExtension
    {
        public static CommandType GetRandomCommand()
        {
            return (CommandType) Random.Range(0, (int) CommandType.AttackAll);
        }

        public static CommandType GetRandomCommand(CommandSearchParameters parameters)
        {
            List<CommandType> validCommands = new List<CommandType>();
            foreach (CommandType command in Enum.GetValues(typeof(CommandType)))
            {
                if (command == CommandType.UndefinedCommand) continue;
                if (parameters._excludeCommands.Contains(command)) continue;
                validCommands.Add(command);
            }

            return validCommands[Random.Range(0, validCommands.Count)];
        }

        public static CommandType GetRandomCommand(CommandType commandType) =>
            GetRandomCommand(new CommandSearchParameters() { _excludeCommands = new[] { commandType } });
    }
}