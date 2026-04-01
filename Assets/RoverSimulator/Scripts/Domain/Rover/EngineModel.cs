using UnityEngine;
using RoverSimulator.Data.Input;
using RoverSimulator.Data.Config;

namespace RoverSimulator.Domain.Rover
{
    public class EngineModel : IEngineModel
    {
        private readonly IConfigProvider _configProvider;
        
        private RoverConfig _roverConfig;
        
        public EngineModel(IConfigProvider configProvider)
        {
            _configProvider = configProvider;
        }

        public void Init()
        {
            _roverConfig = _configProvider.GetRoverConfig();
        }

        public EngineOutput CalculateEngineOutput(InputState input)
        {
            input.Forward = Mathf.Clamp(input.Forward, -1f, 1f);
            input.Right = Mathf.Clamp(input.Right, -1f, 1f);

            float leftTorque = (input.Forward + (input.Right * _roverConfig.SideInputCoefficient)) *
                               _roverConfig.MotorTorque;
            float rightTorque = (input.Forward - (input.Right * _roverConfig.SideInputCoefficient)) *
                                _roverConfig.MotorTorque;

            return new EngineOutput
            {
                LeftWheelTorque = leftTorque,
                RightWheelTorque = rightTorque
            };
        }
    }
}