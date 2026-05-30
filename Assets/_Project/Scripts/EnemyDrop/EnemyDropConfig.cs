using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Project.Scripts.EnemyDrop
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Enemy Drop Config", fileName = "Enemy Drop Config", order = 0)]
    public class EnemyDropConfig : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<Loot, LootConfig> _loot;

        public int GetTotalWeight()
            => _loot.Values.Sum(lootData => lootData.DropWeight);

        public List<LootConfig> Loot => _loot.Values.ToList();
    }
}