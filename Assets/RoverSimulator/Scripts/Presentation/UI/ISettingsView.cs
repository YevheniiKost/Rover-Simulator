using System;

namespace RoverSimulator.Presentation.UI
{
    public interface ISettingsView
    {
        event Action<float> MassChanged;
        event Action<float> MotorTorqueChanged;
        event Action<float> SideInputChanged;
        event Action ApplyButtonClick;
        event Action ResetButtonClick;
        event Action CloseButtonClick;

        void SetMass(float mass);
        void SetMotorTorque(float torque);
        void SetSideInputCoefficient(float coefficient);
        void SetResetButtonInteractable(bool interactable);
        void SetApplyButtonInteractable(bool interactable);
        void Close();
    }
}