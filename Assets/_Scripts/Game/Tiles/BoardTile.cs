using _Scripts.Behaviours;
using _Scripts.ScriptableObjects.Setup;
using UnityEngine;

namespace _Scripts.Game.Tiles
{
    [RequireComponent(typeof(MoveToPointOverTime))]
    [RequireComponent(typeof(RotateOverTime))]
    [RequireComponent(typeof(BoxCollider))]
    public class BoardTile: MonoBehaviour
    {
        [SerializeField] protected BoxCollider Collider;
        public Vector3 Size => Collider.size;
        protected RotateOverTime _rotateOverTime;
        protected MoveToPointOverTime _moveToPointOverTime;
        [SerializeField] protected TileInfo _tileInfo;
        public string PairKey => _tileInfo.PairKey;
        public TileType TileType => _tileInfo.TileType;
        
        protected virtual void Awake()
        {
            _rotateOverTime = GetComponent<RotateOverTime>();
            _moveToPointOverTime = GetComponent<MoveToPointOverTime>();
        }

        public virtual void Initialize(TileInfo tileInfo)
        {
            _tileInfo = tileInfo;
        }
        
        public virtual void Move(Vector3 target)
        {
            
        }
    }
}