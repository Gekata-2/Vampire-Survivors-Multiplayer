using System;
using _Project.Scripts.Services;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Player
{
    public class Player : NetworkBehaviour
    {
        public event Action<Player> Died;

        [SerializeField] private bool _isHostImmortal;
        [SerializeField] private PlayerModel _model;

        public NetworkString<_32> Nickname => _model.Nickname;
        public PlayerRef Ref { get; private set; }

        private CameraFollowHandler _cameraFollowHandler;
        private SaveLoadService _saveLoadService;
        private PlayerRegistry _playerRegistry;
        
        [Inject]
        private void Construct(CameraFollowHandler cameraFollowHandler, SaveLoadService saveLoadService,
            PlayerRegistry playerRegistry)
        {
            _cameraFollowHandler = cameraFollowHandler;
            _saveLoadService = saveLoadService;
            _playerRegistry = playerRegistry;
        }

        public override void Spawned()
        {
            _model.Died += OnDied;
            if (HasInputAuthority)
            {
                _cameraFollowHandler.SetTarget(transform);
                string nickName = _saveLoadService.Load().Nickname;
                _model.Rpc_SetNickname(nickName);
            }
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _model.Died -= OnDied;
        }

        public void Initialize(PlayerRef playerRef, PlayerConfig config)
        {
            _model.Initialize(config);
            Ref = playerRef;
        }

        private void OnDied()
        {
            if (HasStateAuthority)
                Died?.Invoke(this);
        }

        public void AddExperience(int value)
        {
            if (HasStateAuthority)
                _model.AddExperience(value);
        }

        public void Heal(int value)
        {
            if (HasStateAuthority)
                _model.ChangeHealth(value);
        }


        public void TakeDamage(int damage)
        {
            if (_playerRegistry.IsHost(this) && _isHostImmortal)
                return;

            _model.ChangeHealth(-damage);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}