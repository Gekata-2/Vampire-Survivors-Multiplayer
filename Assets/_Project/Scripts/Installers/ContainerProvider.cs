using _Project.Scripts.Services;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class ContainerProvider : MonoInstaller
    {
        [Inject] private SceneContainerProvider _sceneContainerProvider;

        public override void InstallBindings()
        {
            _sceneContainerProvider?.SetContainer(Container);
        }
    }
}