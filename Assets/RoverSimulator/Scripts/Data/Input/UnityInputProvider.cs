using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RoverSimulator.Data.Input
{
    public class UnityInputProvider : IInputProvider
    {
        private readonly UnityInputSystemActions _unityInputSystemActions;

        public UnityInputProvider()
        {
            _unityInputSystemActions = new UnityInputSystemActions();
            _unityInputSystemActions.Enable();

            _unityInputSystemActions.Player.ChangeCamera.performed += OnChangeCameraAction;
            _unityInputSystemActions.Player.Pause.performed += OnPauseAction;
        }

        public event Action ChangeCameraAction;
        public event Action PauseAction;

        public InputState ReadInput()
        {
            return new InputState
            {
                Forward = _unityInputSystemActions.Player.Move.ReadValue<Vector2>().y,
                Right = _unityInputSystemActions.Player.Move.ReadValue<Vector2>().x,
            };
        }

        public void SetPlayerInputActive(bool active)
        {
            if (active)
            {
                _unityInputSystemActions.Player.Enable();
            }
            else
            {
                _unityInputSystemActions.Player.Disable();
            }
        }

        public void SetUIInputActive(bool active)
        {
            if (active)
            {
                _unityInputSystemActions.UI.Enable();
            }
            else
            {
                _unityInputSystemActions.UI.Disable();
            }
        }

        public void Dispose()
        {
            _unityInputSystemActions.Player.ChangeCamera.performed -= OnChangeCameraAction;
            _unityInputSystemActions.Player.Pause.performed -= OnPauseAction;

            _unityInputSystemActions.Disable();
            _unityInputSystemActions.Dispose();
        }

        private void OnChangeCameraAction(InputAction.CallbackContext obj)
        {
            ChangeCameraAction?.Invoke();
        }

        private void OnPauseAction(InputAction.CallbackContext obj)
        {
            PauseAction?.Invoke();
        }
    }
}