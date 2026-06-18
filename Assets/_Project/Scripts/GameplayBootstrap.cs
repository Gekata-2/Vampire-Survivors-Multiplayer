using _Project.Scripts.UI;
using Fusion;
using Zenject;

namespace _Project.Scripts
{
    public class GameplayBootstrap : NetworkBehaviour
    {
        private GameHUD _gameHUD;

        [Inject]
        private void Construct(GameHUD hud)
        {
            _gameHUD = hud;
        }

        public override void Spawned()
        {
            _gameHUD.SetRoomName(Runner.SessionInfo.Name);
            _gameHUD.SetPeerStatus(Runner.IsServer ? "Server" : "Client");
        }
    }
}