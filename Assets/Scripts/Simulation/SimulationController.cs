using CyberEvolution.Data.Grid;
using CyberEvolution.Data.Visuals;
using CyberEvolution.Grid;
using UnityEngine;

namespace CyberEvolution.Simulation
{
    public class SimulationController : MonoBehaviour
    {
        private const string TextuePackPath = "Data/Visuals/TexturePack";
        private const string GridDataPath = "Data/Grid/GridData";
        
        [SerializeField] private Transform _gridContainer;
        
        private TexturePack _texturePack;
        private GridData _gridData;
        private GridController _gridController;


        private void Start()
        {
            LoadData();
            Setup();
            CreateGrid(20, 20);
        }

        private void LoadData()
        {
            _texturePack = Resources.Load<TexturePack>(TextuePackPath);
            _gridData = Resources.Load<GridData>(GridDataPath);
        }

        private void Setup()
        {
            _gridController = new GridController(_gridContainer, _gridData, _texturePack);
        }

        private void CreateGrid(int width, int height)
        {
            _gridController.CreateGrid(width, height);
        }
    }
}