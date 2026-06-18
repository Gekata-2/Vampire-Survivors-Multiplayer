using System;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.EnemyDrop
{
    [Serializable]
    public class LootConfig
    {
        [field: SerializeField] public int DropWeight { get; private set; }
        [field: SerializeField] public NetworkPrefabRef Prefab { get; private set; }
    }
}