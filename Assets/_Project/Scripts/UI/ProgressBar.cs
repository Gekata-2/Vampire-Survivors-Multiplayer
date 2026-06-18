using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image bar;

        public void SetProgress(float value)
        {
            value = Mathf.Clamp01(value);
            bar.fillAmount = value;
        }
    }
}