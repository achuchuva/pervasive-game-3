using UnityEngine;

public class EnemyGrabController : MonoBehaviour
{
    public float grabDuration = 2f;
    private float grabTimer = 0f;
    private bool isGrabbed = false;

    private Transform grabbedBy;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private RandomWalker walker;
    private EnemyGun enemyGun;
    private Hand hand;

    void Start()
    {
        walker = GetComponent<RandomWalker>();
        enemyGun = GetComponentInChildren<EnemyGun>();
    }

    void Update()
    {
        if (isGrabbed && grabbedBy != null)
        {
            if (hand == null || !hand.fist)
            {
                isGrabbed = false;
                grabbedBy = null;
                spriteRenderer.color = Color.white;

                if (walker != null)
                {
                    walker.enabled = true;
                }

                if (enemyGun != null)
                {
                    enemyGun.enabled = true;
                }
                return;
            }

            // Follow the hand
            transform.position = grabbedBy.position;

            // Increase redness over time
            grabTimer += Time.deltaTime;
            float t = Mathf.Clamp01(grabTimer / grabDuration);
            spriteRenderer.color = Color.Lerp(Color.white, Color.red, t);

            if (grabTimer >= grabDuration)
            {
                Die();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        CheckGrabbed(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        CheckGrabbed(other);
    }

    void CheckGrabbed(Collider2D other)
    {
        if (isGrabbed) return;

        if (other.CompareTag("Hand"))
        {
            hand = other.GetComponent<Hand>();
            if (hand != null && hand.fist)
            {
                grabbedBy = other.transform;
                isGrabbed = true;
                grabTimer = 0f;

                if (walker != null)
                {
                    walker.enabled = false;
                }

                if (enemyGun != null)
                {
                    enemyGun.enabled = false;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Hand"))
        {
            hand = other.GetComponent<Hand>();
            if (hand != null && hand.fist && isGrabbed)
            {
                grabbedBy = null;
                isGrabbed = false;
                spriteRenderer.color = Color.white;

                if (walker != null)
                {
                    walker.enabled = true;
                }

                if (enemyGun != null)
                {
                    enemyGun.enabled = true;
                }
            }
        }
    }

    void Die()
    {
        // Optional: play effect here
        Destroy(gameObject);
    }
}
