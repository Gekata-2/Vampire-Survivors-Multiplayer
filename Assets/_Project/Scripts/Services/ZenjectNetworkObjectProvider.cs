using Fusion;
using Zenject;

namespace _Project.Scripts.Services
{
    public class ZenjectNetworkObjectProvider : NetworkObjectProviderDefault
    {
        private SceneContainerProvider _containerProvider;

        [Inject]
        private void Construct(SceneContainerProvider sceneContainerProvider)
        {
            _containerProvider = sceneContainerProvider;
        }

        protected override NetworkObject InstantiatePrefab(
            NetworkRunner runner,
            NetworkObject prefab)
        {
            return _containerProvider.Container.InstantiatePrefabForComponent<NetworkObject>(
                prefab.gameObject);
        }

        protected override void DestroyPrefabInstance(
            NetworkRunner runner,
            NetworkPrefabId prefabId,
            NetworkObject instance)
        {
            Destroy(instance.gameObject);
        }
    }
}