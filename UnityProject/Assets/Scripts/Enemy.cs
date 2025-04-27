using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyGun enemyGun; // Reference to the EnemyGun component
    public RandomWalker randomWalker; // Reference to the RandomWalker component
    public EnemyGrabController enemyGrabController; // Reference to the EnemyGrabController component

    public GameObject deathEffectPrefab; // Optional: assign a prefab for death effect in the inspector
    public GameObject bloodEffectPrefab; // Optional: assign a prefab for blood effect in the inspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Die()
    {
        // Optional: play effect here
        if (deathEffectPrefab != null)
        {
            GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(deathEffect, 2f); // Destroy the effect after 2 seconds
        }

        if (bloodEffectPrefab != null)
        {
            // Create random rotation for the blood effect
            Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            GameObject bloodEffect = Instantiate(bloodEffectPrefab, transform.position, randomRotation);
            Destroy(bloodEffect, 4f); // Destroy the effect after 4 seconds
        }

        Destroy(gameObject);
    }
}
