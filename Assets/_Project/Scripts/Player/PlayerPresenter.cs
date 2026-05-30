using _Project.Scripts.UI;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Player
{
    public class PlayerPresenter : NetworkBehaviour
    {
        [SerializeField] private PlayerStateView _stateView;
        [SerializeField] private PlayerModel _model;

        private IPlayerView _view;
        private GameHUD _hud;

        [Inject]
        private void Construct(GameHUD hud)
        {
            _hud = hud;
        }
        
        public override void Spawned()
        {
            _model.ExperienceChanged += ModelOnExperienceChanged;
            _model.HealthChanged += ModelOnHealthChanged;
            _model.ArrowFired += ModelOnArrowFired;
            _model.NicknameChanged += OnNicknameChanged;

            if (HasInputAuthority)
            {
                _view = _hud;
                _stateView.EnableLocalMode();
            }
            else
            {
                _view = _stateView;
            }

            _stateView.SetNickname(_model.Nickname.Value);
            _stateView.SetReloadProgress(_model.Combat.GetReloadProgress());
            _stateView.SetShotsFired(_model.Combat.ArrowsFired);

            InitializeView();
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _model.ExperienceChanged -= ModelOnExperienceChanged;
            _model.HealthChanged -= ModelOnHealthChanged;
            _model.ArrowFired -= ModelOnArrowFired;
            _model.NicknameChanged -= OnNicknameChanged;
        }

        public override void Render()
        {
            _stateView.SetReloadProgress(_model.Combat.GetReloadProgress());
        }

        private void InitializeView()
        {
            _view.SetMaxHealth(_model.Health.MaxHealth);
            _view.SetHealth(_model.Health.Current);
            _view.SetExperienceNormalized(_model.Experience.LevelProgressNormalized);
            _view.SetLevel(_model.Experience.Level);
        }

        private void OnNicknameChanged()
            => _stateView.SetNickname(_model.Nickname.Value);

        private void ModelOnArrowFired()
            => _stateView.SetShotsFired(_model.Combat.ArrowsFired);

        private void ModelOnHealthChanged()
        {
            _view.SetMaxHealth(_model.Health.MaxHealth);
            _view.SetHealth(_model.Health.Current);
        }

        private void ModelOnExperienceChanged()
        {
            _view.SetLevel(_model.Experience.Level);
            _view.SetExperienceNormalized(_model.Experience.LevelProgressNormalized);
        }
    }
}