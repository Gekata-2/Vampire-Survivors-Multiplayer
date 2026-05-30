namespace _Project.Scripts.Enemies.StateMachine.States
{
    public class DieState : State
    {
        public DieState(Enemy enemy) : base(enemy)
        {
        }

        public override void OnEnter()
        {
            Enemy.Die();
        }
    }
}