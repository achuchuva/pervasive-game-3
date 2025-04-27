using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject prefab;
        public float spawnWeight = 1f; // Higher = more likely to spawn
    }

    public List<EnemyType> enemies; // List of enemy types
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;
    public int enemiesPerSpawn = 1; // Start with 1 enemy, increase over time
    public float increaseInterval = 20f; // Every 20 seconds, increase enemiesPerSpawn by 1

    private float timer = 0f;
    private float difficultyTimer = 0f;

    void Start()
    {
        FindFirstObjectByType<Head>().OnDeath += () => enabled = false;
        timer = spawnInterval;
        difficultyTimer = increaseInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        difficultyTimer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnEnemies();
            timer = spawnInterval;
        }

        if (difficultyTimer <= 0f)
        {
            enemiesPerSpawn++;
            difficultyTimer = increaseInterval;
        }
    }

    void SpawnEnemies()
    {
        if (spawnPoints.Length < 1 || enemies.Count < 1) return;

        for (int i = 0; i < enemiesPerSpawn; i++)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            GameObject prefabToSpawn = PickRandomEnemy();

            GameObject enemy = Instantiate(prefabToSpawn, spawnPoints[spawnIndex].position, Quaternion.identity);
            EnemyGun enemyGun = enemy.GetComponentInChildren<EnemyGun>();
            if (enemyGun != null)
            {
                enemyGun.target = GameObject.FindGameObjectWithTag("Head").transform;
            }
        }
    }

    GameObject PickRandomEnemy()
    {
        float totalWeight = 0f;
        foreach (var enemy in enemies)
        {
            totalWeight += enemy.spawnWeight;
        }

        float randomValue = Random.Range(0, totalWeight);
        float currentWeight = 0f;

        foreach (var enemy in enemies)
        {
            currentWeight += enemy.spawnWeight;
            if (randomValue <= currentWeight)
            {
                return enemy.prefab;
            }
        }

        return enemies[0].prefab; // Fallback
    }
}
