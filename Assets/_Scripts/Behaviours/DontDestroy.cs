using System.Linq;
using UnityEngine;

namespace _Scripts.Behaviours
{
    public class DontDestroy : MonoBehaviour
    {
        [SerializeField] private DontDestroyObjectType ObjectType;
    
        void Awake()
        {
            var sameObject = FindObjectsOfType<DontDestroy>().Any(x => x != this && x.ObjectType == ObjectType);

            if (sameObject)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }

    public enum DontDestroyObjectType
    {
        Cheats,
        InputManager,
        GUI,
    }
}