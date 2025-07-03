using UnityEngine;

public interface IInteractable
{
    /// <summary>
    /// Called when the player right-clicks this object in range.
    /// </summary>
    void Interact();
}