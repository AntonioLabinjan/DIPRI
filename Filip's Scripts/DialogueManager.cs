using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public GameObject startDialogue;
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Button nextLine;
    public Transform choicesContainer;
    public GameObject choiceButton;
    public GameObject InventoryCanvas;


    private string pendingQuestId = null;
    private Dialogue currentDialogue;
    private int currentLineIndex = 0;


    private PlayerController playerController;

    void Start()
    {

        if (SceneManager.GetActiveScene().name != "World")
        {
            SceneManager.LoadScene("World");
            return;
        }
        
        playerController = FindFirstObjectByType<PlayerController>();

        startDialogue.SetActive(false);
        dialoguePanel.SetActive(false);
        nextLine.onClick.AddListener(DisplayNextLine);
    }

    public void ShowTalkButton(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        startDialogue.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        startDialogue.GetComponent<Button>().onClick.RemoveAllListeners();
        startDialogue.GetComponent<Button>().onClick.AddListener(() => StartDialogue());
    }

    public void HideTalkButton()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        startDialogue.SetActive(false);

    }

    void StartDialogue()
    {
        InventoryCanvas.SetActive(false);
        startDialogue.SetActive(false);
        dialoguePanel.SetActive(true);
        currentLineIndex = 0;

        if (playerController != null)
            playerController.canLook = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        nextLine.onClick.RemoveAllListeners();
        nextLine.onClick.AddListener(DisplayNextLine);

        DisplayNextLine();
    }

    void DisplayNextLine()
    {
        ClearChoices();

        if (currentLineIndex >= currentDialogue.lines.Count)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = currentDialogue.lines[currentLineIndex];

        nameText.text = line.speakerName;
        dialogueText.text = line.dialogueText;

        if (line.choices != null && line.choices.Count > 0)
        {
            nextLine.gameObject.SetActive(false);

            for (int i = 0; i<line.choices.Count; i++)
            {
                DialogueChoice choice = line.choices[i];


                GameObject buttonObj = Instantiate(choiceButton, choicesContainer);
                TMP_Text btnText = buttonObj.GetComponentInChildren<TMP_Text>();
                btnText.text = choice.choiceText;

                AddChoiceListener(buttonObj.GetComponent<Button>(), choice);
                LayoutRebuilder.ForceRebuildLayoutImmediate(choicesContainer.GetComponent<RectTransform>());
            }
        }
        else
        {
            nextLine.gameObject.SetActive(true);
            currentLineIndex++;
        }
    }

  

    void ClearChoices()
    {
        foreach (Transform child in choicesContainer)
        {
            Destroy(child.gameObject);
        }
    }
    void AddChoiceListener(Button button, DialogueChoice choice)
    {
        button.onClick.AddListener(() => OnChoiceSelected(choice));
    }

    void EndDialogue()
    {
   
        dialoguePanel.SetActive(false);
        ClearChoices();
        currentLineIndex = 0;
        startDialogue.SetActive(true);
        InventoryCanvas.SetActive(true);

        if (playerController != null)
            playerController.canLook = true;

        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;

    
        if (!string.IsNullOrEmpty(pendingQuestId))
        {
            StartQuest(pendingQuestId);
            pendingQuestId = null;
        }
    }


    void StartQuest(string questId)
    {
        if (string.IsNullOrEmpty(questId))
        {
            SceneManager.LoadScene("World");
        }
        else
        {

            QuestData.CurrentQuestId = questId;
            QuestData.QuestCompleted = false;
            QuestData.QuestSucceeded = false;


            string sceneName = GetSceneForQuest(questId);

            SceneManager.LoadScene(sceneName);
        }
    }
    void OnChoiceSelected(DialogueChoice choice)
    {
        currentLineIndex = choice.nextLineIndex;
        DialogueLine line = currentDialogue.lines[currentLineIndex];

        nameText.text = line.speakerName;
        dialogueText.text = line.dialogueText;

        ClearChoices();
        nextLine.gameObject.SetActive(true);

        nextLine.onClick.RemoveAllListeners();
        nextLine.onClick.AddListener(EndDialogue);

        if (!string.IsNullOrEmpty(choice.questIdToTrigger))
        {
            pendingQuestId = choice.questIdToTrigger;
        }

    }

    string GetSceneForQuest(string questId)
    {
        if (string.IsNullOrEmpty(questId))
        {
            return "World";  
        }
        switch (questId)
        {
            case "findTrayQuest":
                return "FindTacnaQuestScene";
            case "CarrySuitcaseQuest":
                return "SuitcaseToRoomQuestScene";
            case "lostAndConfusedQuest":
                return "LostAndConfusedScene";
            case "bringBagQuest":
                return "BringBagScene";
            // od tud na dalje se proizvoljno namjesta
            // case dio je quest id a returna se pripadna scena, one se moru proizvoljno nazvat
            case "waterPlantsQuest":
                return "PotPlantsScene";
            case "findRakeQuest":
                return "Maze";
            case "orderByNumberQuest":
                return "ShelveOrganizer";
            case "evilAndUpsideDownQuest":
                return "evilAndUpsideDownScene";
            case "z":
                return "z";
            case "s":
                return "s";
            case "a":
                return "a";
            default:
                return "World"; 
        }
       
    }
}


