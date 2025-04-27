using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float lifetime = 5f;
    public int damage = 10;

    private Rigidbody2D rb;
    void Start()
    {
        Destroy(gameObject, lifetime);
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 velocity = rb.linearVelocity;
        if (velocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Head"))
        {
            // Damage player head here
            Head head = other.GetComponent<Head>();
            if (head != null)
            {
                head.TakeDamage(damage); // Adjust damage value as needed
            }
            Destroy(gameObject);
        }
    }
}
