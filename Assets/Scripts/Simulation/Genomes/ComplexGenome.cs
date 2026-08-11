using System;
using CyberEvolution.Commands;
using CyberEvolution.Entities;
using CyberEvolution.Grid;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CyberEvolution.Simulation.Genomes
{
    public class ComplexGenome : GenomeBase
    {
        public const int ColumnsCount = 6;
        public const int DirectionalRowsCount = 8;
        private int GenericGenomeStartIndex => ColumnsCount * DirectionalRowsCount;
        private int GenericGenomeLength => _commands.Length - GenericGenomeStartIndex;
        private int GenericGenomeRowCount => GenericGenomeLength / ColumnsCount;
        
        public ComplexGenome(int id, int[] commands, Color color) : base(id, commands, color)
        {
            
        }

        public override CommandType GetNextCommand(ref int ptr, SensorData sensorData)
        {
            int x, y;
            if (sensorData.WasAttacked)
            {
                x = GetColumnByCell(sensorData.CellInFront, sensorData.SelfGenerationId);
                y = GetRowByDirection(sensorData.AttackDirection);
                return (CommandType)_commands[ColumnsCount * y + x];
            }

            /*int commandValue = _commands[GenericGenomeStartIndex + ptr];

            ptr = ((ptr + 1) % GenericGenomeLength + GenericGenomeLength) % GenericGenomeLength;

            return (CommandType)commandValue;*/

            x = GetColumnByCell(sensorData.CellInFront, sensorData.SelfGenerationId);
            y = DirectionalRowsCount + ptr;
            ptr = ((ptr + 1) % GenericGenomeRowCount + GenericGenomeRowCount) % GenericGenomeRowCount;
            return (CommandType) _commands[ColumnsCount * y + x];
        }

        private int GetRowByDirection(Vector2Int direction)
        {
            if (direction.Equals(Vector2Int.up)) return 0;
            if (direction.Equals(Vector2Int.right)) return 1;
            if (direction.Equals(Vector2Int.down)) return 2;
            if (direction.Equals(Vector2Int.left)) return 3;
            if (direction.Equals(Vector2Int.up + Vector2Int.right)) return 4;
            if (direction.Equals(Vector2Int.down + Vector2Int.right)) return 5;
            if (direction.Equals(Vector2Int.down + Vector2Int.left)) return 6;
            if (direction.Equals(Vector2Int.up + Vector2Int.left)) return 7;
            throw new ArgumentOutOfRangeException($"[ComplexGenome][GetRowByDirection] Vector {direction} didnt match any direction!");
        }

        protected override GenomeBase CreateMutatedGenome(int id, int[] commands, Color color) =>
            new ComplexGenome(id, commands, color);

        private int GetColumnByCell(Cell cell, int generationId)
        {
            if (cell == null) return 1;
            if (cell.IsWall) return 1;
            if (cell.IsEmpty) return 0;
            if (cell.Food != null)
            {
                if (cell.Food.Type == FoodType.Plant) return 2;
                if (cell.Food.Type == FoodType.Meat) return 3;
            }

            if (cell.Mob != null)
            {
                return cell.Mob.IsFriendly(generationId) ? 4 : 5;
            }
            throw new ArgumentOutOfRangeException($"[ComplexGenome][GetColumnByCell] Cell at {cell.GridPosition} didn't satisfy any condition!");
        }

        public override string ToString()
        {
            string result = "";
            result += $"===== Complex Genome {ID} =====\n";
            for (int i = 0; i < _commands.Length / ColumnsCount; i++)
            {
                for (int j = 0; j < ColumnsCount; j++)
                {
                    result += $" {_commands[ColumnsCount * i + j]} ";
                }

                result += "\n";
            }

            result += "===============";
            return result;
        }
    }
}