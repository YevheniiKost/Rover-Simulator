using System;

namespace RoverSimulator.Presentation.UI
{
    public interface IMainMenuView
    {
        event Action StartButtonPressed;
        event Action ResetButtonPressed;
        event Action ContinueButtonPressed;
        event Action SettingsButtonPressed;
        event Action QuitButtonPressed;

        void OpenHud();
        void SetButtonLayout(bool startActive, bool resetActive, bool continueActive);
        void OpenSettings();
    }
}