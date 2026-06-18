using System.Collections.Generic;

namespace _Project.Scripts.Enemies
{
    public class EnemyRegistry
    {
        private readonly List<Enemy> _enemies = new();

        public List<Enemy> Enemies => new(_enemies);

        public void Add(Enemy enemy)
        {
            if (!_enemies.Contains(enemy))
                _enemies.Add(enemy);
        }

        public void Remove(Enemy enemy)
        {
            if (_enemies.Contains(enemy))
                _enemies.Remove(enemy);
        }

        public void Clear()
            => _enemies.Clear();
    }
}