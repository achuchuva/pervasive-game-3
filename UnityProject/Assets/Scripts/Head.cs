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

    public SpriteRenderer _spriteRenderer;
    public Sprite _openMouthSprite;
    public Sprite _closedMouthSprite;
    public UnityAction OnDeath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth; // Initialize health to maxHealth
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
