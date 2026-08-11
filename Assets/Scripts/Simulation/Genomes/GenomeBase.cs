using CyberEvolution.Commands;
using UnityEngine;

namespace CyberEvolution.Simulation.Genomes
{
    public class GenomeBase : IGenome
    {
        public int ID { get; private set; }
        public Color Color { get; private set; }

        protected int[] _commands;

        public GenomeBase(int id, int[] commands, Color color)
        {
            ID = id;
            Color = color;
            _commands = commands;
        }

        public virtual CommandType GetNextCommand(ref int ptr, SensorData sensorData)
        {
            int commandValue = _commands[ptr];
            ptr = ((ptr + 1) % _commands.Length + _commands.Length) % _commands.Length;
            return (CommandType) commandValue;
        }

        public virtual GenomeBase Mutate(int id, Color color)
        {
            int[] newGenome = new int[_commands.Length];
            _commands.CopyTo(newGenome, 0);
            int replaceIndex = Random.Range(0, _commands.Length);
            _commands[replaceIndex] = (int) CommandTypeExtension.GetRandomCommand((CommandType) _commands[replaceIndex]);
            return CreateMutatedGenome(id, newGenome, color);
        }

        protected virtual GenomeBase CreateMutatedGenome(int id, int[] commands, Color color) => new(id, commands, color);
    }
}