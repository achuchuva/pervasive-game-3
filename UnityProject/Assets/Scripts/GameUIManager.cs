using UnityEngine;
using UnityEngine.UI;
using TMPro; // Assuming you are using TextMeshPro for better text rendering

public class GameUIManager : MonoBehaviour
{
    public Head head;
    public Slider healthSlider;
    public TextMeshProUGUI healthText; // Assuming you are using TextMeshPro for better text rendering
    public GameObject gameOverPanel;

    void Start()
    {
        if (head == null)
        {
            head = FindFirstObjectByType<Head>();
        }
        gameOverPanel.SetActive(false);
        healthSlider.maxValue = head.maxHealth;
        healthSlider.value = head.currentHealth;

        head.OnDeath += ShowGameOver;
    }

    void Update()
    {
        healthSlider.value = head.currentHealth;
        healthText.text = head.currentHealth.ToString();
    }

    void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }
}
