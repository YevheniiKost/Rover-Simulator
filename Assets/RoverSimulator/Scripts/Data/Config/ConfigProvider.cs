using System;

using UnityEngine;

namespace RoverSimulator.Data.Config
{
    public class ConfigProvider : IConfigProvider
    {
        private const string DefaultRoverConfigPath = "Configs/RoverConfigAsset";

        private RoverConfig _currentRoverConfig;

        public bool IsConfigOverridden { get; set; }

        public RoverConfig GetRoverConfig()
        {
            if (_currentRoverConfig != null)
            {
                return _currentRoverConfig.Clone();
            }
            
            RoverConfigAsset configAsset = Resources.Load<RoverConfigAsset>(DefaultRoverConfigPath) ??
                                           throw new Exception("Rover config is null");

            _currentRoverConfig = configAsset.GetRoverConfig();

            return _currentRoverConfig;
        }

        public void OverrideRoverConfig(RoverConfig config)
        {
            _currentRoverConfig =
                config ?? throw new ArgumentNullException(nameof(config), "RoverConfig cannot be null");
            IsConfigOverridden = true;
        }
    }
}