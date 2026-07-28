using CyberEvolution.Commands;
using CyberEvolution.Data.Commands;
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
        private const string CommandsDataPath = "Data/Commands/CommandsData";
        
        private const int _testGridWidth = 20;
        private const int _testGridHeigth = 20;
        
        [SerializeField] private Transform _gridContainer;
        [SerializeField] private BasicPooler _pooler;
        
        private TexturePack _texturePack;
        private GridData _gridData;
        private CommandsData _commandsData;
        
        private GridController _gridController;
        private MobsController _mobsController;
        private GenomeCache _genomeCache;
        private CommandsFactory _commandsFactory;
        
        private float _updateTime;
        private float _timeSinceLastUpdate;
        private bool _isPaused;
        private bool _isInitialized;

        private void Start()
        {
            LoadData();
            Setup();
        }

        private void Update()
        {
            if (!_isInitialized) return;
            if (_isPaused) return;
            
            _timeSinceLastUpdate += Time.deltaTime;

            if (_timeSinceLastUpdate >= _updateTime)
            {
                _timeSinceLastUpdate = 0;
                _gridController.UpdateCells();
            }
        }

        private void LoadData()
        {
            _texturePack = Resources.Load<TexturePack>(TextuePackPath);
            _gridData = Resources.Load<GridData>(GridDataPath);
            _commandsData = Resources.Load<CommandsData>(CommandsDataPath);
        }

        private void Setup()
        {
            _gridController = new GridController(_gridContainer, _gridData, _texturePack);
            _commandsFactory = new CommandsFactory(_commandsData, null);
            _genomeCache = new GenomeCache(_commandsFactory);
            _pooler.CreatePools(_testGridWidth * _testGridHeigth);
            _mobsController = new MobsController(_pooler, _gridController, _genomeCache);
            _gridController.CreateGrid(_testGridWidth, _testGridHeigth);
            //todo: Remember to set seed
            
            _isInitialized = true;
        }
    }
}