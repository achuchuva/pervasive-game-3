using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; // Assuming you are using TextMeshPro for better text rendering

public class TutorialTipUIManager : MonoBehaviour
{
    public TextMeshProUGUI tipText;
    public GameObject tipPanel;

    [TextArea]
    public string[] tips;           // Set messages in Inspector
    public float delayBeforeFirstTip = 5f;
    public float timeBetweenTips = 10f;
    public float displayDuration = 10f;

    private int currentTipIndex = 0;

    void Start()
    {
        tipPanel.SetActive(false);
        StartCoroutine(ShowTipsRoutine());
    }

    IEnumerator ShowTipsRoutine()
    {
        yield return new WaitForSeconds(delayBeforeFirstTip);

        while (currentTipIndex < tips.Length)
        {
            tipPanel.SetActive(true);
            tipText.text = tips[currentTipIndex];

            yield return new WaitForSeconds(displayDuration);

            tipPanel.SetActive(false);

            currentTipIndex++;
            yield return new WaitForSeconds(timeBetweenTips);
        }
    }

    public void OverrideTutorial()
    {
        StopAllCoroutines(); // Stop the current tip routine
        tipPanel.SetActive(false); // Hide the tip panel
        currentTipIndex = tips.Length; // Skip to the end of the tips
    }
}
