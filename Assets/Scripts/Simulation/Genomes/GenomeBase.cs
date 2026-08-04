using CyberEvolution.Commands;
using UnityEngine;

namespace CyberEvolution.Simulation.Genomes
{
    public class GenomeBase : IGenome
    {
        public int ID { get; private set; }
        
        protected int[] _commands;

        public GenomeBase(int id, int[] commands)
        {
            ID = id;
            _commands = commands;
        }

        public virtual CommandType GetNextCommand(ref int ptr)
        {
            int commandValue = _commands[ptr];
            ptr = ((ptr + 1) % _commands.Length + _commands.Length) % _commands.Length;
            return (CommandType) commandValue;
        }

        public GenomeBase Mutate(int id)
        {
            int[] newGenome = new int[_commands.Length];
            _commands.CopyTo(newGenome, 0);
            int replaceIndex = Random.Range(0, _commands.Length);
            _commands[replaceIndex] = (int) CommandTypeExtension.GetRandomCommand((CommandType) _commands[replaceIndex]);
            return new GenomeBase(id, newGenome);
        }
    }
}