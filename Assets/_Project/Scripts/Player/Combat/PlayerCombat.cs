using System;
using _Project.Scripts.Enemies;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Player.Combat
{
    public class PlayerCombat : NetworkBehaviour
    {
        public event Action ArrowFired;

        [SerializeField] private Arrow _arrowPrefab;
        [SerializeField] private float _arrowTTL = 5f;
        [Networked] private float _attackRadius { get; set; }
        [Networked] private float _attackSpeed { get; set; }
        [Networked] private int _damage { get; set; }
        [Networked] private float _arrowSpeed { get; set; }

        private Enemy _target;
        private EnemyRegistry _enemyRegistry;

        [Networked] private TickTimer _attackTimer { get; set; }

        [Networked, OnChangedRender(nameof(OnArrowFired))]
        public int ArrowsFired { get; private set; }

        private float ReloadTime => 1f / _attackSpeed;

        [Inject]
        private void Construct(EnemyRegistry enemyRegistry)
        {
            _enemyRegistry = enemyRegistry;
        }

        public void Initialize(float attackRadius, float attackSpeed, int damage, float arrowSpeed)
        {
            _attackRadius = attackRadius;
            _attackSpeed = attackSpeed;
            _damage = damage;
            _arrowSpeed = arrowSpeed;
        }

        private void OnArrowFired()
        {
            ArrowFired?.Invoke();
        }

        public float GetReloadProgress()
        {
            float? remainingTime = _attackTimer.RemainingTime(Runner);
            float reloadTime = ReloadTime;
            float? progress = (reloadTime - remainingTime) / reloadTime;
            return progress ?? 0f;
        }


        private Enemy FindNearestEnemy()
        {
            float minDistance = float.MaxValue;
            Enemy closestEnemy = null;
            foreach (Enemy enemy in _enemyRegistry.Enemies)
            {
                float distance = Vector3.Distance(enemy.transform.position, transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestEnemy = enemy;
                }
            }

            return minDistance <= _attackRadius ? closestEnemy : null;
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority)
                return;

            _target = FindNearestEnemy();
            if (_target == null)
                return;

            if (_attackTimer.ExpiredOrNotRunning(Runner))
            {
                ShootArrow(_target.Center);
                _attackTimer = CreateTimer();
            }
        }

        private TickTimer CreateTimer()
            => TickTimer.CreateFromSeconds(Runner, ReloadTime);

        private void ShootArrow(Vector3 targetPosition)
        {
            Vector3 playerPosition = transform.position;
            Quaternion rotation = CalculateRotation(targetPosition, playerPosition);
            Arrow arrow = Runner.Spawn(_arrowPrefab, playerPosition, rotation, Object.InputAuthority);
            arrow.Initialize(_arrowSpeed, _arrowTTL, _damage);
            ArrowsFired++;
        }

        private static Quaternion CalculateRotation(Vector3 targetPosition, Vector3 playerPosition)
        {
            Vector3 targetDirection = targetPosition - playerPosition;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            return Quaternion.Euler(new Vector3(0f, 0f, angle));
        }

        public void SetDamage(int value)
        {
            if (HasStateAuthority) _damage = value;
        }

        public void SetAttackSpeed(float value)
        {
            if (HasStateAuthority) _attackSpeed = value;
        }
    }
}