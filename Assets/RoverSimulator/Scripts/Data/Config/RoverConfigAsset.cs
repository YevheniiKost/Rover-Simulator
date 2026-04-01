using UnityEngine;

namespace RoverSimulator.Data.Config
{
    [CreateAssetMenu(fileName = "RoverConfigAsset", menuName = "RoverSimulator/Config/RoverConfigAsset")]
    public class RoverConfigAsset : ScriptableObject
    {
        [Header("Motor")]
        [SerializeField]
        private float _motorTorque;
        [SerializeField]
        private float _sideMovementCoefficient;

        [Header("Physics")]
        [SerializeField]
        private float _mass;
        
        [Header("Prefab")]
        [SerializeField]
        private GameObject _roverPrefab;
        
        public RoverConfig GetRoverConfig()
        {
            return new RoverConfig(_motorTorque, _sideMovementCoefficient, _mass, _roverPrefab);
        }
    }
}