using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    public Light[] targets;    

    public void Interact()
    {
        foreach (var L in targets)
            L.enabled = !L.enabled;
    }
}
