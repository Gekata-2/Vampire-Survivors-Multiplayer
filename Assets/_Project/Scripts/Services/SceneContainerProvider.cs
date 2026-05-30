using Zenject;

namespace _Project.Scripts.Services
{
    public class SceneContainerProvider
    {
        public DiContainer Container { get; private set; }

        public void SetContainer(DiContainer container) 
            => Container = container;
    }
}