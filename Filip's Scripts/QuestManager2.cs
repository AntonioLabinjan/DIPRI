using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class QuestManager2 : MonoBehaviour
{
    public float questDuration = 60f;
    public float timer;
    public bool questStarted = false;
    public bool questEnded = false;
    public TMP_Text timerText;
    public int deliveredSuitcases = 0;
    public int totalSuitcases = 3;
    public bool suitcaseDelivered = false;
    

    public GameObject QuestFail;
    public GameObject QuestPass;

    public TMP_Text PassText;
    public TMP_Text FailText;

    public TMP_Text RoomNumber;
    public TMP_Text ClickedRoomNum;
    public TMP_Text Delivered;

    public string[] possibleRoomNumbers = 
        {"300", "301", "302", "303", "304", "305", "306", 
        "307", "308", "309", "310", "311", "312", "313", 
        "314", "315", "316", "317", "318", "319", "320", 
        "321", "322", "323", "324", "325", "326", "327", 
        "328", "329" };
    public string currentRoomNumber;

    void Start()
    {
        PickRandomRoom();
    }

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
        RoomNumber.SetText($"Deliver suitcase to room: {currentRoomNumber}");
    }
    void Awake()
    {
        if (possibleRoomNumbers == null || possibleRoomNumbers.Length == 0)
        {
            possibleRoomNumbers = new string[] 
            {"300", "301", "302", "303", "304", "305", "306", "307", "308", "309", "310", "311", "312", "313",
            "314", "315", "316", "317", "318", "319", "320",
            "321", "322", "323", "324", "325", "326", "327", "328", "329" };
        }
    }
    public void PickRandomRoom()
    {
        if (possibleRoomNumbers.Length > 0)
        {
            int index = Random.Range(0, possibleRoomNumbers.Length);
            currentRoomNumber = possibleRoomNumbers[index];
        }
        else
        {
            Debug.LogWarning("No room numbers defined in possibleRoomNumbers!");
        }
    }
    public void StartQuest()
    {
     
        timer = questDuration;
        questStarted = true;
        timerText.gameObject.SetActive(true);
        
    }
    public void FoundRoom()
    {
        if (!questEnded && questStarted)
        {
            deliveredSuitcases++;

            if (deliveredSuitcases < totalSuitcases)
            {
                timer += 2f;
                PickRandomRoom();
                RoomNumber.SetText($"Deliver suitcase to room: {currentRoomNumber}");
         
            }
            else
            {
                questEnded = true;
                timerText.gameObject.SetActive(false);

                if (QuestPass != null) QuestPass.SetActive(true);

                PassText.text = "Well done, all suitcases delivered!";
                RoomNumber.SetText("");

                StartCoroutine(ProceedToSampleScene());
            }
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

            FailText.text = ("Man it's not that hard, can you not read...");
            StartCoroutine(ProceedToSampleScene());

        }
    }

    IEnumerator ProceedToSampleScene()
    {
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("World");
    }
    public IEnumerator FadeOutClickedText(float duration)
    {
        Color originalColor = ClickedRoomNum.color;
        float startAlpha = originalColor.a;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float blend = 1f - (t / duration);
            ClickedRoomNum.color = new Color(originalColor.r, originalColor.g, originalColor.b, blend * startAlpha);
            yield return null;
        }
        ClickedRoomNum.text = "";
        ClickedRoomNum.color = new Color(originalColor.r, originalColor.g, originalColor.b, startAlpha); 
    }
    public IEnumerator HideDeliveredText(float duration)
    {
        Color originalColor = Delivered.color;
        float startAlpha = originalColor.a;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float blend = 1f - (t / duration);
            Delivered.color = new Color(originalColor.r, originalColor.g, originalColor.b, blend * startAlpha);
            yield return null;
        }
        Delivered.text = "";
        Delivered.color = new Color(originalColor.r, originalColor.g, originalColor.b, startAlpha);
    }
}

