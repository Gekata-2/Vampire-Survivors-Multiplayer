using UnityEngine;

namespace _Project.Scripts.Enemies.StateMachine.States
{
    public class ChaseState : State
    {
        private readonly PlayerDetector _playerDetector;

        public ChaseState(Enemy enemy, PlayerDetector playerDetector) : base(enemy)
        {
            _playerDetector = playerDetector;
        }

        public override void OnEnter()
        {
            Enemy.IsMoving = true;
        }

        public override void OnExit()
        {
            Enemy.IsMoving = false;
        }

        public override void NetworkUpdate()
        {
            _playerDetector.DetectPlayer();
            Enemy.Velocity = _playerDetector.Target != null ? GetPlayerDirection() * Enemy.Speed : Vector2.zero;
        }

        private Vector2 GetPlayerDirection()
            => ((Vector2)_playerDetector.Target.transform.position - Enemy.Position).normalized;
    }
}