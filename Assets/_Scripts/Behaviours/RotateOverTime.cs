using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts
{
    public class RotateOverTime: MonoBehaviour
    {
        private Quaternion _currentRotation;
        private Quaternion _targetEulerRotation;
        private bool _isRotating;
        private float _timeToRotate;
        private float _startedRotationAt;
        private UnityAction _onRotateComplete;
        
        public void Rotate(Vector3 rotationDelta, float timeToRotateMS, UnityAction onRotateComplete)
        {
            _targetEulerRotation = Quaternion.Euler(rotationDelta);
            _currentRotation = transform.rotation;
            _timeToRotate = timeToRotateMS / 1000;
            _startedRotationAt = Time.time;
            _isRotating = true;
            _onRotateComplete = onRotateComplete;
        }

        private void Update()
        {
            if (!_isRotating) return;

            var timeFactor = (Time.time - _startedRotationAt) / _timeToRotate;

            var nextRotation = Quaternion.Slerp(_currentRotation, _targetEulerRotation, timeFactor);

            transform.rotation = nextRotation;

            if (timeFactor < 1) return;
            
            _isRotating = false;
            _onRotateComplete?.Invoke();
        }
    }
}