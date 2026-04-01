using System;

using RoverSimulator.Data.Config;
using RoverSimulator.Domain.Simulation;

namespace RoverSimulator.Presentation.UI
{
    public class MainMenuPresenter : IMainMenuPresenter
    {
        private readonly IConfigProvider _configProvider;
        private readonly ISimulationModel _model;

        private IMainMenuView _view;

        public MainMenuPresenter(ISimulationModel model, IConfigProvider configProvider)
        {
            _model = model;
            _configProvider = configProvider;
        }

        public void AttachView(IMainMenuView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));

            _view.StartButtonPressed += OnStartButtonPressed;
            _view.ResetButtonPressed += OnStartButtonPressed;
            _view.ContinueButtonPressed += OnContinueButtonPressed;
            _view.SettingsButtonPressed += OnSettingsButtonPressed;
            _view.QuitButtonPressed += OnQuitButtonPressed;

            InitializeLayout();
        }

        public void DetachView()
        {
            if (_view != null)
            {
                _view.StartButtonPressed -= OnStartButtonPressed;
                _view.ResetButtonPressed -= OnStartButtonPressed;
                _view.ContinueButtonPressed -= OnContinueButtonPressed;
                _view.SettingsButtonPressed -= OnSettingsButtonPressed;
                _view.QuitButtonPressed -= OnQuitButtonPressed;

                _view = null;
            }
        }

        private void InitializeLayout()
        {
            bool isStarted = _model.IsStarted;
            bool isConfigOverriden = _configProvider.IsConfigOverridden;

            _view.SetButtonLayout(!isStarted, isStarted, isStarted && !isConfigOverriden);
        }

        private void OnStartButtonPressed()
        {
            _configProvider.IsConfigOverridden = false;
            _model.Start();
            _view.OpenHud();
        }

        private void OnContinueButtonPressed()
        {
            _model.Resume();
            _view.OpenHud();
        }

        private void OnSettingsButtonPressed()
        {
            _view.OpenSettings();
        }

        private void OnQuitButtonPressed()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}