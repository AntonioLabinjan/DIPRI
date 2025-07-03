using UnityEngine;

public class FlashlightUsable : MonoBehaviour, IUsable
{
    public Light beam;      
    public void Use()
    {
        if (beam != null)
            beam.enabled = !beam.enabled;
        else
            Debug.LogWarning("FlashlightUsable: no Light assigned!");
    }
}
