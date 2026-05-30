using Fusion;
using UnityEngine;

namespace _Project.Scripts.Misc
{
    public class ObjectSpawner : NetworkBehaviour
    {
        [SerializeField] private float _radius;
        [SerializeField] private KeyCode _key;
        [SerializeField] private NetworkPrefabRef _prefab;

        public override void FixedUpdateNetwork()
        {
            if (HasStateAuthority && Input.GetKeyDown(_key))
            {
                Vector3 position = transform.position + (Vector3)(Random.insideUnitCircle * _radius);
                NetworkObject obj = Runner.Spawn(_prefab, position, Quaternion.identity);
                obj.transform.SetParent(transform);
            }
        }
    }
}