using _Project.Scripts.Player;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Enemies
{
    public class PlayerDetector : MonoBehaviour
    {
        [SerializeField] private float _detectionRadius = 10f;

        public Player.Player Target { get; private set; }

        private PlayerRegistry _playerRegistry;


        [Inject]
        private void Construct(PlayerRegistry playerRegistry)
        {
            _playerRegistry = playerRegistry;
        }

        public void Initialize(float detectionRadius)
        {
            _detectionRadius = detectionRadius;
        }

        public void DetectPlayer()
        {
            Target = FindClosestPlayer();
        }

        private Player.Player FindClosestPlayer()
        {
            float minDistance = float.MaxValue;
            Player.Player closest = null;
            foreach (Player.Player player in _playerRegistry.All)
            {
                float distance = GetDistance(player.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = player;
                }
            }

            return minDistance <= _detectionRadius ? closest : null;
        }

        private float GetDistance(Vector3 position)
            => Vector3.Distance(transform.position, position);
    }
}