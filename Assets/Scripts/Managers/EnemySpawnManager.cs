using System.Collections;
using System.Collections.Generic;
using GameTypes;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public EnemyType[] enemyTypes;
        public float spawnInterval;
        public int enemiesPerSpawn;
        public float waveDuration;
    }
    
    [SerializeField] private Wave[] waves;
    [SerializeField] private float gameplayArea = 20f;
    [SerializeField] private Transform player;
    
    private float waveTimer;
    private float spawnTimer;
    private int currentWave;
    
    private void Update()
    {
        if (currentWave >= waves.Length) return;
        
        Wave wave = waves[currentWave];
        spawnTimer += Time.deltaTime;
        waveTimer += Time.deltaTime;
        
        if (spawnTimer >= wave.spawnInterval)
        {
            SpawnEnemies(wave);
            spawnTimer = 0;
        }
        
        if (waveTimer >= wave.waveDuration)
        {
            currentWave++;
            waveTimer = 0;
            spawnTimer = 0;
        }
    }
    
    private void SpawnEnemies(Wave wave)
    {
        for (int i = 0; i < wave.enemiesPerSpawn; i++)
        {
            EnemyType enemyType = wave.enemyTypes[Random.Range(0, wave.enemyTypes.Length)];
            Vector2 spawnPosition = GetRandomSpawnPosition();
            
            ResourceManager.Instance.SpawnEnemy(enemyType, spawnPosition);
        }
    }
    
    private Vector2 GetRandomSpawnPosition()
    {
        // Spawn enemies outside the screen but within gameplay area
        float angle = Random.Range(0f, 360f);
        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;
        return (Vector2)player.position + direction * gameplayArea;
    }
}
