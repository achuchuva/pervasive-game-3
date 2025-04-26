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

    private RectTransform _rectTransform;
    public Image _image;
    public Sprite _openHandSprite;
    public Sprite _closedHandSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Smoothly move the hand to the new position
        Vector2 targetPosition = new Vector2(x, y);
        Vector2 currentPosition = _rectTransform.anchoredPosition;
        Vector2 newPosition = Vector2.Lerp(currentPosition, targetPosition, Time.deltaTime * 10f); // Adjust the speed as needed
        _rectTransform.anchoredPosition = newPosition;

        if (fist)
        {
            _image.sprite = _closedHandSprite;
        }
        else
        {
            _image.sprite = _openHandSprite;
        }
    }
}
