using _Project.Scripts.Services;

namespace _Project.Scripts.UI.QuickMenu
{
    public class QuickMenuModel
    {
        private readonly RunnerProvider _runnerProvider;
        
        public QuickMenuModel(RunnerProvider runnerProvider)
        {
            _runnerProvider = runnerProvider;
        }

        public void QuitGame()
        {
            _runnerProvider.Runner.Shutdown();
        }
    }
}