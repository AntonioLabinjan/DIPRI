using UnityEngine;


public class NPC : MonoBehaviour
{
    public Dialogue dialogue;
    private DialogueManager dialogueManager;
    public string questIdToStart;
    void Start()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();

        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager not found in scene!");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && dialogueManager != null)
        {
            dialogueManager.ShowTalkButton(dialogue);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && dialogueManager != null)
        {
            dialogueManager.HideTalkButton();
        }
    }
    public void AcceptQuest()
    {
        QuestData.CurrentQuestId = questIdToStart;
        QuestData.QuestCompleted = false;
        QuestData.QuestSucceeded = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("QuestScene1");
    }

}
