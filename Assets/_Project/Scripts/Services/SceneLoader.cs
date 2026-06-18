using UnityEngine.SceneManagement;

namespace _Project.Scripts.Services
{
    public class SceneLoader
    {
        public void LoadMainMenu()
        {
            SceneManager.LoadScene(ScenesRepository.MainMenuSceneIndex);
        }
    }
}