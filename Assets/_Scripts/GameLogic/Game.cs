using System.Collections.Generic;
using _Scripts.Game;
using _Scripts.Game.Tiles;
using _Scripts.ScriptableObjects.Channels;
using _Scripts.ScriptableObjects.Setup;
using _Scripts.ScriptableObjects.Variables;
using _Scripts.State;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace _Scripts.GameLogic
{
    [RequireComponent(typeof(LevelController))]
    public class Game: MonoBehaviour
    {
        private StateMachine _stateMachine;
        
        private int _amountSelected = 0;
        [SerializeField] private GameChannel GameChannel;

        private int _tileCount;
        private readonly List<MemoryTile> _selectedTiles = new();

        private LevelController _levelController;
        [SerializeField] private BooleanVariable IsInputActive;
        
        private LoadLevelState _loadLevelState;
        private NewTurnState _newTurnState;
        private LevelTransitionState _levelTransitionState;
        
        private bool _isTileRevealing = false;
        
        private void Awake()
        {
            GameChannel.OnLevelReadyEvent += OnLevelReadyEvent;
            GameChannel.OnTileClickedEvent += OnTileClicked;
            _levelController = GetComponent<LevelController>();
        }

        private void SetupStateMachine()
        {
            _stateMachine = new StateMachine();

            _loadLevelState = new LoadLevelState(_levelController);
            _newTurnState = new NewTurnState(IsInputActive);
            _levelTransitionState = new LevelTransitionState(IsInputActive);

            var loadToNew = new Transition()
            {
                Origin = _loadLevelState,
                Target = _newTurnState,
                Predicate = () => _loadLevelState.IsLevelLoaded
            };
            _stateMachine.AddTransition(loadToNew);

            var anyToLevelTransition = new Transition()
            {
                Origin = null,
                Target = _levelTransitionState,
                Predicate = () => _isTileRevealing
            };
            
            _stateMachine.AddTransition(anyToLevelTransition);

        }

        private void OnLevelStart(LevelInfo levelInfo)
        {
            _loadLevelState.SetLevelToLoad(levelInfo);
            _stateMachine.SetState(_loadLevelState);
        }
        
        private void OnLevelReadyEvent(int tileCount)
        {
            _tileCount = tileCount;
        }

        public void OnTileClicked(MemoryTile memoryTile)
        {
            _isTileRevealing = true;
            
            if (memoryTile.TileType == TileType.BlackTile)
            {
                memoryTile.OnTileRevealedEvent += OnBlackTileRevealed;
            }
            else
            {
                memoryTile.OnTileRevealedEvent += OnTileRevealedEvent;
                _selectedTiles.Add(memoryTile);
                _amountSelected++;
            }
            
            memoryTile.Reveal();
        }

        private void OnBlackTileRevealed(MemoryTile blackTile)
        {
            blackTile.OnTileRevealedEvent -= OnTileRevealedEvent;
            
            GameChannel.OnBlackTileRevealed();
        }
        
        private void OnTileRevealedEvent(MemoryTile memoryTile)
        {
            memoryTile.OnTileRevealedEvent -= OnTileRevealedEvent;
            
            if (_amountSelected != 2) return;
            
            if (_selectedTiles[0].PairKey == _selectedTiles[1].PairKey)
            {
                _tileCount -= 2;

                if (_tileCount == 0)
                {
                    Debug.Log("Win");
                }
                else
                {
                    StartNewTurn();
                }
            }
            else
            {
                RevertTurn();
            }
        }

        private void StartNewTurn()
        {
            _amountSelected = 0;
            _selectedTiles.Clear();
        }
        
        private void RevertTurn()
        {
            foreach (var selectedTile in _selectedTiles)
            {
                selectedTile.Hide();
            }
                        
            StartNewTurn();
        }
    }
}