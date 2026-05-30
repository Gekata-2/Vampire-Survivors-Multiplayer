using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.QuickMenu
{
    public class QuickMenu : MonoBehaviour
    {
        public event Action QuitClicked;

        [SerializeField] private Button _quitButton;

        private void Start()
        {
            _quitButton.onClick.AddListener(OnQuitClicked);
        }

        private void OnDestroy()
        {
            _quitButton.onClick.RemoveListener(OnQuitClicked);
        }

        private void OnQuitClicked()
            => QuitClicked?.Invoke();
    }
}