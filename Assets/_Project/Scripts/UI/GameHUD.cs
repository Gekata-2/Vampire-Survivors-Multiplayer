using _Project.Scripts.Player;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class GameHUD : MonoBehaviour, IPlayerView
    {
        [SerializeField] private TMP_Text _roomName;
        [SerializeField] private TMP_Text _peerStatus;
        [SerializeField] private ProgressBarText _health;
        [SerializeField] private LevelProgressBar _xp;
        
        public void SetRoomName(string roomName)
            => _roomName.text = roomName;

        public void SetPeerStatus(string status)
            => _peerStatus.text = status;

        public void SetHealth(float value)
            => _health.SetCurrentValue(value);

        public void SetMaxHealth(float value)
            => _health.SetMaxValue(value);

        public void SetLevel(int lvl)
            => _xp.SetLevel(lvl);

        public void SetExperienceNormalized(float value)
            => _xp.SetProgress(value);
    }
}