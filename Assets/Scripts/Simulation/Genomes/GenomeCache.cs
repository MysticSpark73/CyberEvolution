using System.Collections.Generic;
using CyberEvolution.Commands;
using Random = UnityEngine.Random;

namespace CyberEvolution.Simulation.Genomes
{
    public class GenomeCache
    {
        public int CurrentGenerationId { get; private set; }

        private Dictionary<int, GenomeBase> Genomes = new ();
        private readonly float _mutationPercent;

        public GenomeCache(float mutationPercent)
        {
            _mutationPercent = mutationPercent;
            CreateFirstGenome();
        }

        public CommandType GetNextCommand(int id, ref int ptr)
        {
            if (Genomes.TryGetValue(id, out var genome))
            {
                return genome.GetNextCommand(ref ptr);
            }

            return CommandType.UndefinedCommand;
        }

        public bool TryMutateGenome(int id, out int mutatedId)
        {
            mutatedId = id;
            if (Random.Range(0, 100) <= 1 - _mutationPercent)
            {
                GenomeBase genome = Genomes[id];
                CurrentGenerationId++;
                mutatedId = CurrentGenerationId;
                Genomes.Add(CurrentGenerationId, genome.Mutate(CurrentGenerationId));
                return true;
            }

            return false;
        }

        private void CreateFirstGenome()
        {
            CurrentGenerationId = 0;
            GenomeBase genomeBase = new GenomeBase(CurrentGenerationId, CreateRandomGenome(64));
            Genomes.Add(CurrentGenerationId, genomeBase);
        }

        private int[] CreateRandomGenome(int length)
        {
            int[] genome =  new int[length];
            for (int i = 0; i < length; i++)
            {
                genome[i] = (int) CommandTypeExtension.GetRandomCommand();
            }

            return genome;
        }
    }
}