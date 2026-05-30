using Unity.Cinemachine;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class CameraFollowHandler : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _camera;

        public void SetTarget(Transform target)
        {
            _camera.Follow = target;
        }
    }
}