using System;
using _Scripts.Game;
using UnityEngine;

namespace _Scripts.ScriptableObjects.Setup
{
    [CreateAssetMenu(fileName = "Tile Info", menuName = "Setup/Tile Info")]
    public class TileInfo: ScriptableObject
    {
        public Texture2D Texture;
        public string PairKey;
        public TileType TileType;

        private void OnEnable()
        {
            if (!string.IsNullOrEmpty(PairKey)) return;

            PairKey = Guid.NewGuid().ToString();
        }
    }
}