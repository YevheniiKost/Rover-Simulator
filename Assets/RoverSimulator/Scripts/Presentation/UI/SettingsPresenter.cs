using System;

using RoverSimulator.Data.Config;
using RoverSimulator.Utilities.Log;

namespace RoverSimulator.Presentation.UI
{
    public class SettingsPresenter : ISettingsPresenter
    {
        private const string LogTag = "SettingsPresenter";

        private readonly IConfigProvider _configProvider;
        private bool _isDirty;

        private RoverConfig _roverConfig;
        private ISettingsView _view;

        public SettingsPresenter(IConfigProvider configProvider)
        {
            _configProvider = configProvider;
        }

        public void AttachView(ISettingsView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));

            _view.MassChanged += OnMassChanged;
            _view.MotorTorqueChanged += OnMotorTorqueChanged;
            _view.SideInputChanged += OnSideInputChanged;
            _view.ApplyButtonClick += OnApplyButtonClick;
            _view.ResetButtonClick += OnResetButtonClick;
            _view.CloseButtonClick += OnCloseButtonClick;

            SetCurrentValues();
        }

        public void DetachView()
        {
            _view.MassChanged -= OnMassChanged;
            _view.MotorTorqueChanged -= OnMotorTorqueChanged;
            _view.SideInputChanged -= OnSideInputChanged;
            _view.ApplyButtonClick -= OnApplyButtonClick;
            _view.ResetButtonClick -= OnResetButtonClick;
            _view.CloseButtonClick -= OnCloseButtonClick;

            _view = null;
        }

        private void OnCloseButtonClick()
        {
            _view.Close();
        }

        private void OnMassChanged(float value)
        {
            value = Math.Clamp(value, 0.1f, float.MaxValue);
            _roverConfig.Mass = value;
            SetDirty(true);
        }

        private void OnMotorTorqueChanged(float value)
        {
            value = Math.Clamp(value, 0, float.MaxValue);
            _roverConfig.MotorTorque = value;
            SetDirty(true);
        }

        private void OnSideInputChanged(float value)
        {
            value = Math.Clamp(value, 0, 1);
            _roverConfig.SideInputCoefficient = value;
            SetDirty(true);
        }

        private void OnApplyButtonClick()
        {
            if (!_isDirty)
            {
                return;
            }
            
            try
            {
                _configProvider.OverrideRoverConfig(_roverConfig);
                SetDirty(false);
            }
            catch (Exception ex)
            {
                SimulatorLogger.LogError(LogTag, $"Failed to apply config: {ex.Message}");
            }
        }

        private void OnResetButtonClick()
        {
            SetCurrentValues();
        }

        private void SetCurrentValues()
        {
            try
            {
                RoverConfig config = _configProvider.GetRoverConfig();
                _roverConfig = config;

                _view.SetMass(config.Mass);
                _view.SetMotorTorque(config.MotorTorque);
                _view.SetSideInputCoefficient(config.SideInputCoefficient);
                SetDirty(false);
            }
            catch (Exception ex)
            {
                SimulatorLogger.LogError(LogTag, $"Failed to load config: {ex.Message}");
            }
        }

        private void SetDirty(bool isDirty)
        {
            _isDirty = isDirty;
            _view.SetResetButtonInteractable(isDirty);
            _view.SetApplyButtonInteractable(isDirty);
        }
    }
}