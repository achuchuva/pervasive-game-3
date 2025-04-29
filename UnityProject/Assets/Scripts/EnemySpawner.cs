using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject prefab;
        public float spawnWeight = 1f;         // Starting spawn weight
        public float weightIncreasePerTick = 0f; // How much this enemy's weight grows over time
    }

    public List<EnemyType> enemies;
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;
    public int enemiesPerSpawn = 1;
    public float increaseInterval = 20f; // Time to increase enemiesPerSpawn
    public float weightIncreaseInterval = 10f; // Time to increase enemy weights

    public AudioSource audioSource;
    public AudioClip spawnSound; // Sound to play when spawning enemies

    private float spawnTimer = 0f;
    private float difficultyTimer = 0f;
    private float weightTimer = 0f;

    void Start()
    {
        FindFirstObjectByType<Head>().OnDeath += () => enabled = false;
        spawnTimer = spawnInterval;
        difficultyTimer = increaseInterval;
        weightTimer = weightIncreaseInterval;
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        difficultyTimer -= Time.deltaTime;
        weightTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemies();
            spawnTimer = spawnInterval;
        }

        if (difficultyTimer <= 0f)
        {
            enemiesPerSpawn++;
            difficultyTimer = increaseInterval;
        }

        if (weightTimer <= 0f)
        {
            IncreaseEnemyWeights();
            weightTimer = weightIncreaseInterval;
        }
    }

    void SpawnEnemies()
    {
        if (audioSource != null && spawnSound != null)
        {
            audioSource.PlayOneShot(spawnSound); // Play spawn sound
        }

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

    void IncreaseEnemyWeights()
    {
        foreach (var enemy in enemies)
        {
            enemy.spawnWeight += enemy.weightIncreasePerTick;
        }
    }
}
