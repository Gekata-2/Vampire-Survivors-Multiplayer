using System;
using _Project.Scripts.Player.Combat;
using _Project.Scripts.Systems;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Player
{
    public class PlayerModel : NetworkBehaviour
    {
        public event Action HealthChanged;
        public event Action Died;
        public event Action ExperienceChanged;
        public event Action ArrowFired;
        public event Action NicknameChanged;

        [field: SerializeField] public PlayerMovement Movement { get; private set; }
        [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public Experience Experience { get; private set; }
        [field: SerializeField] public PlayerStats Stats { get; private set; }
        [field: SerializeField] public PlayerCombat Combat { get; private set; }

        private LevelingSystem _levelingSystem;

        [Networked, OnChangedRender(nameof(OnNicknameChanged))]
        public NetworkString<_32> Nickname { get; private set; }

        private void Awake()
        {
            Movement = GetComponent<PlayerMovement>();
            Health = GetComponent<Health>();
            Experience = GetComponent<Experience>();
            Stats = GetComponent<PlayerStats>();
            Combat = GetComponent<PlayerCombat>();
        }

        [Inject]
        private void Construct(LevelingSystem levelingSystem)
        {
            _levelingSystem = levelingSystem;
        }

        public override void Spawned()
        {
            Health.HealthChanged += OnHealthChanged;
            Health.MaxChanged += OnMaxHealthChanged;
            Health.ZeroReached += OnDied;

            Experience.LevelChanged += OnLevelChanged;
            Experience.ExperienceChanged += OnExperienceChanged;

            Stats.MaxHealthChanged += OnMaxHealthStatChanged;
            Stats.AttackSpeedChanged += OnAttackSpeedChanged;
            Stats.MoveSpeedChanged += OnMoveSpeedChanged;
            Stats.DamageChanged += OnDamageChanged;

            Combat.ArrowFired += OnArrowFired;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            Health.HealthChanged -= OnHealthChanged;
            Health.MaxChanged -= OnMaxHealthChanged;
            Health.ZeroReached -= OnDied;

            Experience.LevelChanged -= OnLevelChanged;
            Experience.ExperienceChanged -= OnExperienceChanged;

            Stats.MaxHealthChanged -= OnMaxHealthStatChanged;
            Stats.AttackSpeedChanged -= OnAttackSpeedChanged;
            Stats.MoveSpeedChanged -= OnMoveSpeedChanged;
            Stats.DamageChanged -= OnDamageChanged;

            Combat.ArrowFired -= OnArrowFired;
        }

        public void Initialize(PlayerConfig config)
        {
            Experience.Initialize(0, 0, _levelingSystem.GetExperienceForLevel(1));
            Health.Initialize(config.Health, config.Health);
            Stats.MaxHealth = config.Health;
            Stats.MoveSpeed = config.MoveSpeed;
            Stats.AttackSpeed = config.AttackSpeed;
            Stats.Damage = config.Damage;
            Movement.Initialize(config.MoveSpeed);
            Combat.Initialize(config.AttackRadius, config.AttackSpeed, config.Damage, config.ArrowSpeed);
        }

        private void OnMaxHealthStatChanged()
        {
            if (HasStateAuthority)
                Health.SetMaxHealth(Stats.MaxHealth);
        }

        private void OnArrowFired()
        {
            ArrowFired?.Invoke();
        }

        private void OnDamageChanged()
        {
            if (HasStateAuthority)
                Combat.SetDamage(Stats.Damage);
        }

        private void OnMoveSpeedChanged()
        {
            if (HasStateAuthority)
                Movement.SetSpeed(Stats.MoveSpeed);
        }

        private void OnAttackSpeedChanged()
        {
            if (HasStateAuthority)
                Combat.SetAttackSpeed(Stats.AttackSpeed);
        }

        private void OnExperienceChanged(int obj)
        {
            ExperienceChanged?.Invoke();
        }

        private void OnLevelChanged(int obj)
        {
            if (HasStateAuthority)
            {
                _levelingSystem.LevelUp(Stats);
                Health.HealFull();
            }

            ExperienceChanged?.Invoke();
        }

        private void OnDied()
        {
            Died?.Invoke();
        }

        private void OnMaxHealthChanged(int obj)
        {
            HealthChanged?.Invoke();
        }

        private void OnHealthChanged(int obj)
        {
            HealthChanged?.Invoke();
        }

        public void OnNicknameChanged()
            => NicknameChanged?.Invoke();


        public void ChangeHealth(int delta)
        {
            Health.Modify(delta);
        }

        public void AddExperience(int value)
        {
            Experience.AddExperience(value);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void Rpc_SetNickname(string nick)
            => Nickname = nick;
    }
}