using CyberEvolution.Data.Grid;
using CyberEvolution.Data.Visuals;
using CyberEvolution.Grid;
using CyberEvolution.Pooling;
using CyberEvolution.Simulation.Genomes;
using UnityEngine;

namespace CyberEvolution.Simulation
{
    public class SimulationController : MonoBehaviour
    {
        private const string TextuePackPath = "Data/Visuals/TexturePack";
        private const string GridDataPath = "Data/Grid/GridData";
        
        private const int _testGridWidth = 20;
        private const int _testGridHeigth = 20;
        
        [SerializeField] private Transform _gridContainer;
        [SerializeField] private BasicPooler _pooler;
        
        private TexturePack _texturePack;
        private GridData _gridData;
        private GridController _gridController;
        private MobsController _mobsController;
        private GenomeCache _genomeCache;

        private void Start()
        {
            LoadData();
            Setup();
        }

        private void LoadData()
        {
            _texturePack = Resources.Load<TexturePack>(TextuePackPath);
            _gridData = Resources.Load<GridData>(GridDataPath);
        }

        private void Setup()
        {
            _gridController = new GridController(_gridContainer, _gridData, _texturePack);
            _genomeCache = new GenomeCache();
            _pooler.CreatePools(_testGridWidth * _testGridHeigth);
            _mobsController = new MobsController(_pooler, _gridController);
            _gridController.CreateGrid(_testGridWidth, _testGridHeigth);
        }
    }
}