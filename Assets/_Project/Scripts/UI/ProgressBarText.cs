using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class ProgressBarText : ProgressBar
    {
        [SerializeField] private TMP_Text _maxValueText;
        [SerializeField] private TMP_Text _currentValueText;

        private float _maxValue;
        private float _current;

        public void SetMaxValue(float value)
        {
            _maxValueText.text = $"{value:.#}";
            _maxValue = value;
            if (!Mathf.Approximately(_maxValue, 0f))
                SetProgress(_current / _maxValue);
        }

        public void SetCurrentValue(float value)
        {
            _currentValueText.text = $"{value:.#}";
            _current = value;
            if (!Mathf.Approximately(_maxValue, 0f))
                SetProgress(value / _maxValue);
        }
    }
}