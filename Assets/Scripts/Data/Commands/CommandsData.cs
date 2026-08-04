using System;
using System.Linq;
using CyberEvolution.Commands;
using UnityEngine;

namespace CyberEvolution.Data.Commands
{
    [CreateAssetMenu(menuName = "CyberEvolution/Data/Commands", fileName = "CommandsData")]
    public class CommandsData : ScriptableObject
    {
        public CommandData[] data;

        public CommandData GetDataByType(CommandType type) => data.FirstOrDefault(i => i.Type == type);
        public CommandData[] GetAllowedCommands() => data;
    }

    [Serializable]
    public struct CommandData
    {
        public CommandType Type;
        public float EnergyCost;
    }
}