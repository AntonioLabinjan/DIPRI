using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor.Build.Content;
using System.Collections;

public class QuestManager : MonoBehaviour
{
    public float questDuration = 60f;
    public float timer;
    public bool questStarted = false;
    public bool questEnded = false;
    public TMP_Text timerText;

    public GameObject QuestFail;
    public GameObject QuestPass;

    public TMP_Text PassText;
    public TMP_Text FailText;



    

    void Update()
    {
        if (questStarted && !questEnded)
        {
            timer -= Time.deltaTime;
            timerText.text = "Time Left: " + Mathf.Ceil(timer).ToString();

            if (timer <= 0)
            {
                QuestFailed();
            }
        }
    }
    public void StartQuest()
    {
        timer = questDuration;
        questStarted = true;
        timerText.gameObject.SetActive(true);
    }

    public void FoundTray()
    {
        if (!questEnded && questStarted)
        {
            questEnded = true;
            timerText.gameObject.SetActive(false);

            if (QuestPass != null) {
                QuestPass.SetActive(true);
               

            }

            PassText.text = ("Ahhh, now we can get to work. You're not completely useless.");
            StartCoroutine(ProceedToSampleScene());
        }
    }

    public void QuestFailed()
    {
        if (!questEnded)
        {
            questEnded = true;
            timerText.gameObject.SetActive(false);

            if (QuestFail != null)
            {
                QuestFail.SetActive(true);

            }

            FailText.text = ("Oh well, we'll serve it on the cutting board again...");
  
            StartCoroutine(ProceedToSampleScene());

        }
    }
    IEnumerator ProceedToSampleScene()
    {
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("World");
    }
}
