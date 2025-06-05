using UnityEngine;

public class DraggableItem : MonoBehaviour
{
    public int itemNumber;
    public float pickupRange = 3f;
    public float dropRange = 1f;

    private Transform player;
    private Vector3 startPosition;
    private bool isCarried = false;

    private void Start()
    {
        startPosition = transform.position;
        itemNumber = Random.Range(0, 100);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("Nema objekta s tagom 'Player'");
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // Pokupi item
        if (!isCarried && distanceToPlayer <= pickupRange && Input.GetKeyDown(KeyCode.E))
        {
            isCarried = true;
            Debug.Log($"Pokupio predmet, broj: {itemNumber}");
        }

        // Ako nosis item – prati poziciju ispred igraca
        if (isCarried)
        {
            Vector3 holdPosition = player.position + player.forward * 1.5f + Vector3.up * 1f;
            transform.position = Vector3.Lerp(transform.position, holdPosition, Time.deltaTime * 10f);

            // Baci item kad si blizu drop zone i stisnes E
            if (Input.GetKeyDown(KeyCode.E))
            {
                TryDrop();
            }
        }
    }

    private void TryDrop()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, dropRange);
        foreach (var hit in hits)
        {
            DropZone dropZone = hit.GetComponent<DropZone>();
            if (dropZone != null)
            {
                if (itemNumber >= dropZone.minRange && itemNumber <= dropZone.maxRange)
                {
                    Debug.Log("Tocno! Item ubacen.");
                    ShelfQuestManager.Instance.IncreaseCorrectCount();
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Kriva polica! Reset.");
                    ResetPosition();
                }
                return;
            }
        }

        Debug.Log("Nisi blizu ni jedne police.");
    }

    public void ResetPosition()
    {
        isCarried = false;
        transform.position = startPosition;
    }
}
