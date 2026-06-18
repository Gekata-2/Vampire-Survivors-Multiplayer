using _Project.Scripts.UI;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerStateView : MonoBehaviour, IPlayerView
    {
        [SerializeField] private ProgressBarText _health;
        [SerializeField] private LevelProgressBar _xp;
        [SerializeField] private TMP_Text _nickname;
        [SerializeField] private ProgressBar _reloadBar;
        [SerializeField] private TMP_Text _shotsFired;

        public void EnableLocalMode()
        {
            _health.gameObject.SetActive(false);
            _xp.gameObject.SetActive(false);
        }

        public void SetNickname(string nickname)
            => _nickname.text = nickname;

        public void SetHealth(float value)
            => _health.SetCurrentValue(value);

        public void SetMaxHealth(float value)
            => _health.SetMaxValue(value);

        public void SetLevel(int lvl)
            => _xp.SetLevel(lvl);

        public void SetExperienceNormalized(float value)
            => _xp.SetProgress(value);

        public void SetShotsFired(int value) 
            => _shotsFired.text = $"{value}";

        public void SetReloadProgress(float progress) 
            => _reloadBar.SetProgress(progress);
    }
}