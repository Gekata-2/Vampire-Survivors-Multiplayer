using System;
using System.Text;
using _Project.Scripts.Network;
using _Project.Scripts.Services;
using Cysharp.Threading.Tasks;
using Fusion;

namespace _Project.Scripts.MainMenu
{
    public class MainMenuModel
    {
        public event Action<ShutdownReason> ConnectionFailed;

        private readonly ExitGameService _exitGameService;
        private readonly SaveLoadService _saveLoadService;
        private readonly RoomNameGenerator _roomNameGenerator;
        private readonly NetworkRunnerFactory _runnerFactory;
        private readonly ZenjectNetworkObjectProvider _objectProvider;
        private readonly RunnerProvider _runnerProvider;

        public MainMenuModel(ExitGameService exitGameService, SaveLoadService saveLoadService,
            RoomNameGenerator roomNameGenerator, NetworkRunnerFactory runnerFactory,
            ZenjectNetworkObjectProvider objectProvider, RunnerProvider runnerProvider)
        {
            _exitGameService = exitGameService;
            _saveLoadService = saveLoadService;
            _roomNameGenerator = roomNameGenerator;
            _runnerFactory = runnerFactory;
            _objectProvider = objectProvider;
            _runnerProvider = runnerProvider;
        }

        public void ExitGame()
            => _exitGameService.PerformExit();

        public string GetCurrentNickname()
        {
            SaveData save = _saveLoadService.Load();
            return save == null ? string.Empty : save.Nickname;
        }

        public async UniTask StartHost(string nickname)
        {
            NetworkRunner runner = _runnerFactory.Create();

            _runnerProvider.SetRunner(runner);
            _saveLoadService.Save(new SaveData(nickname));
            NetworkSceneInfo sceneInfo = CreateNetworkSceneInfo();

            StartGameArgs startArguments = new StartGameArgs
            {
                GameMode = GameMode.AutoHostOrClient,
                SessionName = _roomNameGenerator.GetName(),
                Scene = sceneInfo,
                ObjectProvider = _objectProvider
            };

            StartGameResult result = await runner.StartGame(startArguments).AsUniTask();

            if (!result.Ok)
            {
                await runner.Shutdown().AsUniTask();
                _runnerProvider.SetRunner(null);
                ConnectionFailed?.Invoke(result.ShutdownReason);
            }
        }

        public async UniTask StartClient(string nickname, string roomName)
        {
            NetworkRunner runner = _runnerFactory.Create();
            _runnerProvider.SetRunner(runner);
            _saveLoadService.Save(new SaveData(nickname));
            NetworkSceneInfo sceneInfo = CreateNetworkSceneInfo();

            StartGameArgs startArguments = new StartGameArgs
            {
                GameMode = GameMode.Client,
                SessionName = roomName,
                Scene = sceneInfo,
                ConnectionToken = Encoding.UTF8.GetBytes(nickname),
                ObjectProvider = _objectProvider
            };

            StartGameResult result = await runner.StartGame(startArguments).AsUniTask();

            if (!result.Ok)
            {
                await runner.Shutdown().AsUniTask();
                _runnerProvider.SetRunner(null);
                ConnectionFailed?.Invoke(result.ShutdownReason);
            }
        }

        private NetworkSceneInfo CreateNetworkSceneInfo()
        {
            NetworkSceneInfo sceneInfo = new NetworkSceneInfo();
            sceneInfo.AddSceneRef(SceneRef.FromIndex(ScenesRepository.GameplaySceneIndex));
            return sceneInfo;
        }
    }
}