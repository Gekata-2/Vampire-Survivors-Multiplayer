using System;
using Fusion;
using Zenject;

namespace _Project.Scripts.Player
{
    public class Experience : NetworkBehaviour
    {
        public event Action<int> LevelChanged;
        public event Action<int> ExperienceChanged;

        private LevelingSystem _levelingSystem;

        [Networked, OnChangedRender(nameof(OnValueChanged))]
        private int CurrentExp { get; set; }

        [Networked, OnChangedRender(nameof(OnLevelChanged))]
        public int Level { get; private set; }

        [Networked] private int CurrentLevelThreshold { get; set; }
        [Networked] private int NextLevelThreshold { get; set; }

        public float LevelProgressNormalized =>
            (float)(CurrentExp - CurrentLevelThreshold) / (NextLevelThreshold - CurrentLevelThreshold);

        [Inject]
        private void Construct(LevelingSystem levelingSystem)
        {
            _levelingSystem = levelingSystem;
        }

        public void Initialize(int current, int level, int nextLevelThreshold)
        {
            NextLevelThreshold = nextLevelThreshold;
            CurrentExp = current;
            Level = level;
            LevelChanged?.Invoke(Level);
            ExperienceChanged?.Invoke(CurrentExp);
        }

        public void AddExperience(int value)
        {
            CurrentExp += value;
            while (CurrentExp >= NextLevelThreshold) 
                LevelUp(1);
        }

        private void LevelUp(int levels)
        {
            Level += levels;
            CurrentLevelThreshold = _levelingSystem.GetExperienceForLevel(Level);
            NextLevelThreshold = _levelingSystem.GetExperienceForLevel(Level + 1);
        }

        private void OnValueChanged()
            => ExperienceChanged?.Invoke(CurrentExp);

        private void OnLevelChanged()
            => LevelChanged?.Invoke(Level);
    }
}