using System;
using System.Collections.Generic;

namespace _Project.Scripts.Enemies.StateMachine
{
    public class StateMachine
    {
        private StateNode _current;
        private readonly Dictionary<Type, StateNode> _nodes = new();
        private readonly HashSet<Transition> _anyTransition = new();

        public void UpdateTransition()
        {
            Transition transition = GetTransition();
            if (transition != null)
                ChangeState(transition.To);
        }

        public void NetworkUpdate()
        {
            _current.State?.NetworkUpdate();
        }


        public void SetState(State state)
        {
            _current = _nodes[state.GetType()];
            _current.State?.OnEnter();
        }

        private void ChangeState(State state)
        {
            if (state == _current.State)
                return;

            StateNode nextStateNode = _nodes[state.GetType()];

            State previousState = _current.State;
            State nextState = nextStateNode.State;
            
            previousState.OnExit();
            nextState.OnEnter();

            _current = nextStateNode;
        }

        private Transition GetTransition()
        {
            foreach (Transition transition in _anyTransition)
                if (transition.Condition.Evaluate())
                    return transition;

            foreach (Transition transition in _current.Transitions)
                if (transition.Condition.Evaluate())
                    return transition;

            return null;
        }

        public void AddTransition(State from, State to, Predicate condition)
            => GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);

        public void AddAnyTransition(State to, Predicate condition) =>
            _anyTransition.Add(new Transition(GetOrAddNode(to).State, condition));

        private StateNode GetOrAddNode(State state)
        {
            StateNode node = _nodes.GetValueOrDefault(state.GetType());
            if (node == null)
            {
                node = new StateNode(state);
                _nodes.Add(state.GetType(), node);
            }

            return node;
        }
    }
}