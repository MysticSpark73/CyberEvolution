using System.Collections.Generic;
using CyberEvolution.Commands;
using CyberEvolution.Simulation.Colors;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CyberEvolution.Simulation.Genomes
{
    public class GenomeCache
    {
        public int CurrentGenerationId { get; private set; }

        private Dictionary<int, GenomeBase> Genomes = new ();
        private readonly IColorProvider _colorProvider;
        private readonly float _mutationPercent;

        public GenomeCache(IColorProvider colorProvider, float mutationPercent)
        {
            _colorProvider = colorProvider;
            _mutationPercent = mutationPercent;
            CreateFirstGenome();
        }

        public CommandType GetNextCommand(int id, ref int ptr, SensorData sensorData)
        {
            if (Genomes.TryGetValue(id, out var genome))
            {
                return genome.GetNextCommand(ref ptr, sensorData);
            }

            return CommandType.UndefinedCommand;
        }

        public bool TryMutateGenome(int id, out int mutatedId)
        {
            mutatedId = id;
            int roll = Random.Range(0, 100);
            if (roll <= _mutationPercent * 100)
            {
                GenomeBase genome = Genomes[id];
                CurrentGenerationId++;
                mutatedId = CurrentGenerationId;
                Genomes.Add(CurrentGenerationId, genome.Mutate(CurrentGenerationId, _colorProvider.GetNext()));
                return true;
            }

            return false;
        }

        private void CreateFirstGenome()
        {
            CurrentGenerationId = 0;
            ComplexGenome complexGenome = new ComplexGenome(CurrentGenerationId, CreateRandomGenome(6 * (8 + 3)), _colorProvider.GetNext());
            // ComplexGenome complexGenome = new ComplexGenome(CurrentGenerationId, CreateIdealGenome(), _colorProvider.GetNext());
            Debug.Log(complexGenome.ToString());
            Genomes.Add(CurrentGenerationId, complexGenome);
        }

        private int[] CreateRandomGenome(int length)
        {
            int[] genome = new int[length];
            for (int i = 0; i < length; i++)
            {
                genome[i] = (int) CommandTypeExtension.GetRandomCommand();
            }
            
            return genome;
        }

        private int[] CreateIdealGenome()
        {
            return new[]
            {
                0, 0, 4, 5, 1, 6,
                1, 1, 4, 5, 1, 6,
                3, 1, 4, 5, 1, 1,
                2, 2, 4, 5, 2, 2,
                1, 1, 4, 5, 1, 1,
                3, 2, 3, 3, 2, 2,
                3, 1, 4, 5, 1, 1,
                2, 2, 4, 5, 2, 2,
                3, 1, 4, 5, 1, 2,
                3, 2, 4, 5, 2, 1,
                1, 2, 4, 5, 1, 2
            };
        }

        public Color GetColor(int generationId)
        {
            if (Genomes.TryGetValue(generationId, out var genome))
            {
                return genome.Color;
            }

            return Color.white;
        }
    }
}