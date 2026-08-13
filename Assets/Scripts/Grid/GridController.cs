using System.Collections.Generic;
using CyberEvolution.Data.Grid;
using CyberEvolution.Data.Visuals;
using UnityEngine;

namespace CyberEvolution.Grid
{
    public class GridController
    {
        private Cell[,] _grid;

        private readonly Transform _gridContainer;
        private readonly GridData _gridData;
        private readonly TexturePack _texturePack;
        
        private int _gridWidth;
        private int _gridHeight;
        
        public GridController(Transform gridContainer, GridData gridData, TexturePack texturePack)
        {
            _gridContainer = gridContainer;
            _gridData = gridData;
            _texturePack = texturePack;
        }
        
        public void CreateGrid(int width, int height)
        {
            _grid = new Cell[width, height];
            _gridWidth = width;
            _gridHeight = height;
            
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    bool isWall = i == 0 || i == width - 1 || j == 0 || j == height - 1;
                    GameObject cellObject = Object.Instantiate(_gridData.TilePrefab, new Vector3(i, j, 0), Quaternion.identity, _gridContainer);
                    Cell cell = cellObject.GetComponent<Cell>();
                    if (cell == null) continue;
                    
                    _grid[i, j] = cell;
                    cell.Initialize(new Vector2Int(i, j), _texturePack.TileSprite, _texturePack.WallSprite, isWall);
                }
            }
        }

        public Cell GetRandomEmptyCell()
        {
            List<Cell> emptyCells = new();
            for (int i = 1; i < _gridWidth - 1; i++)
            {
                for (int j = 1; j < _gridHeight - 1; j++)
                {
                    if (_grid[i, j].IsEmpty)
                    {
                        emptyCells.Add(_grid[i, j]);
                    }
                }
            }

            if (emptyCells.Count == 0) return null;
            
            return emptyCells[Random.Range(0, emptyCells.Count)];
        }

        public Cell GetFirstEmptyCellNearby(Vector2Int position)
        {
            int x, y;
            for (int i = -1; i < 1; i++)
            {
                for (int j = -1; j < 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    x = position.x + i;
                    y = position.y + j;
                    if (IsOutOfBounds(new Vector2Int(x, y))) continue;
                    if (_grid[x, y].IsEmpty) return _grid[x, y];
                }
            }

            return null;
        }
        
        public Cell GetRandomEmptyCellNearby(Vector2Int position)
        {
            List<Cell> emptyCells = new();
            int x, y;
            for (int i = -1; i < 1; i++)
            {
                for (int j = -1; j < 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    x = position.x + i;
                    y = position.y + j;
                    if (IsOutOfBounds(new Vector2Int(x, y))) continue;
                    if (_grid[x, y].IsEmpty) emptyCells.Add(_grid[x, y]);
                }
            }

            if (emptyCells.Count == 0) return null;
            return emptyCells[Random.Range(0, emptyCells.Count)];
        }

        public Cell GetFirstWalkableCellNearby(Vector2Int position)
        {
            int x, y;
            for (int i = -1; i < 1; i++)
            {
                for (int j = -1; j < 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    x = position.x + i;
                    y = position.y + j;
                    if (IsOutOfBounds(new Vector2Int(x, y))) continue;
                    if (_grid[x, y].IsWalkable) return _grid[x, y];
                }
            }

            return null;
        }

        public Cell GetRandomWalkableCellNearby(Vector2Int position)
        {
            int x, y;
            List<Cell> walkableCells = new();
            for (int i = -1; i < 1; i++)
            {
                for (int j = -1; j < 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    x = position.x + i;
                    y = position.y + j;
                    if (IsOutOfBounds(new Vector2Int(x, y))) continue;
                    if (_grid[x, y].IsWalkable)
                    {
                        walkableCells.Add(_grid[x, y]);
                    }
                }
            }

            if (walkableCells.Count == 0) return null;
            return walkableCells[Random.Range(0, walkableCells.Count)];
        }

        private bool IsOutOfBounds(Vector2Int position)
        {
            return position.x < 0 || position.x > _gridWidth || position.y < 0 || position.y > _gridHeight;
        }

        public Cell GetCell(Vector2Int position)
        {
            if (position.x < 0 || position.x >= _gridWidth || position.y < 0 || position.y >= _gridHeight) return null;
            return _grid[position.x, position.y];
        }
    }
}