using RoverSimulator.Data.Input;

namespace RoverSimulator.Domain.Rover
{
    public interface IEngineModel
    {
        void Init();
        EngineOutput CalculateEngineOutput(InputState input);
    }
}