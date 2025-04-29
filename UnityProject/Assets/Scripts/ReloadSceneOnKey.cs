using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadSceneOnKey : MonoBehaviour
{
    public string sceneName; // Set this in the Inspector
    public KeyCode reloadKey = KeyCode.R;

    void Update()
    {
        if (Input.GetKeyDown(reloadKey))
        {
            // Set time to normal
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f; // Reset fixed delta time to default

            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
