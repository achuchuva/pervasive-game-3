using UnityEngine;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public GameObject explosionPrefab;       // Prefab for explosions
    public RectTransform gameOverPanel;       // UI Panel to animate
    public string animationTriggerName = "Show"; // Animator trigger name
    public int explosionCount = 5;           // How many explosions
    public float explosionInterval = 0.05f;
    public Vector2 spawnAreaMin = new Vector2(-10, -5); // Area to spawn explosions
    public Vector2 spawnAreaMax = new Vector2(10, 5);
    public float timeScaleDuringGameOver = 0.2f; // How much to slow time

    private Animator panelAnimator;

    void Awake()
    {
        if (gameOverPanel != null)
        {
            panelAnimator = gameOverPanel.GetComponent<Animator>();
        }
    }

    public void TriggerGameOver()
    {
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        // Slow down time
        Time.timeScale = timeScaleDuringGameOver;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Also camera shake
        CameraShake cameraShake = FindFirstObjectByType<CameraShake>();
        if (cameraShake != null)
        {
            cameraShake.Shake(0.5f, 0.5f); // Adjust duration and magnitude as needed
        }

        // Start spawning explosions one after another
        for (int i = 0; i < explosionCount; i++)
        {
            SpawnExplosion();
            yield return new WaitForSecondsRealtime(explosionInterval); // Realtime ignores timeScale
        }

        // Trigger Game Over panel animation
        if (panelAnimator != null)
        {
            panelAnimator.SetTrigger(animationTriggerName);
        }

        // Optional: After some time, restore normal time (if you want)
        Destroy(this);
    }

    private void SpawnExplosion()
    {
        if (explosionPrefab == null) return;

        Vector2 spawnPos = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        Instantiate(explosionPrefab, spawnPos, Quaternion.identity);
    }
}
