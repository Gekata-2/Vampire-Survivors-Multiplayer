using Fusion;
using UnityEngine;

namespace _Project.Scripts.EnemyDrop
{
    public class LootDropSystem
    {
        private readonly EnemyDropConfig _enemyDropConfig;

        public LootDropSystem(EnemyDropConfig enemyDropConfig)
        {
            _enemyDropConfig = enemyDropConfig;
        }

        public NetworkPrefabRef GetRandomLoot()
        {
            int totalWeight = _enemyDropConfig.GetTotalWeight();
            int randomWeight = Random.Range(1, totalWeight + 1);

            int weightSum = 0;
            foreach (LootConfig lootData in _enemyDropConfig.Loot)
            {
                weightSum += lootData.DropWeight;

                if (randomWeight <= weightSum)
                    return lootData.Prefab;
            }

            return NetworkPrefabRef.Empty;
        }
    }
}