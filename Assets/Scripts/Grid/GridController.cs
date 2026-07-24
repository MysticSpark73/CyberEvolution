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

        public GridController(Transform gridContainer, GridData gridData, TexturePack texturePack)
        {
            _gridContainer = gridContainer;
            _gridData = gridData;
            _texturePack = texturePack;
        }
        
        public void CreateGrid(int width, int height)
        {
            _grid = new Cell[width, height];
            
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    bool isWall = i == 0 || i == width - 1 || j == 0 || j == height - 1;
                    GameObject cellObject = Object.Instantiate(_gridData.TilePrefab, new Vector3(i, j, 0), Quaternion.identity, _gridContainer);
                    Cell cell = cellObject.GetComponent<Cell>();
                    if (cell == null)
                    {
                        continue;
                    }
                    
                    _grid[i, j] = cell;
                    cell.Initialize(i, j, _texturePack.TileSprite, _texturePack.WallSprite, isWall);
                }
            }
        }

        public Cell GetCell(int x, int y)
        {
            return _grid[x, y];
        }
    }
}