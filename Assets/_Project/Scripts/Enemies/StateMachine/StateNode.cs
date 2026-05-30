using System.Collections.Generic;

namespace _Project.Scripts.Enemies.StateMachine
{
    public class StateNode
    {
        public State State { get; }

        public HashSet<Transition> Transitions { get; } = new();

        public StateNode(State state)
        {
            State = state;
        }

        public void AddTransition(State to, Predicate condition)
            => Transitions.Add(new Transition(to, condition));
    }
}