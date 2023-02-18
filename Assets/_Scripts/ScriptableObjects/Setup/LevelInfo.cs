using System;
using System.Linq;
using _Scripts.Game;
using UnityEngine;

namespace _Scripts.ScriptableObjects.Setup
{
    [CreateAssetMenu(fileName = "Level Info", menuName = "Setup/Level Info")]
    public class LevelInfo: ScriptableObject
    {
        public int TimeCapInSeconds = 0;
        public string Name;
        public int Order;

        [TextArea(2,15)]
        public string LevelStructure;

        [HideInInspector] public int AmountOfTiles;
        
        private void OnEnable()
        {
            var structureLines = LevelStructure.Split('\n');

            AmountOfTiles = 0;
            
            foreach (var line in structureLines)
            {
                AmountOfTiles += line.Count(x => x == TileTypeConverter.GetValue(TileType.RegularTile));
            }
        }
    }
}