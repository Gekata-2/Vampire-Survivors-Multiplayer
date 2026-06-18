using System;
using System.Collections.Generic;
using _Project.Scripts.Enemies.StateMachine;
using _Project.Scripts.Enemies.StateMachine.States;
using _Project.Scripts.EnemyDrop;
using _Project.Scripts.Systems;
using Fusion;
using Fusion.LagCompensation;
using UnityEngine;

namespace _Project.Scripts.Enemies
{
    [RequireComponent(typeof(Health), typeof(Rigidbody2D), typeof(Collider2D))]
    public class Enemy : NetworkBehaviour, IBeforeUpdate
    {
        public event Action<Enemy> Died;

        [SerializeField] private PlayerDetector _playerDetector;

        [Header("Movement")] [SerializeField] private float _speed = 3f;

        [Header("Combat")] [SerializeField] private int _damage;
        [SerializeField] private float _attackRadius = 1.5f;
        [SerializeField] private float _attackCooldown;
        [SerializeField] private float _damageDelay;
        [SerializeField, Min(0.001f)] private float _attackDuration;
        [SerializeField] private float _hitRadius = 0.25f;
        [SerializeField] private Transform _hitOrigin;
        [SerializeField] private Transform _hitOriginFlipped;
        [SerializeField] private LayerMask _playerCollisionLayer;

        private Health _health;
        private Collider2D _collider;
        private Rigidbody2D _rigidbody;

        public Vector3 Center
            => _collider.bounds.center;

        private StateMachine.StateMachine _stateMachine;

        public Vector2 Position => _rigidbody.position;
        public float Speed => _speed;

        [Networked] public bool IsAttacking { get; private set; }
        [Networked] public bool IsIdle { get; set; }
        [Networked] public bool IsMoving { get; set; }
        [Networked] public bool IsDead { get; private set; }
        [Networked] public bool IsFlipped { get; private set; }
        [Networked] private Vector2 CurrentVelocity { get; set; }

        private TickTimer _attackCooldownTimer;
        private TickTimer _damageTimer;
        private TickTimer _attackEndTimer;

        private bool _damageDealt;


        private Player.Player _attackTarget;
        private LootDropSystem _lootDropSystem;

        private readonly List<LagCompensatedHit> _lagCompensatedHits = new();

        public Vector2 Velocity
        {
            set
            {
                _rigidbody.linearVelocity = value;
                CurrentVelocity = _rigidbody.linearVelocity;
            }
        }

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _health = GetComponent<Health>();
        }


        public void Initialize(LootDropSystem lootDropSystem, EnemyConfig config)
        {
            _lootDropSystem = lootDropSystem;
            _health.Initialize(config.Health, config.Health);
            _damage = config.Damage;
            _attackCooldown = config.AttackCooldown;
            _attackRadius = config.AttackRadius;
            _attackDuration = config.AttackDuration;
            _damageDelay = config.DamageDelay;
            _hitRadius = config.HitRadius;
            _playerDetector.Initialize(config.DetectionRadius);
            _speed = config.MoveSpeed;
        }

        public override void Spawned()
        {
            _health.ZeroReached += OnHealthZeroReached;
            _stateMachine = new StateMachine.StateMachine();

            IdleState idle = new IdleState(this, _playerDetector);
            ChaseState chase = new ChaseState(this, _playerDetector);
            AttackState attack = new AttackState(this, _playerDetector);
            DieState dieState = new DieState(this);
            _stateMachine.AddTransition(idle, chase, new Predicate(() => _playerDetector.Target != null));
            _stateMachine.AddTransition(chase, idle, new Predicate(() => _playerDetector.Target == null));

            _stateMachine.AddTransition(chase, attack,
                new Predicate(() => IsPlayerInAttackRadius() && _attackCooldownTimer.ExpiredOrNotRunning(Runner)));
            _stateMachine.AddTransition(attack, chase, new Predicate(() => !IsAttacking));

            _stateMachine.AddAnyTransition(dieState, new Predicate(() => IsDead));
            _stateMachine.SetState(idle);
        }


        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _health.ZeroReached -= OnHealthZeroReached;
        }

        public void BeforeUpdate()
        {
            _stateMachine.UpdateTransition();
        }

        public override void FixedUpdateNetwork()
        {
            if (HasStateAuthority)
            {
                _stateMachine.NetworkUpdate();
                HandleFlip();
            }
        }

        public void BeginAttack(Player.Player target)
        {
            _attackTarget = target;
            _damageDealt = false;

            Velocity = Vector2.zero;
            IsAttacking = true;

            _damageTimer = TickTimer.CreateFromSeconds(Runner, _damageDelay);
            _attackEndTimer = TickTimer.CreateFromSeconds(Runner, _attackDuration);
        }


        private void HandleFlip()
        {
            if (CurrentVelocity.x == 0) return;

            if (CurrentVelocity.x > 0) IsFlipped = false;
            if (CurrentVelocity.x < 0) IsFlipped = true;
        }

        public void ProcessAttack()
        {
            if (!HasStateAuthority)
                return;

            if (!_damageDealt && _damageTimer.Expired(Runner))
            {
                _damageDealt = true;
                if (_attackTarget != null)
                {
                    if (TryHitTarget(out _attackTarget))
                        _attackTarget.TakeDamage(_damage);
                }
            }

            if (_attackEndTimer.Expired(Runner))
            {
                IsAttacking = false;
                _attackCooldownTimer = TickTimer.CreateFromSeconds(Runner, _attackCooldown);
            }
        }

        private bool TryHitTarget(out Player.Player target)
        {
            _lagCompensatedHits.Clear();

            Vector3 hitOriginPosition = IsFlipped ? _hitOriginFlipped.position : _hitOrigin.position;
            int count = Runner.LagCompensation.OverlapSphere(hitOriginPosition, _hitRadius,
                _attackTarget.Ref,
                _lagCompensatedHits, _playerCollisionLayer);

            if (count == 0)
            {
                target = null;
                return false;
            }

            _lagCompensatedHits.SortDistance();
            target = _lagCompensatedHits[0].GameObject.GetComponent<Player.Player>();

            return true;
        }

        public void EndAttack()
        {
            _attackTarget = null;
            _damageDealt = false;
        }

        private bool IsPlayerInAttackRadius()
        {
            if (_playerDetector.Target == null)
                return false;

            return Vector2.Distance(_playerDetector.Target.transform.position, Position) <= _attackRadius;
        }

        private void OnHealthZeroReached()
        {
            IsDead = true;
        }

        public void Die()
        {
            if (!HasStateAuthority)
                return;

            NetworkPrefabRef prefabRef = _lootDropSystem.GetRandomLoot();
            if (prefabRef.IsValid && prefabRef != NetworkPrefabRef.Empty)
                Runner.Spawn(prefabRef, Position, Quaternion.identity);
            Runner.Despawn(Object);
            Died?.Invoke(this);
        }

        public void Damage(int value)
            => _health.Modify(-value);
    }
}