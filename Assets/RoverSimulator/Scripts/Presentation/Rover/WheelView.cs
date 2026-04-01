using RoverSimulator.Utilities.Log;

using UnityEngine;

namespace RoverSimulator.Presentation.Rover
{
    public class WheelView : MonoBehaviour
    {
        private const string LogTag = "WheelView";
        private const float DegreesPerRevolution = 360f;
        private const float SecondsPerMinute = 60f;

        [SerializeField]
        private Transform _visualTransform;

        [SerializeField]
        private WheelCollider _wheelCollider;

        private float _accumulatedAngleDegrees;

        public float MotorTorque
        {
            get => _wheelCollider.motorTorque;
            set => _wheelCollider.motorTorque = value;
        }

        private void FixedUpdate()
        {
            if (_wheelCollider == null || _visualTransform == null)
            {
                SimulatorLogger.LogError(LogTag, "WheelCollider or VisualTransform is null");
                return;
            }

            float degreesPerSecond = _wheelCollider.rpm * DegreesPerRevolution / SecondsPerMinute;
            _accumulatedAngleDegrees += degreesPerSecond * Time.fixedDeltaTime;
            _visualTransform.localRotation = Quaternion.Euler(_accumulatedAngleDegrees, 0f, 0f);
        }
    }
}