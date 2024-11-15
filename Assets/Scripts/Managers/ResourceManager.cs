using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameTypes;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    
    [Header("Prefabs")]
    public GameObject[] enemyPrefabs;
    public GameObject[] weaponPrefabs;
    public GameObject[] pickupPrefabs;
    public GameObject[] effectPrefabs;
    
    public void SpawnEnemy(EnemyType type, Vector2 position)
    {
        // Handle enemy spawning
    }
    
    public void SpawnPickup(PickupType type, Vector2 position)
    {
        // Handle pickup spawning
    }
}