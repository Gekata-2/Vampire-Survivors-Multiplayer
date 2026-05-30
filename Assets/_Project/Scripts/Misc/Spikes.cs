using _Project.Scripts.Systems;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Misc
{
    public class Spikes : NetworkBehaviour
    {
        [SerializeField] private int _damage = 5;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (HasStateAuthority && other.TryGetComponent(out Health health))
                health.Modify(-_damage);
        }
    }
}