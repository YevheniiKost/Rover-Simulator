using System;

using UnityEngine;

using RoverSimulator.Data.Input;
using RoverSimulator.Domain.Rover;
using RoverSimulator.Domain.Simulation;
using RoverSimulator.Utilities.Log;

namespace RoverSimulator.Presentation.Rover
{
    public class RoverView : MonoBehaviour
    {
        private const string LogTag = "RoverView";
        
        [SerializeField]
        private Rigidbody _rigidbody;
       
        [Header("Audio")]
        [SerializeField]
        private EngineAudioView _engineAudioView;

        [Header("Camera")]
        [SerializeField]
        private Transform _firstPersonCameraPosition;

        [Header("Wheels")]
        [SerializeField]
        private WheelView _frontLeftWheel;
        [SerializeField]
        private WheelView _frontRightWheel;
        [SerializeField]
        private WheelView _backLeftWheel;
        [SerializeField]
        private WheelView _backRightWheel;


        private IInputProvider _inputProvider;
        private IEngineModel _engineModel;

        public Transform FirstPersonCameraPosition => _firstPersonCameraPosition;
        public Action<RoverRealtimeData> OnRoverDataUpdated { get; set; }

        public void Init(IInputProvider provider, IEngineModel engineModel)
        {
            if (provider == null)
            {
                SimulatorLogger.LogError(LogTag, "Input provider is null");
                return;
            }

            if (engineModel == null)
            {
                SimulatorLogger.LogError(LogTag, "Engine model is null");
                return;
            }

            _engineModel = engineModel;
            _inputProvider = provider;
        }

        public void ApplyConfig(float mass)
        {
            if (mass <= 0)
            {
                SimulatorLogger.LogError(LogTag, "Mass must be greater than zero");
                return;
            }

            _rigidbody.mass = mass;
        }

        private void FixedUpdate()
        {
            if (_inputProvider == null || _engineModel == null)
            {
                return;
            }

            ProcessEngineOutput();
        }

        private void Update()
        {
            if (OnRoverDataUpdated == null)
            {
                return;
            }

            RoverRealtimeData data = new RoverRealtimeData(_rigidbody.linearVelocity.magnitude);

            OnRoverDataUpdated.Invoke(data);
        }

        private void ProcessEngineOutput()
        {
            InputState inputState = _inputProvider.ReadInput();
            EngineOutput engineOutput = _engineModel.CalculateEngineOutput(inputState);

            _frontLeftWheel.MotorTorque = engineOutput.LeftWheelTorque;
            _backLeftWheel.MotorTorque = engineOutput.LeftWheelTorque;
            _frontRightWheel.MotorTorque = engineOutput.RightWheelTorque;
            _backRightWheel.MotorTorque = engineOutput.RightWheelTorque;

            _engineAudioView.SetEngineActive(engineOutput.EngineRunning);
        }
    }
}