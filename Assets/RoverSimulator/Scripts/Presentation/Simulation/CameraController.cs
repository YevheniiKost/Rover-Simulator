using RoverSimulator.Presentation.Rover;
using RoverSimulator.Utilities.Log;

using Unity.Cinemachine;

using UnityEngine;

namespace RoverSimulator.Presentation.Simulation
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private CinemachineCamera _firstPersonCamera;

        [SerializeField]
        private CinemachineCamera _overviewCamera;

        [SerializeField]
        private CinemachineCamera _thirdPersonCamera;

        public void SetTarget(RoverView rover)
        {
            if (rover == null)
            {
                SimulatorLogger.LogError("CameraController", "Rover view is null");
                return;
            }

            _thirdPersonCamera.Follow = rover.transform;
            _thirdPersonCamera.LookAt = rover.transform;

            _firstPersonCamera.Follow = rover.FirstPersonCameraPosition;
            _firstPersonCamera.LookAt = rover.FirstPersonCameraPosition;
        }

        public void SwitchView(ViewType viewType)
        {
            _overviewCamera.Priority = 0;
            _firstPersonCamera.Priority = viewType == ViewType.FirstPerson ? 1 : 0;
            _thirdPersonCamera.Priority = viewType == ViewType.ThirdPerson ? 1 : 0;
        }

        public void ResetTarget()
        {
            _firstPersonCamera.Priority = 0;
            _thirdPersonCamera.Priority = 0;
            _overviewCamera.Priority = 1;
        }
    }
}