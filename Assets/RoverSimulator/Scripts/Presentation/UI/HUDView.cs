using TMPro;

using UnityEngine;

namespace RoverSimulator.Presentation.UI
{
    public class HUDView : Window<IHUDPresenter>, IHUDView
    {
        [SerializeField]
        private TextMeshProUGUI _speedText;

        public override WindowType WindowType => WindowType.HUD;

        public void SetSpeed(float speed)
        {
            _speedText.text = $"Speed: {speed:F1} m/s";
        }

        public void OpenMainMenu()
        {
            UIManager.OpenWindow(WindowType.MainMenu);
            UIManager.HideWindow(WindowType.HUD);
        }

        protected override void OnShow()
        {
            Presenter.AttachView(this);
        }

        protected override void OnHide()
        {
            Presenter.DetachView();
        }

        private void Update()
        {
            Presenter?.Update();
        }
    }
}