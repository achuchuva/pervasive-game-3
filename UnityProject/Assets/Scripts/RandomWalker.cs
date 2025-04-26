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

    void Start()
    {
        PickNewTarget();
    }

    void Update()
    {
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
