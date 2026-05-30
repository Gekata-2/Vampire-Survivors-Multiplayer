using System;
using Fusion;

namespace _Project.Scripts.Systems
{
    public class Health : NetworkBehaviour
    {
        public event Action ZeroReached;
        public event Action<int> HealthChanged;
        public event Action<int> MaxChanged;

        [Networked, OnChangedRender(nameof(OnMaxChanged))]
        public int MaxHealth { get; private set; }

        [Networked, OnChangedRender(nameof(OnCurrentChanged))]
        public int Current { get; private set; }

        public void Initialize(int max, int current)
        {
            MaxHealth = max;
            Current = current;
        }

        public void Modify(int delta)
        {
            if (Current + delta <= 0)
            {
                Current = 0;
                ZeroReached?.Invoke();
            }
            else if (Current + delta > MaxHealth)
            {
                Current = MaxHealth;
            }
            else
            {
                Current += delta;
            }
        }

        public void HealFull() 
            => Current = MaxHealth;

        public void SetMaxHealth(int value) 
            => MaxHealth = value;

        private void OnMaxChanged() 
            => MaxChanged?.Invoke(MaxHealth);

        private void OnCurrentChanged() 
            => HealthChanged?.Invoke(Current);
    }
}