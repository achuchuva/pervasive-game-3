using UnityEngine;
using System.Collections;

public class TutorialEnemySpawner : MonoBehaviour
{
    public EnemySpawner normalEnemySpawner; // Reference to your normal spawner
    public PowerupSpawner powerupSpawner; // Reference to your powerup spawner (if needed)
    public GameObject[] tutorialEnemies;    // Enemies to spawn during tutorial
    public Transform[] spawnPoints;         // Spawn points
    public float spawnDelay = 5f;            // Time between tutorial spawns
    public float initialDelay = 2f;           // Initial delay before starting tutorial spawns

    public AudioSource audioSource;
    public AudioClip spawnSound; // Sound to play when spawning enemies

    private void Awake()
    {
        if (normalEnemySpawner != null)
        {
            normalEnemySpawner.enabled = false; // Disable normal spawner at start
        }

        if (powerupSpawner != null)
        {
            powerupSpawner.enabled = false; // Disable powerup spawner at start
        }

        StartCoroutine(SpawnTutorialEnemies());
    }

    void Update()
    {
        // If input is space skip tutorial
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OverrideTutorial();
        }
    }

    private IEnumerator SpawnTutorialEnemies()
    {
        yield return new WaitForSeconds(initialDelay); // Wait for initial delay

        foreach (GameObject enemyPrefab in tutorialEnemies)
        {
            if (spawnPoints.Length == 0) yield break;

            // Play sound effect
            if (audioSource != null && spawnSound != null)
            {
                audioSource.PlayOneShot(spawnSound);
            }
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);

            yield return new WaitForSeconds(spawnDelay);
        }

        // After tutorial enemies spawned, enable normal spawner
        if (normalEnemySpawner != null)
        {
            normalEnemySpawner.enabled = true;
        }

        if (powerupSpawner != null)
        {
            powerupSpawner.enabled = true; // Enable powerup spawner if needed
        }

        // Optionally destroy this script if you don't need it anymore
        Destroy(this);
    }

    public void OverrideTutorial()
    {
        // This method can be called to skip the tutorial and enable the normal spawner immediately
        if (normalEnemySpawner != null)
        {
            normalEnemySpawner.enabled = true;
        }

        if (powerupSpawner != null)
        {
            powerupSpawner.enabled = true; // Enable powerup spawner if needed
        }

        TutorialTipUIManager manager = FindFirstObjectByType<TutorialTipUIManager>();

        if (manager != null)
        {
            manager.OverrideTutorial(); // Call the method to skip tutorial tips
        }

        // Optionally destroy this script if you don't need it anymore
        Destroy(this);
    }
}
