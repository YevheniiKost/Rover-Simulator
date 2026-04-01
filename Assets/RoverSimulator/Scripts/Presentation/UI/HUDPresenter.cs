using System;

using RoverSimulator.Domain.Simulation;
using RoverSimulator.Utilities.Log;

namespace RoverSimulator.Presentation.UI
{
    public class HUDPresenter : IHUDPresenter
    {
        private const string LogTag = "HUDPresenter";

        private readonly ISimulationModel _simulationModel;
        private IHUDView _view;

        public HUDPresenter(ISimulationModel simulationModel)
        {
            _simulationModel = simulationModel;
        }

        public void AttachView(IHUDView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));

            _simulationModel.Paused += OnPaused;
        }

        public void DetachView()
        {
            _view = null;

            _simulationModel.Paused -= OnPaused;
        }

        public void Update()
        {
            if (_view == null)
            {
                SimulatorLogger.LogWarning(LogTag, "View is not attached");
                return;
            }

            _view.SetSpeed(_simulationModel.RoverRealtimeData.Speed);
        }

        private void OnPaused()
        {
            _view.OpenMainMenu();
        }
    }
}