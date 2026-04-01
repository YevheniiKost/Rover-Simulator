using System;

namespace RoverSimulator.Domain.Simulation
{
    public class SimulationModel : ISimulationModel
    {
        public event Action Started;
        public event Action Paused;
        public event Action Resumed;

        public RoverRealtimeData RoverRealtimeData { get; private set; }
        public bool IsStarted { get; private set; }

        public void Start()
        {
            IsStarted = true;
            Started?.Invoke();
        }

        public void Pause()
        {
            Paused?.Invoke();
        }

        public void Resume()
        {
            Resumed?.Invoke();
        }
        
        public void UpdateRoverRealtimeData(RoverRealtimeData roverRealtimeData)
        {
            RoverRealtimeData = roverRealtimeData;
        }
    }
}