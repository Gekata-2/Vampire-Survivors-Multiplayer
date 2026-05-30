namespace _Project.Scripts.Enemies.StateMachine.States
{
    public class AttackState : State
    {
        private readonly PlayerDetector _playerDetector;

        public AttackState(Enemy enemy, PlayerDetector playerDetector) : base(enemy)
        {
            _playerDetector = playerDetector;
        }

        public override void OnEnter()
        {
            _playerDetector.DetectPlayer();
            Enemy.BeginAttack(_playerDetector.Target);
        }

        public override void NetworkUpdate()
        {
            _playerDetector.DetectPlayer();
            Enemy.ProcessAttack();
        }

        public override void OnExit()
        {
            Enemy.EndAttack();
        }
    }
}