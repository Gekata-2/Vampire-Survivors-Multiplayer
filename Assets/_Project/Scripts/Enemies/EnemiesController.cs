using _Project.Scripts.EnemyDrop;
using _Project.Scripts.Player;
using Fusion;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Enemies
{
    public class EnemiesController : NetworkBehaviour
    {
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private float _spawnRadius = 10f;
        [SerializeField] private float _spawnCooldown = 2f;
        [SerializeField] private float _startDelay = 2f;

        private Player.Player _host;
        private TickTimer _spawnTimer;


        private LootDropSystem _lootDropSystem;
        private EnemyConfig _enemyConfig;
        private EnemyRegistry _registry;
        private PlayerRegistry _playerRegistry;


        [Inject]
        private void Construct(LootDropSystem dropSystem,
            EnemyConfig enemyConfig,
            EnemyRegistry enemyRegistry,
            PlayerRegistry playerRegistry)
        {
            _lootDropSystem = dropSystem;
            _enemyConfig = enemyConfig;
            _registry = enemyRegistry;
            _playerRegistry = playerRegistry;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            foreach (Enemy enemy in _registry.Enemies)
                enemy.Died -= OnEnemyDied;

            _registry.Clear();
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority)
                return;

            if (_playerRegistry.Host == null)
                return;
            if (_host == null || _host != _playerRegistry.Host)
                SetHost(_playerRegistry.Host);

            if (_spawnTimer.Expired(Runner))
            {
                SpawnEnemy();
                _spawnTimer = CreateTimer();
            }
        }

        private TickTimer CreateTimer()
            => TickTimer.CreateFromSeconds(Runner, _spawnCooldown);

        private void SpawnEnemy()
        {
            Vector3 position = GetSpawnPosition();
            Enemy enemy = Runner.Spawn(_enemyPrefab, position, Quaternion.identity,
                onBeforeSpawned: (_, e) => e.GetComponent<Enemy>().Initialize(_lootDropSystem, _enemyConfig));
            enemy.Died += OnEnemyDied;
            _registry.Add(enemy);
        }

        private void OnEnemyDied(Enemy enemy)
        {
            enemy.Died -= OnEnemyDied;
            _registry.Remove(enemy);
        }

        private Vector3 GetSpawnPosition()
            => (Vector3)Random.insideUnitCircle.normalized * _spawnRadius + _host.transform.position;

        private void SetHost(Player.Player player)
        {
            _host = player;
            _spawnTimer = TickTimer.CreateFromSeconds(Runner, _startDelay);
        }
    }
}