using Fusion;
using UnityEngine;

namespace _Project.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : NetworkBehaviour
    {
        [Networked] private float _speed { get; set; }
        [Networked] public Vector2 Velocity { get; private set; }

        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Initialize(float speed)
        {
            _speed = speed;
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out PlayerInput input))
            {
                Move(input.MoveDirection);
                Velocity = _rigidbody.linearVelocity;
            }
            else if (HasStateAuthority)
            {
                Move(Vector2.zero);
            }
        }

        private void Move(Vector2 moveDirection)
        {
            _rigidbody.linearVelocity = moveDirection * _speed;
            Velocity = _rigidbody.linearVelocity;
        }

        public void SetSpeed(float speed)
            => _speed = speed;
    }
}