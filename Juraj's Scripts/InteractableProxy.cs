using UnityEngine;

public class InteractableProxy : MonoBehaviour, IInteractable
{
    public UnlockedDoor target;

    public void Interact()
    {
        if (target != null)
            target.Interact();
    }
}
