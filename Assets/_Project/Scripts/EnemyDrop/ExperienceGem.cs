using Fusion;
using UnityEngine;

namespace _Project.Scripts.EnemyDrop
{
    public class ExperienceGem : NetworkBehaviour
    {
        [SerializeField] private int _experience;
        [SerializeField] private Collider2D _collider;

        public override void Spawned()
        {
            _collider.enabled = HasStateAuthority;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (HasStateAuthority && other.TryGetComponent(out Player.Player player))
            {
                _collider.enabled = false;
                player.AddExperience(_experience);
                Runner.Despawn(Object);
            }
        }
    }
}