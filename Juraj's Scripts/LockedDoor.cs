using UnityEngine;

public class LockedDoor : MonoBehaviour, IInteractable
{
    public UnlockedDoor unlockedDoor;
    private bool isLocked = true;

    public void Interact()
    {
        if (isLocked)
        {
            Debug.Log("🚪 The door is locked.");
        }
        else
        {
            unlockedDoor.Interact();
        }
    }
    public void ToggleLock() => isLocked = !isLocked;
}