using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Game.Tiles;
using _Scripts.New.ScriptableObjects.Setup;
using _Scripts.ScriptableObjects.Channels;
using _Scripts.ScriptableObjects.Setup;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Game
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private BoardTile BoardTilePrefab;
        [SerializeField] private MemoryTile TilePrefab;
        [SerializeField] private TileSettings TileSettings;
        [SerializeField] private GameChannel GameChannel;
        [SerializeField] private TileInfo BlackTileInfo;
        
        private LevelInfo _currentLevel;
        private List<MemoryTile> _tiles = new();
        private readonly List<List<BoardTile>> _board = new();
        public LevelInfo TempDefaultLevel;

        private void Awake()
        {
            GameChannel.OnBlackTileRevealedEvent += OnBlackTileRevealedEvent;
        }

        private void OnBlackTileRevealedEvent()
        {
            var allBoardTiles = _board.SelectMany(x => x).Where(x => x.TileType == TileType.RegularTile).ToList();
            var newBoardPositions = new List<List<Vector3>>();

            for (var i = 0; i < _board.Count; i++)
            {
                newBoardPositions.Add(new List<Vector3>());
                for (var j = 0; j < _board[i].Count; j++)
                {
                    if (_board[i][j].TileType != TileType.RegularTile)
                    {
                        continue;
                    }
                    
                    var randomTileIndex = Random.Range(0, allBoardTiles.Count);
                    var newPosition = allBoardTiles[randomTileIndex].transform.position;
                    newBoardPositions[i].Add(newPosition);

                    _board[i][j].Move(newPosition);
                    
                    Debug.DrawLine(_board[i][j].transform.position, newPosition, Color.red);
                    
                    allBoardTiles.RemoveAt(randomTileIndex);
                }   
            }
        }

        [ContextMenu("Setup Level")]
        private void SetupLevelInternal()
        {
            if (TempDefaultLevel == null) return;
            
            SetupLevel(TempDefaultLevel);
        }

        public void ClearLevel()
        {
            DestroyPreviousLevel();
        }
        
        public void SetupLevel(LevelInfo levelInfo)
        {
            DestroyPreviousLevel();

            GenerateTiles(levelInfo);

            SetupTiles(levelInfo);

            PositionTiles();
            
            GameChannel.OnLevelReady(_tiles.Count);
        }

        private void DestroyPreviousLevel()
        {
            foreach (var tile in _tiles)
            {
                Destroy(tile.gameObject);
            }

            _board.Clear();
        }

        private void PositionTiles()
        {
            var rowCount = _board.Count;
            var maxColCount = _board.Max(x => x.Count);

            var gap = .1f;
            
            var startZ = -1 * (rowCount * (TilePrefab.Size.z + gap) / 2);
            var startX = -1 * ((maxColCount * (TilePrefab.Size.x + gap)) / 2);

            for (int i = 0; i < rowCount; i++)
            {
                var startColIndex = maxColCount - _board[i].Count;
                
                for (int j = 0; j < _board[i].Count; j++)
                {
                    var jPosition = j + startColIndex;
                    _board[i][j].transform.position =
                        new Vector3(startX + jPosition * (TilePrefab.Size.x + gap),
                            0,
                            startZ + i * (TilePrefab.Size.z + gap));
                }
            }
        }

        private void SetupTiles(LevelInfo levelToSpawn)
        {
            var structureLines = levelToSpawn.LevelStructure.Split('\n');

            var tileIndex = 0;
            var rowIndex = 0;

            foreach (var line in structureLines)
            {
                _board.Add(new List<BoardTile>());
                
                foreach (var tileSign in line)
                {
                    if (tileSign == TileTypeConverter.GetValue(TileType.RegularTile))
                    {
                        _board[rowIndex].Add(_tiles[tileIndex]);
                        tileIndex++;
                    }
                    else if (tileSign == TileTypeConverter.GetValue(TileType.EmptyTile))
                    {
                        var emptyTile = Instantiate(BoardTilePrefab);
                        _board[rowIndex].Add(emptyTile);
                    }
                    else if (tileSign == TileTypeConverter.GetValue(TileType.BlackTile))
                    {
                        var blackTile = Instantiate(TilePrefab);
                        blackTile.Initialize(BlackTileInfo);
                        
                        _board[rowIndex].Add(blackTile);
                    }
                }

                rowIndex++;
            }
        }

        private void GenerateTiles(LevelInfo levelToSpawn)
        {
            var amountOfTiles = levelToSpawn.AmountOfTiles;

            if (amountOfTiles % 2 != 0)
            {
                Debug.LogError("Non event tile number");
            }

            var amountOfCouples = amountOfTiles / 2;
            var tileInfos = TileSettings.GetRandomUniqueTiles(amountOfCouples);

            for (var i = 0; i < amountOfTiles; i += 2)
            {
                var tileIndex = i / 2;
                var tileInfo = tileInfos[tileIndex];

                var firstTile = Instantiate(TilePrefab);
                firstTile.Initialize(tileInfo);

                _tiles.Add(firstTile);

                var secondTile = Instantiate(TilePrefab);
                secondTile.Initialize(tileInfo);

                _tiles.Add(secondTile);
            }

            _tiles = _tiles.OrderBy(x => Random.value).ToList();
        }
    }
}