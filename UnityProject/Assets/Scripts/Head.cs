using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class Head : MonoBehaviour
{
    public float x;
    public float y;
    public bool mouthOpen;
    [HideInInspector] public int currentHealth;
    public int maxHealth = 100;

    public float maxLaserCharge = 100f;
    [HideInInspector] public float currentLaserCharge;
    public float laserDuration = 5f; // Duration for laser charge to last
    public bool laserReady = false; // Flag to check if laser is ready
    public GameObject laser;

    public SpriteRenderer _spriteRenderer;
    public Sprite _openMouthSprite;
    public Sprite _closedMouthSprite;
    public UnityAction OnDeath;

    public float mouthRadius = 1f; // Radius for power-up collection
    public LayerMask powerupLayer; // Layer for power-ups

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth; // Initialize health to maxHealth
        currentLaserCharge = 0f;
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

        if (laserReady && mouthOpen)
        {
            FireLaser(); // Fire the laser if it's ready and the mouth is open
        }
    }

    void TryGetPowerup()
    {
        if (!mouthOpen) return; // Only check for power-ups if the mouth is open
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
                if (currentLaserCharge >= maxLaserCharge)
                {
                    currentLaserCharge = maxLaserCharge; // Cap charge at max charge
                    laserReady = true; // Set laser as ready
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

    void FireLaser()
    {
        if (laserReady)
        {
            laser.SetActive(true);
            laserReady = false;
            StartCoroutine(DrainLaserCharge());
        }
    }

    IEnumerator DrainLaserCharge()
    {
        while (currentLaserCharge > 0)
        {
            currentLaserCharge -= 20f;
            yield return new WaitForSeconds(1f);
        }

        laser.SetActive(false);
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
