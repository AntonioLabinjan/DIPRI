using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class QuestManager3 : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text dialogueText;
    public GameObject questPassCanvas;

    [Header("Audio")]
    public AudioSource knockSound;
    public AudioSource gunshotSound;

    [Header("State")]
    public bool visited405 = false;
    public bool visited504 = false;
    public bool deliveredBag = false;
    public bool heardGunshot = false;
    public bool questFinished = false;

    public GameObject QuestPass;
    public TMP_Text PassText;

    public GameObject suitcase;

    private PlayerController playerController;

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();

        dialogueText.text = "Deliver the suitcase to room 405.. or 504?";
        StartCoroutine(HideObjectiveText(2f));
        questPassCanvas.SetActive(false);
    }

    public void KnockRoom504()
    {
        if (visited504) return;

        StartCoroutine(Room504Sequence());
    }

    IEnumerator Room504Sequence()
    {
        visited504 = true;

        knockSound.Play();
        yield return new WaitForSeconds(1f);

        if (playerController != null)
            playerController.canLook = false;

        dialogueText.text = "Hello, did you lose your bag?";

        yield return new WaitForSeconds(2f);

        dialogueText.text = "I didn’t lose my bag. I lost everything.";

        yield return new WaitForSeconds(3f);

        dialogueText.text = "Maybe this isn’t the right guest...";

        yield return new WaitForSeconds(2f);
        dialogueText.text = "";

        if (playerController != null)
            playerController.canLook = true;
    }

    public void KnockRoom405()
    {
        if (deliveredBag) { return; }
        if (visited504) {
     
            StartCoroutine(Room405Sequence());
        }
        
   
    }

    IEnumerator Room405Sequence()
    {
        visited405 = true;
        
        knockSound.Play();
        yield return new WaitForSeconds(1f);

        if (playerController != null)
            playerController.canLook = false;

        dialogueText.text = "*The quiet man opens the door and smiles.*";

        yield return new WaitForSeconds(2f);

        dialogueText.text = "Thank you for bringing this.";

        yield return new WaitForSeconds(2f);

        dialogueText.text = "*You hand over the bag.*";
        deliveredBag = true;

        if(suitcase != null)
        {
            Destroy(suitcase);
            suitcase = null;
        }
      
        yield return new WaitForSeconds(2f);

        gunshotSound.Play();
        heardGunshot = true;

        dialogueText.text = "*A gunshot echoes from room 504*";

        if (playerController != null)
            playerController.canLook = true;

        yield return new WaitForSeconds(3.5f);

        dialogueText.text = "Return to the elevator.";

        yield return new WaitForSeconds(3f);
        dialogueText.text = "";

        

    }
    public void EnterElevator()
    {
        if (!heardGunshot || questFinished) return;

        questFinished = true;
        StartCoroutine(ElevatorSequence());
    }


    IEnumerator ElevatorSequence()
    {
        if (QuestPass != null) QuestPass.SetActive(true);

        PassText.text = "Room 504 is no longer occupied...\nIf you need a place to stay.";

        yield return new WaitForSeconds(3f); 

        SceneManager.LoadScene("World"); 
    }
    public IEnumerator HideObjectiveText(float duration)
    {
        Color originalColor = dialogueText.color;
        float startAlpha = originalColor.a;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float blend = 1f - (t / duration);
            dialogueText.color = new Color(originalColor.r, originalColor.g, originalColor.b, blend * startAlpha);
            yield return null;
        }
        dialogueText.text = "";
        dialogueText.color = new Color(originalColor.r, originalColor.g, originalColor.b, startAlpha);
    }
}
