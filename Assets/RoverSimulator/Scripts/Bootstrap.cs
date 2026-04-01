using System;

using UnityEngine;

using RoverSimulator.Data.Config;
using RoverSimulator.Data.Input;
using RoverSimulator.Domain.Rover;
using RoverSimulator.Domain.Simulation;
using RoverSimulator.Presentation.Simulation;
using RoverSimulator.Presentation.UI;
using RoverSimulator.Utilities.DI;

namespace RoverSimulator
{
    [DefaultExecutionOrder(-1)]
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField]
        private SimulationPresenter _simulationPresenter;

        [SerializeField]
        private UIManager _uiManager;

        private DiContainer _container;

        private void Awake()
        {
            try
            {
                _container = GenerateContainer();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void Start()
        {
            try
            {
                _simulationPresenter.Init(_container);
                _uiManager.Init(_container);
                _uiManager.OpenWindow(WindowType.MainMenu);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private DiContainer GenerateContainer()
        {
            DiServiceCollection services = new DiServiceCollection();

            services.RegisterSingleton<IConfigProvider, ConfigProvider>();
            services.RegisterSingleton<IInputProvider, UnityInputProvider>();

            services.RegisterTransient<IEngineModel, EngineModel>();
            services.RegisterSingleton<ISimulationModel, SimulationModel>();

            services.RegisterTransient<IMainMenuPresenter, MainMenuPresenter>();
            services.RegisterTransient<IHUDPresenter, HUDPresenter>();
            services.RegisterTransient<ISettingsPresenter, SettingsPresenter>();

            return services.GenerateContainer();
        }
    }
}