using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Head : MonoBehaviour
{
    public float x;
    public float y;
    public bool mouthOpen;
    [HideInInspector] public int currentHealth;
    public int maxHealth = 100;

    [HideInInspector] public float laserCharge = 100f;
    [HideInInspector] public float currentLaserCharge;

    public SpriteRenderer _spriteRenderer;
    public Sprite _openMouthSprite;
    public Sprite _closedMouthSprite;
    public UnityAction OnDeath;

    public float mouthRadius = 2f; // Radius for power-up collection
    public LayerMask powerupLayer; // Layer for power-ups

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth; // Initialize health to maxHealth
        currentLaserCharge = 0f;
        laserCharge = 100f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Smoothly move the head to the new position
        Vector3 targetPosition = new Vector3(x, y, transform.position.z);
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * 10f); // Adjust the speed as needed
        transform.position = newPosition;

        // Update the sprite based on mouthOpen state
        if (mouthOpen)
        {
            _spriteRenderer.sprite = _openMouthSprite;
        }
        else
        {
            _spriteRenderer.sprite = _closedMouthSprite;
        }

        TryGetPowerup(); // Check for power-ups
    }

    void TryGetPowerup()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, mouthRadius, powerupLayer);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Powerup"))
            {
                PowerUp powerUp = hit.GetComponent<PowerUp>();
                if (powerUp != null)
                {
                    string powerUpName = powerUp.powerUpName;
                    HandlePowerUp(powerUpName);
                    Destroy(hit.gameObject); // Remove the power-up after collecting it
                }
            }
        }
    }

    void HandlePowerUp(string powerUpName)
    {
        // Example power-up actions based on the name
        switch (powerUpName)
        {
            case "Laser":
                currentLaserCharge += 20; // Increase laser charge
                if (currentLaserCharge > laserCharge)
                {
                    currentLaserCharge = laserCharge; // Cap charge at max charge
                }
                break;

            case "Health":
                currentHealth += 20; // Heal the head
                if (currentHealth > maxHealth)
                {
                    currentHealth = maxHealth; // Cap health at maxHealth
                }
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }
}
