using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public string powerUpName = "Speed Boost";  // Set this in the Inspector to change name
    public float rotationSpeed = 100f;          // Speed of rotation

    void Update()
    {
        // Spin the object around the Z axis (2D rotation)
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
