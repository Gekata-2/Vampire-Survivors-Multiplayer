using System;
using Fusion;

namespace _Project.Scripts.Player
{
    public class PlayerStats : NetworkBehaviour
    {
        public event Action AttackSpeedChanged;
        public event Action DamageChanged;
        public event Action MoveSpeedChanged;
        public event Action MaxHealthChanged;

        [Networked, OnChangedRender(nameof(OnAttackSpeedChanged))]
        public float AttackSpeed { get; set; }

        [Networked, OnChangedRender(nameof(OnDamageChanged))]
        public int Damage { get; set; }

        [Networked, OnChangedRender(nameof(OnMoveSpeedChanged))]
        public float MoveSpeed { get; set; }

        [Networked, OnChangedRender(nameof(OnMaxHealthChanged))]
        public int MaxHealth { get; set; }

        private void OnAttackSpeedChanged()
            => AttackSpeedChanged?.Invoke();

        private void OnDamageChanged()
            => DamageChanged?.Invoke();

        private void OnMoveSpeedChanged()
            => MoveSpeedChanged?.Invoke();

        private void OnMaxHealthChanged()
            => MaxHealthChanged?.Invoke();
    }
}