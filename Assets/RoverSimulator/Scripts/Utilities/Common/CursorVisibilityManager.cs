using UnityEngine;
using UnityEngine.InputSystem;

namespace RoverSimulator.Utilities.Common
{
    public class CursorVisibilityManager : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void OnEnable()
        {
            InputSystem.onActionChange += OnInputActionChange;
        }

        private void OnDisable()
        {
            InputSystem.onActionChange -= OnInputActionChange;
        }

        private void OnInputActionChange(object obj, InputActionChange change)
        {
            if (change == InputActionChange.ActionPerformed)
            {
                InputAction inputAction = obj as InputAction;
                if (inputAction == null)
                {
                    return;
                }
                
                InputControl lastControl = inputAction.activeControl;
                InputDevice lastDevice = lastControl.device;

                if (lastDevice is Keyboard || lastDevice is Gamepad)
                {
                    Cursor.visible = false;
                }
                else if (lastDevice is Mouse)
                {
                    Cursor.visible = true;
                }
            }
        }
    }
}