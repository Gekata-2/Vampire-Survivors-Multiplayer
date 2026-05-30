using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        public event Action PlayClicked;
        public event Action ExitClicked;
        public event Action HostClicked;
        public event Action JoinClicked;
        public event Action BackClicked;


        [Header("Main")] [SerializeField] private GameObject _mainMenu;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _exitButton;


        [Space] [Header("Connection")] [SerializeField]
        private GameObject _connectionMenu;

        [SerializeField] private TMP_InputField _nicknameInput;
        [SerializeField] private TMP_InputField _roomInput;

        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _joinButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private TMP_Text _status;

        public string Nickname => _nicknameInput.text;
        public string Room => _roomInput.text.ToUpper();

        private void Start()
        {
            _playButton.onClick.AddListener(OnPlayClicked);
            _exitButton.onClick.AddListener(OnExitClicked);
            _hostButton.onClick.AddListener(OnHostClicked);
            _joinButton.onClick.AddListener(OnJoinClicked);
            _backButton.onClick.AddListener(OnBackClicked);
            _roomInput.onValueChanged.AddListener(OnRoomNameChanged);
            _joinButton.interactable = false;
            _status.text = "";
        }


        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(OnPlayClicked);
            _exitButton.onClick.RemoveListener(OnExitClicked);
            _hostButton.onClick.RemoveListener(OnHostClicked);
            _joinButton.onClick.RemoveListener(OnJoinClicked);
            _backButton.onClick.RemoveListener(OnBackClicked);
            _roomInput.onValueChanged.RemoveListener(OnRoomNameChanged);
        }

        private void OnPlayClicked()
            => PlayClicked?.Invoke();

        private void OnExitClicked()
            => ExitClicked?.Invoke();

        private void OnHostClicked()
            => HostClicked?.Invoke();

        private void OnJoinClicked()
            => JoinClicked?.Invoke();

        private void OnBackClicked()
            => BackClicked?.Invoke();

        private void OnRoomNameChanged(string newName)
            => _joinButton.interactable = !string.IsNullOrEmpty(newName);

        public void OpenMain()
        {
            _mainMenu.SetActive(true);
            _connectionMenu.SetActive(false);
        }

        public void OpenConnection()
        {
            _mainMenu.SetActive(false);
            _connectionMenu.SetActive(true);
        }

        public void SetConnectionStatus(string status)
            => _status.text = status;

        public void SetNickname(string currentNickname)
            => _nicknameInput.text = currentNickname;

        public void SetConnectionButtonsActive(bool isActive)
        {
            _hostButton.interactable = isActive;
            _joinButton.interactable = isActive;
            _backButton.interactable = isActive;
        }
    }
}