using UnityEngine;

public class DropZone : MonoBehaviour
{
    public int minRange;
    public int maxRange;

    private void OnTriggerEnter(Collider other)
    {
        DraggableItem item = other.GetComponent<DraggableItem>();
        if (item != null)
        {
            if (item.itemNumber >= minRange && item.itemNumber <= maxRange)
            {
                Debug.Log("Tocno!");
                ShelfQuestManager.Instance.IncreaseCorrectCount();
                Destroy(item); 
            }
            else
            {
                Debug.Log("Kriva polica. Probaj ponovno.");
                item.ResetPosition();
            }
        }
    }
}
