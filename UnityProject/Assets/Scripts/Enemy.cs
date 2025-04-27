using UnityEngine;
using System.Collections; // Required for IEnumerator

public class Enemy : MonoBehaviour
{
    public EnemyGun enemyGun; // Reference to the EnemyGun component
    public RandomWalker randomWalker; // Reference to the RandomWalker component
    public EnemyGrabController enemyGrabController; // Reference to the EnemyGrabController component

    public GameObject deathEffectPrefab; // Optional: assign a prefab for death effect in the inspector
    public GameObject bloodEffectPrefab; // Optional: assign a prefab for blood effect in the inspector
    public GameObject stunnedEffect;

    public bool hasBomb; // Flag to indicate if the enemy has a bomb
    public GameObject bombExplosionPrefab; // Optional: assign a prefab for bomb explosion effect in the inspector

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

        if (hasBomb)
        {
            if (bombExplosionPrefab != null)
            {
                GameObject bombExplosion = Instantiate(bombExplosionPrefab, transform.position, Quaternion.identity);
                Destroy(bombExplosion, 2f); // Destroy the explosion effect after 2 seconds
            }
            // Check if hand is nearby and apply damage
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f); // Adjust radius as needed
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Hand")) // Assuming the hand has a tag "Hand"
                {
                    Hand hand = collider.GetComponent<Hand>();
                    if (hand != null)
                    {
                        hand.TakeDamage(25); // Adjust damage value as needed
                    }
                }

                if (collider.CompareTag("Head")) // Assuming the head has a tag "Head"
                {
                    Head head = collider.GetComponent<Head>();
                    if (head != null)
                    {
                        head.TakeDamage(60); // Adjust damage value as needed
                    }
                }
            }
        }

        // Find the GameUIManager and call the EnemyDied method
        GameUIManager gameUIManager = FindObjectOfType<GameUIManager>();
        if (gameUIManager != null)
        {
            int scoreIncrement = 10; // Default score increment
            if (hasBomb)
            {
                scoreIncrement += 20;
            }
            if (enemyGun != null)
            {
                scoreIncrement += 50;
            }
            gameUIManager.EnemyDied(scoreIncrement); // Call the method to update the UI
        }

        Destroy(gameObject);
    }

    public void Stun()
    {
        // Disable the EnemyGun and RandomWalker components
        if (enemyGun != null)
        {
            enemyGun.enabled = false;
        }

        if (enemyGrabController != null)
        {
            enemyGrabController.stunned = true; // Set stunned state in EnemyGrabController
        }

        if (randomWalker != null)
        {
            randomWalker.enabled = false;
        }

        // Optionally, you can add a visual effect or change the enemy's color to indicate it's stunned
        stunnedEffect.SetActive(true); // Activate the stunned effect
        StartCoroutine(EndStun());
    }

    private IEnumerator EndStun()
    {
        yield return new WaitForSeconds(2f); // Stun duration

        // Re-enable the EnemyGun and RandomWalker components
        if (enemyGun != null)
        {
            enemyGun.enabled = true;
        }

        if (enemyGrabController != null)
        {
            enemyGrabController.stunned = false; // Reset stunned state in EnemyGrabController
        }

        if (randomWalker != null)
        {
            randomWalker.enabled = true;
        }
        stunnedEffect.SetActive(false); // Deactivate the stunned effect
    }
}
