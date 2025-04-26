using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float lifetime = 5f;
    public int damage = 10;

    void Start()
    {
        Destroy(gameObject, lifetime);
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
