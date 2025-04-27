using UnityEngine;

public class RandomWalker : MonoBehaviour
{
    public float speed = 2f;
    public float waitTime = 1f;
    public Vector2 areaMin = new Vector2(-5, -3); // x1, y1
    public Vector2 areaMax = new Vector2(5, 3);   // x2, y2

    private Vector2 target;
    private float waitTimer = 0f;
    private bool moving = false;
    private float originalSpeed;

    void Start()
    {
        PickNewTarget();
        originalSpeed = speed; // Store the original speed
    }

    void Update()
    {
        // Check if the postion is within the defined area
        if (transform.position.x < areaMin.x || transform.position.x > areaMax.x || transform.position.y < areaMin.y || transform.position.y > areaMax.y)
        {
            // Temporarily increase speed to move back into the area
            speed = 12f;
            waitTimer = 0f; // Reset wait timer to avoid waiting when out of bounds
        }
        else
        {
            speed = originalSpeed; // Reset speed to original
        }

        if (moving)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target) < 0.1f)
            {
                moving = false;
                waitTimer = waitTime;
            }
        }
        else
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                PickNewTarget();
            }
        }
    }

    void PickNewTarget()
    {
        float x = Random.Range(areaMin.x, areaMax.x);
        float y = Random.Range(areaMin.y, areaMax.y);
        target = new Vector2(x, y);
        moving = true;
    }
}
