using Fusion;
using UnityEngine;

namespace _Project.Scripts.Network
{
    public class ClientSimulationHandler : NetworkBehaviour
    {
        [SerializeField] private bool _isSimulated = true;

        public override void Spawned()
        {
            if (HasStateAuthority)
                return;

            Runner.SetIsSimulated(Object, _isSimulated);
        }
    }
}