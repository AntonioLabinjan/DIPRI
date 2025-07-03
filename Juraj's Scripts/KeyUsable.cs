using UnityEngine;

public class KeyUsable : MonoBehaviour, IUsable
{
    public float range = 3f;
    public LayerMask doorLayer;

    public void Use()
    {
        var cam = Camera.main;
        if (cam == null) return;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out var hit, range))
        {
            var door = hit.collider.GetComponentInParent<LockedDoor>();
            if (door != null)
            {
                door.ToggleLock();
            }
        }

    }
}
