using Fusion;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public struct PlayerInput : INetworkInput
    {
        public Vector2 MoveDirection;
    }
}