using UnityEngine;
using GameTypes;
using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public EnemyType[] enemyTypes;
        public float spawnInterval = 2f;
        public int baseEnemiesPerSpawn = 2;
        public float waveDuration = 30f;
        public bool includesBoss = false;
        [Range(1f, 2f)] public float difficultyMultiplier = 1.2f;
    }

    [Header("Wave Settings")]
    [SerializeField] private Wave[] waves;
    //[SerializeField] private float gameplayArea = 20f;
    [SerializeField] private Transform player;
    [SerializeField] private float minSpawnDistance = 15f;
    [SerializeField] private float maxSpawnDistance = 25f;
    
    [Header("Difficulty Settings")]
    [SerializeField] private float globalDifficultyMultiplier = 1f;
    [SerializeField] private float difficultyIncreaseRate = 0.1f;
    [SerializeField] private int enemiesPerWaveIncrease = 1;
    
    [Header("Special Spawn Settings")]
    [SerializeField] private float bossWaveInterval = 5;
    [SerializeField] private float specialEnemyChance = 0.1f;

    private float waveTimer;
    private float spawnTimer;
    private int currentWave;
    private int totalEnemiesSpawned;
    private EnemyFactory enemyFactory;
    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Start()
    {
        enemyFactory = GetComponent<EnemyFactory>();
        if (enemyFactory == null)
        {
            enemyFactory = gameObject.AddComponent<EnemyFactory>();
        }
        
        InitializeWave();
    }

    private void InitializeWave()
    {
        waveTimer = 0;
        spawnTimer = 0;
        UpdateDifficulty();
    }

    private void Update()
    {
        if (currentWave >= waves.Length) return;
        
        Wave wave = waves[currentWave];
        spawnTimer += Time.deltaTime;
        waveTimer += Time.deltaTime;
        
        // Clean up destroyed enemies from the list
        activeEnemies.RemoveAll(enemy => enemy == null);
        
        if (spawnTimer >= GetAdjustedSpawnInterval(wave))
        {
            SpawnEnemies(wave);
            spawnTimer = 0;
        }
        
        if (waveTimer >= wave.waveDuration)
        {
            AdvanceWave();
        }
    }

    private void AdvanceWave()
    {
        currentWave++;
        waveTimer = 0;
        spawnTimer = 0;
        
        if (currentWave < waves.Length)
        {
            UpdateDifficulty();
            
            // Spawn boss every few waves
            if (currentWave % bossWaveInterval == 0)
            {
                SpawnBoss();
            }
        }
    }

    private void UpdateDifficulty()
    {
        globalDifficultyMultiplier += difficultyIncreaseRate;
    }

    private float GetAdjustedSpawnInterval(Wave wave)
    {
        return wave.spawnInterval / Mathf.Sqrt(globalDifficultyMultiplier);
    }

    private void SpawnEnemies(Wave wave)
    {
        int enemiesToSpawn = CalculateEnemiesPerSpawn(wave);
        
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            EnemyType enemyType = SelectEnemyType(wave);
            Vector2 spawnPosition = GetRandomSpawnPosition();
            
            GameObject enemy = enemyFactory.CreateEnemy(enemyType, spawnPosition);
            if (enemy != null)
            {
                activeEnemies.Add(enemy);
                totalEnemiesSpawned++;
            }
        }
    }

    private int CalculateEnemiesPerSpawn(Wave wave)
    {
        float baseAmount = wave.baseEnemiesPerSpawn + (currentWave * enemiesPerWaveIncrease);
        return Mathf.RoundToInt(baseAmount * globalDifficultyMultiplier);
    }

    private EnemyType SelectEnemyType(Wave wave)
    {
        // Chance to spawn special enemy
        if (Random.value < specialEnemyChance * globalDifficultyMultiplier)
        {
            return EnemyType.Boss;
        }
        
        return wave.enemyTypes[Random.Range(0, wave.enemyTypes.Length)];
    }

    private void SpawnBoss()
    {
        Vector2 spawnPosition = GetRandomSpawnPosition();
        GameObject boss = enemyFactory.CreateEnemy(EnemyType.Boss, spawnPosition);
        if (boss != null)
        {
            activeEnemies.Add(boss);
            // Optionally scale boss stats based on wave number
            if (boss.TryGetComponent<EnemyBase>(out var bossEnemy))
            {
                // Scale boss stats here if needed
            }
        }
    }

    private Vector2 GetRandomSpawnPosition()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
        
        Vector2 spawnOffset = new Vector2(
            Mathf.Cos(angle) * distance,
            Mathf.Sin(angle) * distance
        );
        
        return (Vector2)player.position + spawnOffset;
    }

    // Helper method to get current wave info
    public (int wave, int enemies) GetWaveStatus()
    {
        return (currentWave + 1, activeEnemies.Count);
    }
}