using _Scripts.Game;
using _Scripts.Game.Tiles;
using _Scripts.ScriptableObjects.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.ScriptableObjects.Channels
{
    [CreateAssetMenu(fileName = "Game Channel", menuName = "Channels/Game")]
    public class GameChannel: ScriptableObject
    {
        public UnityAction<MemoryTile> OnTileClickedEvent;
        public UnityAction<int> OnLevelReadyEvent;
        public UnityAction OnBlackTileRevealedEvent;
        
        [SerializeField] private BooleanVariable IsInputActive;

        public void OnLevelReady(int tileCount)
        {
            OnLevelReadyEvent?.Invoke(tileCount);
            IsInputActive.Value = true;
        }
        
        public void OnTileClicked(MemoryTile memoryTile)
        {
            OnTileClickedEvent?.Invoke(memoryTile);
        }

        public void OnBlackTileRevealed()
        {
            OnBlackTileRevealedEvent?.Invoke();
        }
    }
}