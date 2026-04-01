using System;

namespace RoverSimulator.Data.Input
{
    public interface IInputProvider : IDisposable
    {
        event Action ChangeCameraAction;
        event Action PauseAction;
        
        InputState ReadInput();
        
        void SetPlayerInputActive(bool active);
        void SetUIInputActive(bool active);
    }
}