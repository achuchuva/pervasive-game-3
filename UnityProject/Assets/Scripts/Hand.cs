using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    public enum HandType
    {
        Left,
        Right
    }
    public HandType handType;
    public float x;
    public float y;
    public bool fist;

    public SpriteRenderer _spriteRenderer;
    public Sprite _openHandSprite;
    public Sprite _closedHandSprite;

    public bool grabbing;

    public float slamVelocityThreshold = 10f; // Tune this for how "hard" the slam needs to be
    public LayerMask terrainLayer;            // Set in Inspector: Layer that represents the stage/terrain
    public float slamCheckDistance = 0.5f;     // How far below to check for stage collision
    public GameObject slamEffectPrefab;        // Prefab for the slam effect (e.g., particles, camera shake)

    private Vector2 lastPosition;
    private Vector2 velocity;

    public float grabRadius = 2f;
    public LayerMask grabbableLayer; // Layer for objects you can grab

    public AudioSource audioSource;
    public AudioClip slamSound;

    private Enemy grabbedObject = null;
    public Head head; // Reference to the head script

    private float slamCooldown = 1f; // 1 second cooldown
    private float lastSlamTime = -999f; // Time when last slam happened

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Smoothly move the hand to the new position
        Vector2 targetPosition = new Vector2(x, y);
        Vector2 currentPosition = transform.position;
        Vector2 newPosition = Vector2.Lerp(currentPosition, targetPosition, Time.deltaTime * 10f); // Adjust the speed as needed
        transform.position = newPosition;

        if (fist)
        {
            _spriteRenderer.sprite = _closedHandSprite;
        }
        else
        {
            _spriteRenderer.sprite = _openHandSprite;
        }

        // Track velocity manually
        currentPosition = transform.position;
        velocity = (currentPosition - lastPosition) / Time.deltaTime;
        lastPosition = currentPosition;

        DetectSlam();

        if (fist)
        {
            if (grabbedObject == null)
            {
                TryGrab();
            }
            else
            {
                HoldGrabbedObject();
            }
        }
        else
        {
            Release();
        }
    }

    void DetectSlam()
    {
        if (velocity.y < -slamVelocityThreshold && fist) // Must be moving down fast
        {
            // Check cooldown
            if (Time.time - lastSlamTime < slamCooldown) 
                return; // Not enough time passed

            // Raycast downward to check if near the stage
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, slamCheckDistance, terrainLayer);
            if (hit.collider != null)
            {
                SlamEffect(hit.point);
                lastSlamTime = Time.time; // Update the last slam time
            }
        }
    }

    void SlamEffect(Vector2 slamPosition)
    {
        // TODO: spawn particles, camera shake, sound effects, etc.
        if (slamEffectPrefab != null)
        {
            GameObject effect = Instantiate(slamEffectPrefab, slamPosition, Quaternion.identity);
            Destroy(effect, 3f); // Destroy the effect after 3 seconds
        }

        FindFirstObjectByType<CameraShake>().Shake(); // uses default duration/magnitude

        if (audioSource != null && slamSound != null)
        {
            audioSource.PlayOneShot(slamSound); // Play slam sound
        }

        // Get all the enemies in the scene
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.Stun();
            // Check if the enemy is within a certain distance from the slam position
            if (Vector2.Distance(enemy.transform.position, slamPosition) < 2f) // Adjust distance as needed
            {
                enemy.Die(); // Call the Die method on the enemy
            }
        }
    }

    void TryGrab()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, grabRadius, grabbableLayer);
        if (hits.Length > 0)
        {
            grabbedObject = hits[0].GetComponent<Enemy>();
            grabbedObject.enemyGrabController.grabbedBy = transform;
        }
    }

    void HoldGrabbedObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.transform.position = transform.position;
            grabbedObject.enemyGrabController.grabbedBy = transform;
        }
    }

    public void TakeDamage(int damage)
    {
        // Handle damage to the hand here
        // For example, you can reduce health or play a sound effect
        head.TakeDamage(damage); // Assuming you have a reference to the head
    }

    void Release()
    {
        if (grabbedObject != null)
        {
            grabbedObject.enemyGrabController.grabbedBy = null;
            grabbedObject = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize grab range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, grabRadius);
    }
}
