using UnityEngine;
using UnityEngine.UI;

public class Head : MonoBehaviour
{
    public float x;
    public float y;
    public bool mouthOpen;
    public bool active;

    private RectTransform _rectTransform;
    public Image _image;
    public Sprite _openMouthSprite;
    public Sprite _closedMouthSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (active)
        {
            _image.enabled = true;
        }
        else
        {
            _image.enabled = false;
        }

        // Smoothly move the head to the new position
        Vector2 targetPosition = new Vector2(x, y);
        Vector2 currentPosition = _rectTransform.anchoredPosition;
        Vector2 newPosition = Vector2.Lerp(currentPosition, targetPosition, Time.deltaTime * 10f); // Adjust the speed as needed
        _rectTransform.anchoredPosition = newPosition;

        // Update the sprite based on mouthOpen state
        if (mouthOpen)
        {
            _image.sprite = _openMouthSprite;
        }
        else
        {
            _image.sprite = _closedMouthSprite;
        }
    }
}
