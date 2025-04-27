using UnityEngine;

public class EnemyGrabController : MonoBehaviour
{
    public float grabDuration = 2f;
    private float grabTimer = 0f;

    public Transform grabbedBy { get; set; }
    [SerializeField] private SpriteRenderer spriteRenderer;
    private RandomWalker walker;
    private EnemyGun enemyGun;
    private Enemy enemy;
    private Hand hand;

    void Start()
    {
        walker = GetComponent<RandomWalker>();
        enemyGun = GetComponentInChildren<EnemyGun>();
        enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        if (grabbedBy != null)
        {
            if (walker != null)
            {
                walker.enabled = false;
            }
            if (enemyGun != null)
            {
                enemyGun.enabled = false;
            }

            // Follow the hand
            transform.position = grabbedBy.position;

            // Increase redness over time
            grabTimer += Time.deltaTime;
            float t = Mathf.Clamp01(grabTimer / grabDuration);
            spriteRenderer.color = Color.Lerp(Color.white, Color.red, t);

            if (grabTimer >= grabDuration)
            {
                enemy.Die(); // Call the Die method on the enemy
            }
        }
        else
        {
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
