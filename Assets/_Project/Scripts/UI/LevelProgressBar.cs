using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class LevelProgressBar : ProgressBar
    {
        [SerializeField] private TMP_Text _levelValueText;

        public void SetLevel(int value) 
            => _levelValueText.text = $"Lv. {value}";
    }
}