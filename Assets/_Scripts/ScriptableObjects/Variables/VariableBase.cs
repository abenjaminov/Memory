using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.ScriptableObjects.Variables
{
    public class VariableBase<T> : ScriptableObject
    {
        public UnityAction<T> OnChangedEvent;

        [SerializeField] private T CurrentValue;
        public T Value {
            get => CurrentValue;
            set
            {
                CurrentValue = value;
                OnChangedEvent?.Invoke(CurrentValue);
            }
        }
    }
}