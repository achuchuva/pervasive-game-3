using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupSpawner : MonoBehaviour
{
    public List<GameObject> powerupPrefabs; // List of powerup prefabs
    public Transform player;                // Reference to the player
    public Vector2 spawnAreaMin;             // Bottom-left corner of spawn region
    public Vector2 spawnAreaMax;             // Top-right corner of spawn region
    public float spawnInterval = 5f;         // Time between spawns
    public float minDistanceFromPlayer = 2f; // Minimum distance from player

    void Start()
    {
        StartCoroutine(SpawnPowerups());
    }

    IEnumerator SpawnPowerups()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnPowerup();
        }
    }

    void SpawnPowerup()
    {
        if (powerupPrefabs.Count == 0) return;

        Vector2 spawnPos;
        int tries = 0;

        do
        {
            float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            spawnPos = new Vector2(x, y);

            tries++;
            if (tries > 10) break; // Safety limit to avoid infinite loop
        }
        while (Vector2.Distance(spawnPos, player.position) < minDistanceFromPlayer);

        GameObject prefab = powerupPrefabs[Random.Range(0, powerupPrefabs.Count)];
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
