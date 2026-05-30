using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Enemies
{
    public class EnemyVisuals : NetworkBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Enemy _enemy;
        [SerializeField] private List<SpriteRenderer> _renderers;

        private static readonly int IsAttackingHash = Animator.StringToHash("Attack");
        private static readonly int IsIdleHash = Animator.StringToHash("Idle");
        private static readonly int IsMovingHash = Animator.StringToHash("Move");
        private static readonly int IsDyingHash = Animator.StringToHash("Die");

        public override void Render()
        {
            _animator.SetBool(IsAttackingHash, _enemy.IsAttacking);
            _animator.SetBool(IsDyingHash, _enemy.IsDead);
            _animator.SetBool(IsMovingHash, _enemy.IsMoving);
            _animator.SetBool(IsIdleHash, _enemy.IsIdle);

            foreach (SpriteRenderer spriteRenderer in _renderers) 
                spriteRenderer.flipX = _enemy.IsFlipped;
        }
        
    }
}