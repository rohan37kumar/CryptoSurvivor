using UnityEngine;
using GameTypes;

public class EnemyFactory : MonoBehaviour
{
    [System.Serializable]
    public class EnemyPrefabMapping
    {
        public EnemyType type;
        public GameObject prefab;
    }

    [SerializeField] private EnemyPrefabMapping[] enemyPrefabs;

    public GameObject CreateEnemy(EnemyType type, Vector2 position)
    {
        EnemyPrefabMapping mapping = System.Array.Find(enemyPrefabs, m => m.type == type);
        if (mapping != null)
        {
            return Instantiate(mapping.prefab, position, Quaternion.identity);
        }
        Debug.LogWarning($"No prefab found for enemy type: {type}");
        return null;
    }
}