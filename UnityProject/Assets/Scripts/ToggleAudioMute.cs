using UnityEngine;

public class ToggleAudioMute : MonoBehaviour
{
    public AudioSource audioSource;
    public KeyCode muteKey = KeyCode.M;

    void Update()
    {
        if (Input.GetKeyDown(muteKey) && audioSource != null)
        {
            audioSource.mute = !audioSource.mute;
        }
    }
}
