using UnityEngine;

namespace _Project.Scripts.Player
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Player Config", fileName = "Player Config", order = 0)]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Defences")]
        [field: SerializeField]
        public int Health { get; private set; } = 100;
        [Header("Movement")]
        [field: SerializeField] public float MoveSpeed { get; private set; } = 5f;

        [Header("Combat")]
        [field: SerializeField] public float AttackSpeed { get; private set; } = 1f;

        [field: SerializeField] public float AttackRadius { get; private set; } = 7f;
        [field: SerializeField] public float ArrowSpeed { get; private set; } = 8f;
        [field: SerializeField] public int Damage { get; private set; } = 10;
    }
}