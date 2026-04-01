using RoverSimulator.Utilities.Log;

using UnityEngine;

namespace RoverSimulator.Presentation.Rover
{
    public class EngineAudioView : MonoBehaviour
    {
        private const string LogTag = "EngineAudioView";

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private float _maxFadeOutDuration = 2f;

        [SerializeField]
        private float _maxPitch = 2f;

        [SerializeField]
        private float _maxSpeed = 10f;

        [SerializeField]
        private float _minFadeOutDuration = 0.2f;

        [SerializeField]
        private float _minPitch = 0.5f;

        [SerializeField]
        private Rigidbody _rigidbody;

        private float _baseVolume;
        private float _currentFadeOutDuration;
        private bool _isEngineActive;

        private void Awake()
        {
            _baseVolume = _audioSource != null ? _audioSource.volume : 1f;
        }

        public void SetEngineActive(bool active)
        {
            if (active == _isEngineActive)
            {
                return;
            }

            _isEngineActive = active;

            if (!_isEngineActive)
            {
                float speed = _rigidbody.linearVelocity.magnitude;
                float normalizedSpeed = Mathf.Clamp01(speed / _maxSpeed);
                _currentFadeOutDuration = Mathf.Lerp(_minFadeOutDuration, _maxFadeOutDuration, normalizedSpeed);
            }
        }

        private void Update()
        {
            if (_audioSource == null || _rigidbody == null)
            {
                SimulatorLogger.LogError(LogTag, "AudioSource or Rigidbody is null");
                return;
            }

            float speed = _rigidbody.linearVelocity.magnitude;
            float normalizedSpeed = Mathf.Clamp01(speed / _maxSpeed);

            if (_isEngineActive)
            {
                if (!_audioSource.isPlaying)
                {
                    _audioSource.Play();
                }

                _audioSource.volume = _baseVolume;
                _audioSource.pitch = Mathf.Lerp(_minPitch, _maxPitch, normalizedSpeed);
                return;
            }

            if (!_audioSource.isPlaying)
            {
                return;
            }

            _audioSource.pitch = Mathf.Lerp(_minPitch, _maxPitch, normalizedSpeed);

            float fadeStep = _baseVolume * Time.deltaTime / Mathf.Max(0.001f, _currentFadeOutDuration);
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, 0f, fadeStep);

            if (_audioSource.volume <= 0f)
            {
                _audioSource.Stop();
            }
        }
    }
}