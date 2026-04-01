using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace RoverSimulator.Presentation.UI
{
    public class SliderView : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;
        [SerializeField]
        private TextMeshProUGUI _valueLabel;

        public Action<float> ValueChanged { get; set; }
        public Slider Slider => _slider;

        public void SetMinMaxValue(float min, float max)
        {
            _slider.minValue = min;
            _slider.maxValue = max;
        }

        public void SetValue(float value)
        {
            _slider.value = value;
        }

        private void Awake()
        {
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnDestroy()
        {
            _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            ValueChanged?.Invoke(value);

            if (_valueLabel != null)
            {
                _valueLabel.text = value.ToString("F2");
            }
        }
    }
}