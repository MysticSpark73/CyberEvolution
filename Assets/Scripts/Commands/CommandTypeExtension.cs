using System;
using Random = UnityEngine.Random;

namespace CyberEvolution.Commands
{
    public static class CommandTypeExtension
    {
        public static CommandType GetRandomCommand()
        {
            return (CommandType) Random.Range(0, Enum.GetNames(typeof(CommandType)).Length);
        }

        public static CommandType GetRandomCommand(CommandSearchParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}