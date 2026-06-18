using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerVisuals : NetworkBehaviour
    {
        private static readonly int IsIdleHash = Animator.StringToHash("Idle");
        private static readonly int IsMovingHash = Animator.StringToHash("Move");
      
        [SerializeField] private List<SpriteRenderer> _renderers;
        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerMovement _movement;

        public override void Render()
        {
            UpdateSpriteOrientation();
            _animator.SetBool(IsIdleHash, Mathf.Approximately(_movement.Velocity.magnitude, 0f));
            _animator.SetBool(IsMovingHash, _movement.Velocity.magnitude > 0.001f);
        }

        private void UpdateSpriteOrientation()
        {
            if (Mathf.Approximately(_movement.Velocity.x, 0))
                return;

            Flip(_movement.Velocity.x < 0);
        }

        private void Flip(bool flip)
        {
            foreach (SpriteRenderer spriteRenderer in _renderers)
                spriteRenderer.flipX = flip;
        }
    }
}