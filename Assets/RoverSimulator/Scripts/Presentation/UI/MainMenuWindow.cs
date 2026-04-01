using System;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RoverSimulator.Presentation.UI
{
    public class MainMenuWindow : Window<IMainMenuPresenter>, IMainMenuView
    {
        [SerializeField]
        private Button _continueButton;
        [SerializeField]
        private Button _quitButton;
        [SerializeField]
        private Button _resetButton;
        [SerializeField]
        private Button _settingsButton;
        [SerializeField]
        private Button _startButton;

        public override WindowType WindowType => WindowType.MainMenu;

        public event Action StartButtonPressed;
        public event Action ResetButtonPressed;
        public event Action ContinueButtonPressed;
        public event Action SettingsButtonPressed;
        public event Action QuitButtonPressed;

        public void OpenHud()
        {
            UIManager.OpenWindow(WindowType.HUD);
            UIManager.HideWindow(WindowType.MainMenu);
        }

        public void SetButtonLayout(bool startActive, bool resetActive, bool continueActive)
        {
            _startButton.gameObject.SetActive(startActive);
            _resetButton.gameObject.SetActive(resetActive);
            _continueButton.gameObject.SetActive(continueActive);

            GameObject activeButton = null;

            if (startActive)
            {
                activeButton = _startButton.gameObject;
            }
            else if (continueActive)
            {
                activeButton = _continueButton.gameObject;
            }
            else if (resetActive)
            {
                activeButton = _resetButton.gameObject;
            }

            EventSystem.current.firstSelectedGameObject = activeButton;
            EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
        }

        public void OpenSettings()
        {
            UIManager.OpenWindow(WindowType.Settings);
            UIManager.HideWindow(WindowType.MainMenu);
        }

        protected override void OnShow()
        {
            _startButton.onClick.AddListener(() => StartButtonPressed?.Invoke());
            _resetButton.onClick.AddListener(() => ResetButtonPressed?.Invoke());
            _continueButton.onClick.AddListener(() => ContinueButtonPressed?.Invoke());
            _settingsButton.onClick.AddListener(() => SettingsButtonPressed?.Invoke());
            _quitButton.onClick.AddListener(() => QuitButtonPressed?.Invoke());

            Presenter.AttachView(this);
        }

        protected override void OnHide()
        {
            _startButton.onClick.RemoveAllListeners();
            _resetButton.onClick.RemoveAllListeners();
            _continueButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.RemoveAllListeners();
            _quitButton.onClick.RemoveAllListeners();

            Presenter.DetachView();
        }
    }
}