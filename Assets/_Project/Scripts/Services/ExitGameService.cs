using UnityEngine;

namespace _Project.Scripts.Services
{
    public class ExitGameService
    {
        public void PerformExit()
        {
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}