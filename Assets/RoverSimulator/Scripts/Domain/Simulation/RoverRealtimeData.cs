namespace RoverSimulator.Domain.Simulation
{
    public readonly struct RoverRealtimeData
    {
        public readonly float Speed;
        
        public RoverRealtimeData(float speed)
        {
            Speed = speed;
        }
    }
}