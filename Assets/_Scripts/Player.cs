using System;
using _Scripts.Game;
using _Scripts.Game.Tiles;
using _Scripts.ScriptableObjects.Channels;
using _Scripts.ScriptableObjects.Variables;
using UnityEngine;

namespace _Scripts
{
    public class Player: MonoBehaviour
    {
        [SerializeField] private GameChannel GameChannel;

        [SerializeField] private BooleanVariable IsInputActive;

        private void Update()
        {
            if (!IsInputActive.Value || !Input.GetMouseButtonDown(0)) return;
            
            if (Camera.main is null) return;
            
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit)) return;
            
            var tile = hit.collider.GetComponentInParent<MemoryTile>();

            if (tile == null) return;
            GameChannel.OnTileClicked(tile);
        }
    }
}