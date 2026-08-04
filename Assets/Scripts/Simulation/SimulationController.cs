using CyberEvolution.Commands;
using CyberEvolution.Data.Commands;
using CyberEvolution.Data.Grid;
using CyberEvolution.Data.Visuals;
using CyberEvolution.Grid;
using CyberEvolution.Pooling;
using CyberEvolution.Simulation.Genomes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CyberEvolution.Simulation
{
    public class SimulationController : MonoBehaviour
    {
        private const string TexturePackPath = "Data/Visuals/TexturePack";
        private const string GridDataPath = "Data/Grid/GridData";
        private const string CommandsDataPath = "Data/Commands/CommandsData";
        
        private const int _testGridWidth = 20;
        private const int _testGridHeight = 20;
        
        [SerializeField] private Transform _gridContainer;
        [SerializeField] private BasicPooler _pooler;
        
        private TexturePack _texturePack;
        private GridData _gridData;
        private CommandsData _commandsData;
        
        private GridController _gridController;
        private MobsController _mobsController;
        private GenomeCache _genomeCache;
        private CommandsFactory _commandsFactory;
        private ICommandLogger _commandLogger;
        
        private float _updateTime = .5f;
        private float _timeSinceLastUpdate;
        private bool _isPaused;
        private bool _isInitialized;
        private readonly float _mutationPercent = .2f;
        private int _seed;
        private readonly float _energyToReproduce = 50;
        private readonly float _mobAttackDamage = 50;
        private readonly int _initialPopulation = 5;

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
                _mobsController.Update();
            }
        }

        private void LoadData()
        {
            _texturePack = Resources.Load<TexturePack>(TexturePackPath);
            _gridData = Resources.Load<GridData>(GridDataPath);
            _commandsData = Resources.Load<CommandsData>(CommandsDataPath);
        }

        private void Setup()
        {
            SetupSeed();
            CreateSystems();
            SetupBoard();

            _isInitialized = true;
        }

        private void CreateSystems()
        {
            _commandLogger = new DebugLogger();
            _genomeCache = new GenomeCache(_mutationPercent);
            _gridController = new GridController(_gridContainer, _gridData, _texturePack);
            _commandsFactory = new CommandsFactory(_commandsData, _commandLogger);
            _mobsController = new MobsController(_pooler, _gridController, _genomeCache, _commandsFactory, _energyToReproduce, _mobAttackDamage);
        }

        private void SetupBoard()
        {
            _pooler.CreatePools(_testGridWidth * _testGridHeight);
            _gridController.CreateGrid(_testGridWidth, _testGridHeight);
            SpawnMobs();
        }

        private void SpawnMobs()
        {
            for (int i = 0; i < _initialPopulation; i++)
            {
                _mobsController.SpawnMob(_gridController.GetRandomEmptyCell().GridPosition, _genomeCache.CurrentGenerationId, _energyToReproduce * .5f);
            }
        }

        private void SetupSeed()
        {
            //todo: check data for seed options (random/custom)
            _seed = Application.productName.GetHashCode();
            Random.InitState(_seed);
        }
    }
}