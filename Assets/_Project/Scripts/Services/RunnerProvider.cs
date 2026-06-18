using Fusion;

namespace _Project.Scripts.Services
{
    public class RunnerProvider
    {
        public NetworkRunner Runner { get; private set; }

        public void SetRunner(NetworkRunner runner) 
            => Runner = runner;
    }
}