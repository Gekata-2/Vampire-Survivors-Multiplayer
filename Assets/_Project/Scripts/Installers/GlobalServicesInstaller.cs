using _Project.Scripts.MainMenu;
using _Project.Scripts.Services;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class GlobalServicesInstaller : MonoInstaller
    {
        [SerializeField] private ZenjectNetworkObjectProvider _objectProvider;

        public override void InstallBindings()
        {
            Container.Bind<ExitGameService>().AsSingle();
            Container.Bind<SaveLoadService>().AsSingle();
            Container.Bind<RoomNameGenerator>().AsSingle();
            Container.Bind<SceneContainerProvider>().AsSingle();
            Container.Bind<RunnerProvider>().AsSingle();
            Container.Bind<ZenjectNetworkObjectProvider>().FromInstance(_objectProvider).AsSingle();
        }
    }
}