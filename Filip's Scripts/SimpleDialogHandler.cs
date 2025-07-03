using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SimpleDialogHandler : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject successPanel;
    public GameObject failPanel;
    public TMP_Text successText;
    public TMP_Text failText;

    [Header("Scene Settings")]
    public string returnSceneName = "World";
    public float waitBeforeReturn = 3.5f;

    private bool hasEnded = false;

    // Call this when the player succeeds
    public void ShowSuccess(string message)
    {
        if (hasEnded) return;
        hasEnded = true;

        if (successPanel != null)
            successPanel.SetActive(true);

        if (successText != null)
            successText.text = message;

        StartCoroutine(ReturnToScene());
    }

    // Call this when the player fails
    public void ShowFailure(string message)
    {
        if (hasEnded) return;
        hasEnded = true;

        if (failPanel != null)
            failPanel.SetActive(true);

        if (failText != null)
            failText.text = message;

        StartCoroutine(ReturnToScene());
    }

    private IEnumerator ReturnToScene()
    {
        yield return new WaitForSeconds(waitBeforeReturn);
        SceneManager.LoadScene(returnSceneName);
    }
}
