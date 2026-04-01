using UnityEngine;

namespace RoverSimulator.Utilities.Log
{
    public static class SimulatorLogger
    {
        public static void Log(string tag, string message)
        {
            Debug.Log(BuildMessage(tag, message));
        }

        public static void LogWarning(string tag, string message)
        {
            Debug.LogWarning(BuildMessage(tag, message));
        }

        public static void LogError(string tag, string message)
        {
            Debug.LogError(BuildMessage(tag, message));
        }

        private static string BuildMessage(string tag, string message)
        {
            return $"[{tag}]: {message}";
        }
    }
}