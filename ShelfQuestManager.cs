using UnityEngine;

public class ShelfQuestManager : MonoBehaviour
{
    public static ShelfQuestManager Instance;

    private int correctItemCount = 0;
    public int totalRequiredItems = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // osiguraj da postoji samo jedan
        }
    }

    public void IncreaseCorrectCount()
    {
        correctItemCount++;
        Debug.Log("Correct items placed: " + correctItemCount);

        if (correctItemCount >= totalRequiredItems)
        {
            Debug.Log("Quest complete!");
            
        }
    }
}
