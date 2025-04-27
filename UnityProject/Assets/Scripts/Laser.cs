using UnityEngine;

public class Laser : MonoBehaviour
{
    public float rotationSpeed = 45f;    // Degrees per second
    public float maxRotationAngle = 45f; // How far left and right
    public float rayDistance = 100f;      // How far the laser checks
    public GameObject hitEffectPrefab;
    public LayerMask detectionLayer; // Layer to check for collisions

    private float currentAngle = 0f;
    private bool rotatingRight = true;
    private Vector3 initialRotation;

    void Start()
    {
        initialRotation = transform.eulerAngles;
    }

    void Update()
    {
        RotateLaser();
        CheckCollision();
    }

    void RotateLaser()
    {
        float deltaAngle = rotationSpeed * Time.deltaTime;
        currentAngle += rotatingRight ? deltaAngle : -deltaAngle;

        if (Mathf.Abs(currentAngle) >= maxRotationAngle)
        {
            rotatingRight = !rotatingRight;
            currentAngle = Mathf.Clamp(currentAngle, -maxRotationAngle, maxRotationAngle);
        }

        transform.rotation = Quaternion.Euler(0, 0, initialRotation.z + currentAngle);
    }

    void CheckCollision()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, -transform.up, rayDistance, detectionLayer);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                // Check if the hit object has a "Enemy" tag
                if (hit.collider.CompareTag("Enemy"))
                {
                    // Apply damage or any other effect to the enemy
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.Die(); // Adjust damage value as needed
                    }

                    // Spawn hit effect at collision point
                    if (hitEffectPrefab != null)
                    {
                        GameObject effect = Instantiate(hitEffectPrefab, hit.point, Quaternion.identity);
                        Destroy(effect.gameObject, 2f); // Destroy particle after 1 second
                    }
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Just for editor to see the laser ray
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -transform.up * rayDistance);
    }
}
