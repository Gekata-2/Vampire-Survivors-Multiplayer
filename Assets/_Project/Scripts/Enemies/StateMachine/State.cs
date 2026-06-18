namespace _Project.Scripts.Enemies.StateMachine
{
    public class State
    {
        protected readonly Enemy Enemy;

        public State(Enemy enemy) => Enemy = enemy;

        public virtual void NetworkUpdate()
        {
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }
    }
}