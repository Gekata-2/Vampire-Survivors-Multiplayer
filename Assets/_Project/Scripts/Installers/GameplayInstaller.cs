using _Project.Scripts.Enemies;
using _Project.Scripts.EnemyDrop;
using _Project.Scripts.Player;
using _Project.Scripts.Services;
using _Project.Scripts.UI;
using _Project.Scripts.UI.QuickMenu;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private EnemyDropConfig _enemyDropConfig;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private EnemyConfig _enemyConfig;

        public override void InstallBindings()
        {
            Container.Bind<LevelingSystem>().AsSingle();
            Container.Bind<SceneLoader>().AsSingle();
            Container.Bind<EnemyRegistry>().AsSingle();
            Container.Bind<PlayerRegistry>().AsSingle();

            Container.Bind<GameHUD>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CameraFollowHandler>().FromComponentInHierarchy().AsSingle();

            Container.Bind<QuickMenu>().FromComponentInHierarchy().AsSingle();
            Container.Bind<QuickMenuModel>().AsSingle();
            Container.BindInterfacesTo<QuickMenuPresenter>().AsSingle();

            Container.Bind<EnemyDropConfig>().FromScriptableObject(_enemyDropConfig).AsSingle();
            Container.Bind<PlayerConfig>().FromScriptableObject(_playerConfig).AsSingle();
            Container.Bind<EnemyConfig>().FromScriptableObject(_enemyConfig).AsSingle();
            Container.Bind<LootDropSystem>().AsSingle();
        }
    }
}