using System.Collections.Generic;
using CyberEvolution.Commands;

namespace CyberEvolution.Simulation.Genomes
{
    public class GenomeCache
    {
        private CommandsFactory _commandsFactory;
        
        private Dictionary<int, GenomeBase> Genomes = new ();
        private int currentGenerationId;

        public GenomeCache(CommandsFactory commandsFactory)
        {
            _commandsFactory = commandsFactory;
            
            CreateFirstGenome();
        }

        public void GetNextCommand(int id, ref int ptr)
        {
            //todo: return command
        }

        public void MutateGenome(int id)
        {
            //todo: get genome by id
            //todo: get mutated genome from it's ancestor
            //todo: increase current id
            //todo: add genome with new id to cache
        }

        private void CreateFirstGenome()
        {
            //todo: create first genome based on input parameters: random/ from data
        }
    }
}