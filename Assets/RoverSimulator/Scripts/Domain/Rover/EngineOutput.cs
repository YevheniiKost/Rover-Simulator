namespace RoverSimulator.Domain.Rover
{
    public struct EngineOutput
    {
        public float LeftWheelTorque;
        public float RightWheelTorque;
        
        public bool EngineRunning => LeftWheelTorque != 0f || RightWheelTorque != 0f;
    }
}