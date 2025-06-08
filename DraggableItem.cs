using UnityEngine;

public class DraggableItem : MonoBehaviour
{
    public int itemNumber;
    public float pickupRange = 3f;

    private Transform player;
    private Vector3 startPosition;
    private bool isCarried = false;
    private DropZone currentDropZone = null;
    private Rigidbody rb;

    private void Start()
    {
        startPosition = transform.position;
        itemNumber = Random.Range(0, 100);

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Nema Rigidbody komponentu!");
        }

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
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            Debug.Log($"Pokupio predmet, broj: {itemNumber}");
        }

        // Ako nosis item � prati poziciju ispred igraca
        if (isCarried)
        {
            Vector3 holdPosition = player.position + player.forward * 1.5f + Vector3.up * 1f;
            transform.position = Vector3.Lerp(transform.position, holdPosition, Time.deltaTime * 10f);

            // Drop samo ako si unutar drop zone
            if (Input.GetKeyDown(KeyCode.E))
            {
                TryDrop();
            }
        }
    }

    private void TryDrop()
    {
        if (currentDropZone == null)
        {
            Debug.Log("Nisi tocno na polici.");
            return;
        }

        if (itemNumber >= currentDropZone.minRange && itemNumber <= currentDropZone.maxRange)
        {
            Debug.Log("Tocno! Item ubacen.");
            ShelfQuestManager.Instance.IncreaseCorrectCount();

            // Snap na sredinu police i unisti objekt
            transform.position = currentDropZone.transform.position + Vector3.up * 0.5f;
            isCarried = false;
            rb.isKinematic = false;
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Kriva polica! Reset.");
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        isCarried = false;
        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = startPosition;
        currentDropZone = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        DropZone dropZone = other.GetComponent<DropZone>();
        if (dropZone != null)
        {
            currentDropZone = dropZone;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DropZone dropZone = other.GetComponent<DropZone>();
        if (dropZone != null && dropZone == currentDropZone)
        {
            currentDropZone = null;
        }
    }

    public void FixToShelf(Transform shelf)
    {
        isCarried = false;
        transform.SetParent(shelf); // postavlja kao dijete police
        transform.position = shelf.position + Vector3.up * 0.2f; // malo iznad police
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false; // da ne trigerira ponovno
        this.enabled = false; // onemoguci daljnji pickup
    }
}
