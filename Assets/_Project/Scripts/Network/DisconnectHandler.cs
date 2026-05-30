using _Project.Scripts.Services;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Network
{
    [RequireComponent(typeof(NetworkEvents))]
    public class DisconnectHandler : NetworkBehaviour
    {
        private NetworkEvents _events;
        private SceneLoader _sceneLoader;

        [Inject]
        private void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void Awake()
        {
            _events = GetComponent<NetworkEvents>();
        }

        public override void Spawned()
        {
            Runner.AddCallbacks(_events);
        }

        private void Start()
        {
            _events.OnShutdown.AddListener(OnShutdown);
            _events.OnDisconnectedFromServer.AddListener(OnDisconnectedFromServer);
        }

        private void OnDestroy()
        {
            _events.OnShutdown.RemoveListener(OnShutdown);
            _events.OnDisconnectedFromServer.RemoveListener(OnDisconnectedFromServer);
        }

        private void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            _sceneLoader.LoadMainMenu();
        }

        private void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
            if (reason == NetDisconnectReason.Requested)
                Runner.Shutdown();
        }
    }
}