using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public Transform target;         // The player or boss
    public Transform firePoint;      // Where the bullet spawns
    public GameObject bulletPrefab;
    public float fireInterval = 2f;  // Seconds between shots
    public float bulletSpeed = 10f;
    public AudioClip fireSound;        // Sound to play when firing
    public AudioSource audioSource;    // Audio source to play the sound

    private float fireTimer = 0f;
    private bool headDied = false;

    public void Start()
    {
        // Nice code bro
        if (target == null)
        {
            target = FindFirstObjectByType<Head>().transform; // Find the player if not set
        }

        if (target.GetComponent<Head>())
        {
            target.GetComponent<Head>().OnDeath += () => headDied = true;
        }
        fireTimer = fireInterval; // Initialize the timer
    }

    void Update()
    {
        if (target == null || headDied) return;

        // Rotate to face the target
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Fire at interval
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Fire();
            fireTimer = fireInterval;
        }
    }

    void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = firePoint.right * bulletSpeed;

        // Play sound effect
        if (audioSource != null && fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
    }
}
