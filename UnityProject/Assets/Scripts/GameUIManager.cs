using UnityEngine;
using UnityEngine.UI;
using TMPro; // Assuming you are using TextMeshPro for better text rendering

public class GameUIManager : MonoBehaviour
{
    public Head head;
    public Slider healthSlider;
    public TextMeshProUGUI healthText; // Assuming you are using TextMeshPro for better text rendering
    public GameObject gameOverPanel;

    public Slider laserSlider;
    public TextMeshProUGUI laserText;
    public GameObject laserTextReady;

    void Start()
    {
        if (head == null)
        {
            head = FindFirstObjectByType<Head>();
        }
        gameOverPanel.SetActive(false);
        healthSlider.maxValue = head.maxHealth;
        healthSlider.value = head.currentHealth;

        laserSlider.maxValue = head.laserCharge;
        laserSlider.value = head.currentLaserCharge;

        head.OnDeath += ShowGameOver;
    }

    void Update()
    {
        healthSlider.value = head.currentHealth;
        healthText.text = head.currentHealth.ToString();

        laserSlider.value = head.currentLaserCharge;
        laserText.text = head.currentLaserCharge.ToString();
        if (head.currentLaserCharge >= head.laserCharge)
        {
            laserTextReady.SetActive(true);
        }
        else
        {
            laserTextReady.SetActive(false);
        }
    }

    void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }
}
