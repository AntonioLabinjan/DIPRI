using UnityEngine;

public class BagDeliveryPoint : MonoBehaviour
{
    public QuestManager3 questManager3;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Suitcase") && questManager3 != null && questManager3.visited504 && questManager3.deliveredBag)
        {
            Destroy(other.gameObject);
        }
    }
}
