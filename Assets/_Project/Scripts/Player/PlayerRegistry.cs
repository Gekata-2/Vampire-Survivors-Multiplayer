using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerRegistry
    {
        private readonly Dictionary<PlayerRef, Player> _players = new();

        private readonly HashSet<string> _deadPlayer = new();

        public List<Player> All => _players.Values.ToList();

        public Player Host { get; private set; }

        public bool IsDead(string nick)
            => _deadPlayer.Contains(nick);

        public void Add(Player player)
            => _players.Add(player.Ref, player);

        public bool IsAllDead()
            => _deadPlayer.Contains(Host.Nickname.Value) && _players.Count == 0;

        public bool IsHost(Player player)
            => Host.Ref == player.Ref;

        public void SetHost(Player player)
        {
            Host = player;
            _players.TryAdd(player.Ref, player);
        }

        public void Remove(PlayerRef playerRef)
        {
            if (_players.ContainsKey(playerRef))
                _players.Remove(playerRef);
        }

        public void MoveToDead(PlayerRef playerRef)
        {
            if (!_players.TryGetValue(playerRef, out Player player))
                return;
            if (_deadPlayer.Contains(player.Nickname.Value))
            {
                Debug.LogError($"Adding dead player twice :{player.Nickname.Value}");
                return;
            }

            _players.Remove(playerRef);
            _deadPlayer.Add(player.Nickname.Value);
        }

        public Player GetPlayer(PlayerRef playerRef)
            => _players.TryGetValue(playerRef, out Player player) ? player : null;
    }
}