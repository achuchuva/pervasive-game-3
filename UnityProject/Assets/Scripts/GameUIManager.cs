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

    public TextMeshProUGUI scoreText; // Assuming you are using TextMeshPro for better text rendering
    public TextMeshProUGUI scoreTextGameOver; // Assuming you are using TextMeshPro for better text rendering
    private int score = 0; // Initialize score to 0

    void Start()
    {
        if (head == null)
        {
            head = FindFirstObjectByType<Head>();
        }
        gameOverPanel.SetActive(false);
        healthSlider.maxValue = head.maxHealth;
        healthSlider.value = head.currentHealth;

        laserSlider.maxValue = head.maxLaserCharge;
        laserSlider.value = head.currentLaserCharge;

        head.OnDeath += ShowGameOver;
    }

    void Update()
    {
        healthSlider.value = head.currentHealth;
        healthText.text = head.currentHealth.ToString();

        laserSlider.value = head.currentLaserCharge;
        laserText.text = head.currentLaserCharge.ToString();
        if (head.currentLaserCharge >= head.maxLaserCharge)
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

    public void EnemyDied(int scoreIncrement)
    {
        // Update the score or perform any other actions when an enemy dies
        // For example, you can increase the score and update the UI
        score += scoreIncrement;
        scoreText.text = "Score: " + score.ToString(); // Update the score text
        scoreTextGameOver.text = "Score: " + score.ToString(); // Update the score text in the game over panel
    }
}
