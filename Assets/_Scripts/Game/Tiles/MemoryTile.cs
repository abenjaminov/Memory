using System;
using _Scripts.New.ScriptableObjects;
using _Scripts.ScriptableObjects.Setup;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Game.Tiles
{
    [RequireComponent(typeof(RotateOverTime))]
    [RequireComponent(typeof(BoxCollider))]
    public class MemoryTile: BoardTile
    {
        protected static readonly int MainTex = Shader.PropertyToID("_MainTex");
        
        public bool IsHidden { get; private set; }
        public bool IsFrozen { get; private set; }
        
        
        protected Material Material;

        public UnityAction<MemoryTile> OnTileRevealedEvent;
        public UnityAction<MemoryTile> OnTileHiddenEvent;

        protected override void Awake()
        {
            base.Awake();
            
            Material = GetComponent<MeshRenderer>().materials[0];
            IsHidden = true;
        }

        public override void Initialize(TileInfo tileInfo)
        {
            base.Initialize(tileInfo);
            Material.SetTexture(MainTex, tileInfo.Texture);
        }
        
        [ContextMenu("Reveal")]
        public void Reveal()
        {
            if (!IsHidden || IsFrozen) return;
            
            _rotateOverTime.Rotate(new Vector3(0,0, 180), 350, () =>
            {
                IsHidden = false;
                OnTileRevealedEvent?.Invoke(this);
            });
        }
        
        [ContextMenu("Hide")]
        public void Hide()
        {
            if (IsHidden || IsFrozen) return;
            _rotateOverTime.Rotate(new Vector3(0,0, 0), 350, () =>
            {
                IsHidden = true;
                OnTileHiddenEvent?.Invoke(this);
            });
        }

        public override void Move(Vector3 target)
        {
            _moveToPointOverTime.Move(target, 350, () =>
            {
                Debug.Log("Move Done");
            });
        }

        public void Freeze()
        {
            
        }
    }
}