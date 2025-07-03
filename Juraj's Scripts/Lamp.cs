using UnityEngine;

public class Lamp : MonoBehaviour, IInteractable
{
    public Light lampLight;

    public void Interact()
    {
        if (lampLight != null)
            lampLight.enabled = !lampLight.enabled;
    }
}
