using System;
using UnityEngine;

namespace RoverSimulator.Data.Config
{
    public class RoverConfig 
    {
        public float MotorTorque { get; set; }
        public float SideInputCoefficient { get; set; }
        
        public float Mass { get; set; }
        
        public GameObject Prefab { get; set; }
        
        public RoverConfig(float motorTorque, float sideInputCoefficient, float mass, GameObject prefab)
        {
            MotorTorque = motorTorque;
            SideInputCoefficient = sideInputCoefficient;
            Mass = mass;
            Prefab = prefab;
        }

        public RoverConfig Clone()
        {
            return new RoverConfig(MotorTorque, SideInputCoefficient, Mass, Prefab);
        }
    }
}