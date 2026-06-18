using System;
using System.Collections.Generic;
using System.Text;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Player
{
    public class PlayersController : NetworkBehaviour, IPlayerJoined, IPlayerLeft, INetworkRunnerCallbacks
    {
        [SerializeField] private float _spawnRadius = 2f;
        [SerializeField] private Player _prefab;

        private PlayerRegistry _registry;
        private PlayerConfig _playerConfig;

        private bool _isHostSet;

        [Inject]
        private void Construct(PlayerConfig playerConfig, PlayerRegistry playerRegistry)
        {
            _playerConfig = playerConfig;
            _registry = playerRegistry;
        }
        
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request,
            byte[] token)
        {
            string playerNickname = Encoding.UTF8.GetString(token);
            if (_registry.IsDead(playerNickname))
                request.Refuse();
        }

        public void PlayerJoined(PlayerRef player)
        {
            if (!HasStateAuthority)
                return;

            Player playerInst = Runner.Spawn(_prefab,
                Random.insideUnitCircle * _spawnRadius,
                Quaternion.identity,
                player);
            Runner.SetPlayerObject(player, playerInst.Object);
            playerInst.Initialize(player, _playerConfig);
            SubscribeToPlayer(playerInst);
            if (!_isHostSet)
            {
                _registry.SetHost(playerInst);
                _isHostSet = true;
            }
            else
                _registry.Add(playerInst);
        }

        private void SubscribeToPlayer(Player player)
        {
            player.Died += OnPlayerDied;
        }

        private void UnsubscribeFromPlayer(Player player)
        {
            player.Died -= OnPlayerDied;
        }

        private void OnPlayerDied(Player player)
        {
            _registry.MoveToDead(player.Ref);
            if (player != _registry.Host)
            {
                DespawnPlayerObject(player);
                Runner.Disconnect(player.Ref);
            }
            else
            {
                player.Disable();
            }

            if (_registry.IsAllDead())
            {
                DespawnPlayerObject(player);
                Runner.Shutdown();
            }
        }

        private void DespawnPlayerObject(Player player)
        {
            UnsubscribeFromPlayer(player);
            Runner.Despawn(player.Object);
            Runner.SetPlayerObject(player.Ref, null);
        }

        public void PlayerLeft(PlayerRef playerRef)
        {
            if (Runner.TryGetPlayerObject(playerRef, out _))
            {
                Player player = _registry.GetPlayer(playerRef);
                _registry.Remove(playerRef);
                DespawnPlayerObject(player);
            }
        }

        #region NetworkCallbacks

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
        }


        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key,
            ArraySegment<byte> data)
        {
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }

        #endregion
    }
}