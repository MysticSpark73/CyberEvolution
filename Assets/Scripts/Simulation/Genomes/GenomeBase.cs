using CyberEvolution.Commands;

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
            //todo: mutate random command
            return new GenomeBase(id, newGenome);
        }
    }
}