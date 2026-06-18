namespace _Project.Scripts.Enemies.StateMachine.States
{
    public class IdleState : State
    {
        private readonly PlayerDetector _playerDetector;

        public IdleState(Enemy enemy, PlayerDetector playerDetector) : base(enemy)
        {
            _playerDetector = playerDetector;
        }

        public override void OnEnter()
        {
            Enemy.IsIdle = true;
        }

        public override void OnExit()
        {
            Enemy.IsIdle = false;
        }

        public override void NetworkUpdate() =>
            _playerDetector.DetectPlayer();
    }
}