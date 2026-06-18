using System.Collections.Generic;
using _Project.Scripts.Enemies;
using Fusion;
using Fusion.LagCompensation;
using UnityEngine;

namespace _Project.Scripts.Player.Combat
{
    public class Arrow : NetworkBehaviour
    {
        [SerializeField] private Transform _hitPoint;
        [SerializeField] private Transform _castPoint;
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private float _castRadius = 0.12f;

        private TickTimer _timer;
        private Vector3 _velocity;
        private int _damage;
        private bool _dealtDamage;
        private readonly List<LagCompensatedHit> _overlapHits = new();

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority)
                return;

            if (_timer.Expired(Runner))
                Runner.Despawn(Object);
            else if (WillCollideNextFrame(out Enemy enemy))
                DamageEnemy(enemy);
            else if (IsInsideEnemy(out enemy))
                DamageEnemy(enemy);
            else
                Move();
        }

        private bool WillCollideNextFrame(out Enemy enemy)
        {
            enemy = null;
            bool willHit = Runner.LagCompensation.Raycast(_hitPoint.position, transform.right,
                _velocity.magnitude * Runner.DeltaTime, Object.InputAuthority, out var hit, _enemyLayer);

            if (!willHit)
                return false;

            enemy = hit.GameObject.GetComponent<Enemy>();
            return true;
        }

        private bool IsInsideEnemy(out Enemy enemy)
        {
            _overlapHits.Clear();
            enemy = null;
            int hitCount = Runner.LagCompensation.OverlapSphere(_castPoint.position, _castRadius, Object.InputAuthority,
                _overlapHits, _enemyLayer);

            if (hitCount == 0)
                return false;

            _overlapHits.SortDistance();
            enemy = _overlapHits[0].GameObject.GetComponent<Enemy>();

            return true;
        }

        private void Move() 
            => transform.position += _velocity * Runner.DeltaTime;

        private void DamageEnemy(Enemy enemy)
        {
            if (_dealtDamage)
                return;

            enemy.Damage(_damage);
            _dealtDamage = true;
            Runner.Despawn(Object);
        }

        public void Initialize(float speed, float liveDuration, int damage)
        {
            if (!HasStateAuthority)
                return;

            _velocity = transform.right * speed;
            _timer = TickTimer.CreateFromSeconds(Runner, liveDuration);
            _damage = damage;
        }

        private void OnDrawGizmos()
        {
            if (_castPoint == null) return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_castPoint.position, _castRadius);
        }
    }
}