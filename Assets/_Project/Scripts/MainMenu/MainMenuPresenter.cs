using System;
using Cysharp.Threading.Tasks;
using Fusion;
using Zenject;

namespace _Project.Scripts.MainMenu
{
    public class MainMenuPresenter : IInitializable, IDisposable
    {
        private const string DEFAULT_HOST_NAME = "Game Host";
        private const string DEFAULT_CLIENT_NAME = "Client";

        private readonly MainMenuView _view;
        private readonly MainMenuModel _model;

        public MainMenuPresenter(MainMenuView view, MainMenuModel model)
        {
            _view = view;
            _model = model;
        }

        public void Initialize()
        {
            _view.PlayClicked += OnPlayClicked;
            _view.ExitClicked += OnExitClicked;
            _view.HostClicked += OnHostClicked;
            _view.JoinClicked += OnJoinClicked;
            _view.BackClicked += OnBackClicked;
            _model.ConnectionFailed += OnConnectionFailed;
        }

        private void OnPlayClicked()
        {
            _view.OpenConnection();
            string nickname = _model.GetCurrentNickname();
            if (!string.IsNullOrEmpty(nickname))
                _view.SetNickname(nickname);
        }

        private void OnExitClicked()
            => _model.ExitGame();

        private void OnBackClicked()
        {
            _view.OpenMain();
            _view.SetConnectionStatus("");
        }

        private void OnHostClicked()
        {
            _view.SetConnectionButtonsActive(false);
            _view.SetConnectionStatus("Hosting...");
            string nickname = _view.Nickname.Trim(' ');
            _model
                .StartHost(string.IsNullOrEmpty(nickname) ? DEFAULT_HOST_NAME : nickname)
                .Forget();
        }


        private void OnJoinClicked()
        {
            _view.SetConnectionButtonsActive(false);
            _view.SetConnectionStatus("Connecting...");
            string nickname = _view.Nickname.Trim(' ');
            _model
                .StartClient(string.IsNullOrEmpty(nickname) ? DEFAULT_CLIENT_NAME : nickname, _view.Room)
                .Forget();
        }

        private void OnConnectionFailed(ShutdownReason reason)
        {
            _view.SetConnectionButtonsActive(true);
            _view.SetConnectionStatus($"Connection error: {reason}");
        }

        public void Dispose()
        {
            _view.PlayClicked -= OnPlayClicked;
            _view.ExitClicked -= OnExitClicked;
            _view.HostClicked -= OnHostClicked;
            _view.JoinClicked -= OnJoinClicked;
            _view.BackClicked -= OnBackClicked;
            _model.ConnectionFailed -= OnConnectionFailed;
        }
    }
}