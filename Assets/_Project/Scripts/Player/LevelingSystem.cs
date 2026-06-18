using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Player
{
    public class LevelingSystem
    {
        private const float MULTIPLIER_EXP = 1.5f;
        private const int BASE_EXP = 10;

        private const int HEALTH_DELTA = 30;
        private const int ATTACK_DAMAGE_DELTA = 5;
        private const float MOVE_SPEED_DELTA = 0.5f;
        private const float ATTACK_SPEED_DELTA = 0.3f;

        private readonly List<Action<PlayerStats>> _buffs = new();

        public LevelingSystem()
        {
            _buffs.Add(IncreaseHealth);
            _buffs.Add(IncreaseDamage);
            _buffs.Add(IncreaseMoveSpeed);
            _buffs.Add(IncreaseAttackSpeed);
        }

        public int GetExperienceForLevel(int level)
        {
            if (level <= 0) return 0;
            return (int)(2 * BASE_EXP * (Mathf.Pow(MULTIPLIER_EXP, level) - 1));
        }

        public void LevelUp(PlayerStats stats)
            => GetRandomBuff().Invoke(stats);

        private Action<PlayerStats> GetRandomBuff()
            => _buffs[Random.Range(0, _buffs.Count)];

        private void IncreaseHealth(PlayerStats stats) => stats.MaxHealth += HEALTH_DELTA;

        private void IncreaseDamage(PlayerStats stats) => stats.Damage += ATTACK_DAMAGE_DELTA;

        private void IncreaseMoveSpeed(PlayerStats stats) => stats.MoveSpeed += MOVE_SPEED_DELTA;

        private void IncreaseAttackSpeed(PlayerStats stats) => stats.AttackSpeed += ATTACK_SPEED_DELTA;
    }
}