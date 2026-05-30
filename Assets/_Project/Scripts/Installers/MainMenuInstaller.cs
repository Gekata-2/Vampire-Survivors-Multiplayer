using _Project.Scripts.MainMenu;
using _Project.Scripts.Network;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private NetworkRunner _runnerPrefab;

        public override void InstallBindings()
        {
            Container.BindFactory<NetworkRunner, NetworkRunnerFactory>().FromComponentInNewPrefab(_runnerPrefab);
            Container.Bind<MainMenuView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<MainMenuPresenter>().AsSingle();
            Container.Bind<MainMenuModel>().AsSingle();
        }
    }
}