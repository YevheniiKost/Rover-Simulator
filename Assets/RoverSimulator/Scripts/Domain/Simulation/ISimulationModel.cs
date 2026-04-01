using System;

namespace RoverSimulator.Domain.Simulation
{
    public interface ISimulationModel
    {
        event Action Started;
        event Action Paused;
        event Action Resumed;
        
        RoverRealtimeData RoverRealtimeData { get; }
        bool IsStarted { get; }
        
        void Start();
        void Pause();
        void Resume();
        
        void UpdateRoverRealtimeData(RoverRealtimeData roverRealtimeData);
    }
}