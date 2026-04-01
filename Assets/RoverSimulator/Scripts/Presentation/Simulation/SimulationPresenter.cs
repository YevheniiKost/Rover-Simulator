using System;

using RoverSimulator.Data.Config;
using RoverSimulator.Data.Input;
using RoverSimulator.Domain.Rover;
using RoverSimulator.Domain.Simulation;
using RoverSimulator.Presentation.Rover;
using RoverSimulator.Utilities.DI;
using RoverSimulator.Utilities.Log;

using UnityEngine;

namespace RoverSimulator.Presentation.Simulation
{
    public class SimulationPresenter : MonoBehaviour
    {
        private const string LogTag = "SimulationPresenter";

        [SerializeField]
        private CameraController _cameraController;

        [SerializeField]
        private Transform _spawnPoint;

        private DiContainer _container;

        private ViewType _currentViewType = ViewType.ThirdPerson;
        private IInputProvider _inputProvider;

        private RoverView _rover;
        private ISimulationModel _simulationModel;


        public void Init(DiContainer container)
        {
            if (container == null)
            {
                SimulatorLogger.LogError(LogTag, "DiContainer is null");
                return;
            }

            _container = container;

            _inputProvider = _container.GetService<IInputProvider>();
            _inputProvider.ChangeCameraAction += OnChangeCameraAction;
            _inputProvider.PauseAction += OnPauseAction;
            SwitchInput(false);

            _simulationModel = _container.GetService<ISimulationModel>();

            _simulationModel.Started += OnStarted;
            _simulationModel.Paused += OnPaused;
            _simulationModel.Resumed += OnResumed;
        }

        private void SpawnRover()
        {
            try
            {
                if (_rover != null)
                {
                    Destroy(_rover.gameObject);
                }

                RoverConfig config = _container.GetService<IConfigProvider>().GetRoverConfig() ??
                                     throw new Exception("Rover config is null");

                _rover = Instantiate(config.Prefab, _spawnPoint.position, _spawnPoint.rotation)
                    .GetComponent<RoverView>() ?? throw new Exception("Rover view is null");

                _rover.ApplyConfig(config.Mass);
                IEngineModel engineModel = _container.GetService<IEngineModel>();
                engineModel.Init();
                _rover.Init(_inputProvider, engineModel);
                _rover.OnRoverDataUpdated = OnRoverDataUpdated;

                _cameraController.ResetTarget();
                _cameraController.SetTarget(_rover);
                _cameraController.SwitchView(_currentViewType);
            }
            catch (Exception ex)
            {
                SimulatorLogger.LogError(LogTag, ex.Message);
                _rover = null;
            }
        }

        private void OnRoverDataUpdated(RoverRealtimeData data)
        {
            _simulationModel.UpdateRoverRealtimeData(data);
        }

        private void OnDestroy()
        {
            if (_inputProvider != null)
            {
                _inputProvider.ChangeCameraAction -= OnChangeCameraAction;

                _inputProvider.Dispose();
                _inputProvider = null;
            }

            if (_simulationModel != null)
            {
                _simulationModel.Started -= OnStarted;
                _simulationModel.Paused -= OnPaused;
                _simulationModel.Resumed -= OnResumed;

                _simulationModel = null;
            }
        }

        private void OnChangeCameraAction()
        {
            _currentViewType = _currentViewType == ViewType.FirstPerson ? ViewType.ThirdPerson : ViewType.FirstPerson;
            _cameraController.SwitchView(_currentViewType);
        }

        private void OnPauseAction()
        {
            _simulationModel.Pause();
        }

        private void OnStarted()
        {
            SpawnRover();
            SwitchInput(true);
        }

        private void OnPaused()
        {
            SwitchInput(false);
        }

        private void OnResumed()
        {
            SwitchInput(true);
        }

        private void SwitchInput(bool playerActive)
        {
            _inputProvider.SetPlayerInputActive(playerActive);
            _inputProvider.SetUIInputActive(!playerActive);
        }
    }
}