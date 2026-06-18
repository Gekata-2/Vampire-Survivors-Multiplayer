using UnityEngine;

namespace _Project.Scripts.Enemies
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Enemy Config", fileName = "Enemy Config", order = 0)]
    public class EnemyConfig : ScriptableObject
    {
        [Header("Defences")]
        [field: SerializeField] public int Health { get; private set; } = 100;

        [Header("Movement")]
        [field: SerializeField] public float MoveSpeed { get; private set; } = 5f;

        [Header("Combat")]
        [field: SerializeField] public float AttackCooldown { get; private set; } = 2f;
        [field: SerializeField] public float AttackRadius { get; private set; } = 1.5f;
        [field: SerializeField] public int Damage { get; private set; } = 10;
        [field: SerializeField] public float DetectionRadius { get; private set; } = 10f;
        [field: SerializeField] public float AttackDuration { get; private set; } = 1f;
        [field: SerializeField] public float DamageDelay { get; private set; } = 0.5f;
        [field: SerializeField] public float HitRadius { get; private set; } = 0.25f;
    }
}