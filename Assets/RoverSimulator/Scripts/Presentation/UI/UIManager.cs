using System;
using System.Collections.Generic;

using UnityEngine;

using RoverSimulator.Utilities.DI;
using RoverSimulator.Utilities.Log;

using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace RoverSimulator.Presentation.UI
{
    public class UIManager : MonoBehaviour
    {
        private const string LogTag = "UIManager";

        [SerializeField]
        private InputSystemUIInputModule _inputSystemUIModule;

        [SerializeField]
        private List<Window> _windows;

        private DiContainer _diContainer;

        private void Awake()
        {
            _inputSystemUIModule.cancel.action.performed += OnCancelPerformed;
        }

        private void OnDestroy()
        {
            _inputSystemUIModule.cancel.action.performed -= OnCancelPerformed;
        }

        public void Init(DiContainer diContainer)
        {
            if (diContainer == null)
            {
                SimulatorLogger.LogError(LogTag, "DiContainer is null");
                return;
            }

            _diContainer = diContainer;
        }

        public void OpenWindow(WindowType windowType)
        {
            try
            {
                Window window = _windows.Find(x => x.WindowType == windowType) ??
                                throw new Exception($"Window of type {windowType} not found");

                if (window.IsActive)
                {
                    SimulatorLogger.LogWarning(LogTag, $"Window {windowType} already opened");
                    return;
                }

                window.Init(this, _diContainer);
                window.Show();
            }
            catch (Exception ex)
            {
                SimulatorLogger.LogError(LogTag, $"Failed to open window of type {windowType}: {ex.Message}");
            }
        }

        public void HideWindow(WindowType windowType)
        {
            try
            {
                Window window = _windows.Find(x => x.WindowType == windowType) ??
                                throw new Exception($"Window of type {windowType} not found");

                if (!window.IsActive)
                {
                    SimulatorLogger.LogWarning(LogTag, $"Window {windowType} already closed");
                    return;
                }

                window.Hide();
            }
            catch (Exception ex)
            {
                SimulatorLogger.LogError(LogTag, $"Failed to close window of type {windowType}: {ex.Message}");
            }
        }

        private void OnCancelPerformed(InputAction.CallbackContext context)
        {
            Window activeWindow = _windows.Find(x => x.IsActive);
            if (activeWindow != null)
            {
                activeWindow.OnCancel();
            }
        }
    }
}