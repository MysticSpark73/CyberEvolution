using System;
using System.Collections.Generic;
using CyberEvolution.Commands;
using CyberEvolution.Data.Colors;
using CyberEvolution.Data.Commands;
using CyberEvolution.Data.Food;
using CyberEvolution.Data.Grid;
using CyberEvolution.Data.Visuals;
using CyberEvolution.Grid;
using CyberEvolution.Pooling;
using CyberEvolution.Simulation.Colors;
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
        private const string ColorsDataPath = "Data/Colors/ColorsData";
        private const string FoodDataPath = "Data/Food/FoodData";
        
        private const int _testGridWidth = 20;
        private const int _testGridHeight = 20;
        
        [SerializeField] private Transform _gridContainer;
        [SerializeField] private BasicPooler _pooler;
        
        private TexturePack _texturePack;
        private GridData _gridData;
        private CommandsData _commandsData;
        private ColorsData _colorsData;
        private FoodData _foodData;
        
        private GridController _gridController;
        private FoodController _foodController;
        private MobsController _mobsController;
        private GenomeCache _genomeCache;
        private CommandsFactory _commandsFactory;
        private ColorsProviderFactory _colorsProviderFactory;
        private ICommandLogger _commandLogger;
        private IColorProvider _colorProvider;
        
        private List<IUpdatable> _updateables = new ();
        
        private float _timeSinceLastUpdate;
        private bool _isPaused;
        private bool _isInitialized;
        
        //todo: values that should be set from data/ui
        private int _seed;
        private float _updateTime = .25f;
        private readonly float _mutationPercent = .2f;
        private readonly float _energyToReproduce = 100;
        private readonly float _initialEnergy = 80;
        private readonly float _mobAttackDamage = 25;
        private readonly int _initialPopulation = 10;

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
                foreach (var updatable in _updateables)
                {
                    updatable.Update(_timeSinceLastUpdate);
                }
                _timeSinceLastUpdate = 0;
            }
        }

        private void LoadData()
        {
            _texturePack = Resources.Load<TexturePack>(TexturePackPath);
            _gridData = Resources.Load<GridData>(GridDataPath);
            _commandsData = Resources.Load<CommandsData>(CommandsDataPath);
            _colorsData = Resources.Load<ColorsData>(ColorsDataPath);
            _foodData = Resources.Load<FoodData>(FoodDataPath);
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
            _colorsProviderFactory = new ColorsProviderFactory(_colorsData);
            _colorProvider = _colorsProviderFactory.Create();
            _genomeCache = new GenomeCache(_colorProvider, _mutationPercent);
            _gridController = new GridController(_gridContainer, _gridData, _texturePack);
            _commandsFactory = new CommandsFactory(_commandsData, _commandLogger);
            _foodController = new FoodController(_pooler, _gridController, _foodData);
            _mobsController = new MobsController(_foodController, _pooler, _gridController, _genomeCache,
                _commandsFactory, _energyToReproduce, _mobAttackDamage, _initialPopulation, _initialEnergy);
            
            _updateables.Add(_foodController);
            _updateables.Add(_mobsController);
        }

        private void SetupBoard()
        {
            _pooler.CreatePools(_testGridWidth * _testGridHeight);
            _gridController.CreateGrid(_testGridWidth, _testGridHeight);
            _foodController.CreateInitialFood();
            _mobsController.CreateInitialPopulation();
        }

        private void SetupSeed()
        {
            //todo: check data for seed options (random/custom)
            // _seed = Application.productName.GetHashCode();
            _seed = DateTime.Now.Millisecond;
            Debug.Log($"Seed: {_seed}");
            Random.InitState(_seed);
        }
    }
}