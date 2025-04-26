using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints; // Set at least 2 points in Inspector
    public float spawnInterval = 5f;

    private float timer = 0f;

    void Start()
    {
        FindFirstObjectByType<Head>().OnDeath += () => enabled = false; // Disable spawner on head death
        timer = spawnInterval; // Initialize the timer
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnTwoEnemies();
            timer = spawnInterval;
        }
    }

    void SpawnTwoEnemies()
    {
        if (spawnPoints.Length < 1) return;

        // Pick two different random spawn points
        int index = Random.Range(0, spawnPoints.Length);

        GameObject enemy = Instantiate(enemyPrefab, spawnPoints[index].position, Quaternion.identity) as GameObject;
        EnemyGun enemyGun = enemy.GetComponentInChildren<EnemyGun>();
        if (enemyGun != null)
        {
            enemyGun.target = GameObject.FindGameObjectWithTag("Head").transform;
        }
    }
}
