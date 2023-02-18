using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Behaviours
{
    public class MoveToPointOverTime : MonoBehaviour
    {
        private Vector3 _currentPosition;
        private Vector3 _targetPosition;
        private float _timeToMove;
        private float _startedMovingAt;
        private UnityAction _onMovementComplete;

        public void Move(Vector3 target, float timeToRotateMS, UnityAction onMovementComplete)
        {
            _targetPosition = target;
            _currentPosition = transform.position;
            _timeToMove = timeToRotateMS / 1000;
            _startedMovingAt = Time.time;
            _onMovementComplete = onMovementComplete;

            StartCoroutine(nameof(MovementUpdate));
        }

        private IEnumerator MovementUpdate()
        {
            while (true)
            {
                var timeFactor = (Time.time - _startedMovingAt) / _timeToMove;

                var nextPosition = Vector3.Slerp(_currentPosition, _targetPosition, timeFactor);

                transform.position = nextPosition;

                if (timeFactor >= 1)
                {
                    break;
                }
                
                yield return new WaitForEndOfFrame();
            }
            
            OnMovementComplete();
        }

        private void OnMovementComplete()
        {
            StopCoroutine(nameof(MovementUpdate));
            
            _onMovementComplete?.Invoke();
        }
    }
}