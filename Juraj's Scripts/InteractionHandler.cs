using UnityEngine;

[RequireComponent(typeof(Camera))]
public class InteractionHandler : MonoBehaviour
{
    public float interactRange = 3f;
    public LayerMask interactLayer; 

    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) 
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, interactRange, interactLayer))
            {
                var ix = hit.collider.GetComponentInParent<IInteractable>();
                if (ix != null)
                    ix.Interact();
            }
        }
    }
}
