using UnityEngine.SceneManagement;

namespace _Project.Scripts.Services
{
    public static class ScenesRepository
    {
        public static int GameplaySceneIndex =>
            SceneUtility.GetBuildIndexByScenePath("Assets/_Project/Scenes/Gameplay.unity");
        public static int MainMenuSceneIndex =>
            SceneUtility.GetBuildIndexByScenePath("Assets/_Project/Scenes/Main Menu.unity");
    }
}