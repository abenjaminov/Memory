using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.ScriptableObjects.Setup;
using UnityEngine;

namespace _Scripts.New.ScriptableObjects.Setup
{
    [CreateAssetMenu(fileName = "Tile Settings", menuName = "Setup/Tile Settings")]
    public class TileSettings: ScriptableObject
    {
        public List<TileInfo> Tiles;

        public List<TileInfo> GetRandomUniqueTiles(int amount)
        {
            if (Tiles == null || Tiles.Count == 0 || amount > Tiles.Count)
            {
                throw new Exception("Not enough Textures for tiles");
            }

            var uniqueTextures = new List<TileInfo>();

            var texturesCopy = Tiles.Select(x => x).ToList();

            for (var i = 0; i < amount; i++)
            {
                var randomIndex = UnityEngine.Random.Range(0, texturesCopy.Count);
                
                var texture = texturesCopy[randomIndex];
                uniqueTextures.Add(texture);
                
                texturesCopy.RemoveAt(randomIndex);
            }

            return uniqueTextures;
        }
    }
}