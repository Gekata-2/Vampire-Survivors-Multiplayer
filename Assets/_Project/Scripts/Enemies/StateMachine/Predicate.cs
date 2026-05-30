using System;

namespace _Project.Scripts.Enemies.StateMachine
{
    public class Predicate
    {
        private readonly Func<bool> _func;

        public Predicate(Func<bool> func)
        {
            _func = func;
        }

        public bool Evaluate()
            => _func.Invoke();
    }
}