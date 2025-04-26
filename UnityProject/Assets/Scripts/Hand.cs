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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Smoothly move the hand to the new position
        Vector3 targetPosition = new Vector3(x, y, transform.position.z);
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * 10f); // Adjust the speed as needed
        transform.position = newPosition;

        if (fist)
        {
            _spriteRenderer.sprite = _closedHandSprite;
        }
        else
        {
            _spriteRenderer.sprite = _openHandSprite;
        }
    }
}
