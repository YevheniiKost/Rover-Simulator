using System;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RoverSimulator.Presentation.UI
{
    public class SettingsWindow : Window<ISettingsPresenter>, ISettingsView
    {
        [SerializeField]
        private Button _applyButton;
        [SerializeField]
        private Button _closeButton;
        [SerializeField]
        private SliderView _massSlider;
        [SerializeField]
        private SliderView _motorTorqueSlider;
        [SerializeField]
        private Button _resetButton;
        [SerializeField]
        private SliderView _sideInputSlider;

        public override WindowType WindowType => WindowType.Settings;

        public event Action<float> MassChanged;
        public event Action<float> MotorTorqueChanged;
        public event Action<float> SideInputChanged;
        public event Action ApplyButtonClick;
        public event Action ResetButtonClick;
        public event Action CloseButtonClick;

        public void SetMass(float mass)
        {
            _massSlider.SetValue(mass);
        }

        public void SetMotorTorque(float torque)
        {
            _motorTorqueSlider.SetValue(torque);
        }

        public void SetSideInputCoefficient(float coefficient)
        {
            _sideInputSlider.SetValue(coefficient);
        }

        public void SetResetButtonInteractable(bool interactable)
        {
            _resetButton.interactable = interactable;
        }

        public void SetApplyButtonInteractable(bool interactable)
        {
            _applyButton.interactable = interactable;
        }

        public void Close()
        {
            UIManager.HideWindow(WindowType.Settings);
            UIManager.OpenWindow(WindowType.MainMenu);
        }

        public override void OnCancel()
        {
            CloseButtonClick?.Invoke();
        }

        protected override void OnShow()
        {
            _massSlider.ValueChanged = value => { MassChanged?.Invoke(value); };
            _motorTorqueSlider.ValueChanged = value => { MotorTorqueChanged?.Invoke(value); };
            _sideInputSlider.ValueChanged = value => { SideInputChanged?.Invoke(value); };
            _applyButton.onClick.AddListener(() => { ApplyButtonClick?.Invoke(); });
            _resetButton.onClick.AddListener(() => { ResetButtonClick?.Invoke(); });
            _closeButton.onClick.AddListener(() => { CloseButtonClick?.Invoke(); });

            Presenter.AttachView(this);

            EventSystem.current.SetSelectedGameObject(_massSlider.Slider.gameObject);
        }

        protected override void OnHide()
        {
            _massSlider.ValueChanged = null;
            _motorTorqueSlider.ValueChanged = null;
            _sideInputSlider.ValueChanged = null;
            _applyButton.onClick.RemoveAllListeners();
            _resetButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();

            Presenter.DetachView();
        }
    }
}