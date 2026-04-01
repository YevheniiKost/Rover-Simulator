using RoverSimulator.Utilities.DI;

using UnityEngine;

namespace RoverSimulator.Presentation.UI
{
    public abstract class Window : MonoBehaviour
    {
        private bool _isActive;

        protected UIManager UIManager;

        public abstract WindowType WindowType { get; }
        public bool IsActive => _isActive;

        public virtual void OnCancel()
        {
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _isActive = true;

            OnShow();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _isActive = false;

            OnHide();
        }

        public void Init(UIManager uiManager, DiContainer diContainer)
        {
            UIManager = uiManager;
            InitPresenter(diContainer);
        }

        protected abstract void OnShow();

        protected abstract void OnHide();

        protected abstract void InitPresenter(DiContainer diContainer);
    }

    public abstract class Window<T> : Window
    {
        protected T Presenter;

        protected sealed override void InitPresenter(DiContainer diContainer)
        {
            Presenter = diContainer.GetService<T>();
        }
    }
}