using UnityEngine;

[ExecuteInEditMode]
public class DebugBounds : MonoBehaviour
{
    public ModuleDefinition def;

    void OnDrawGizmosSelected()
    {
        if (def == null) return;

        Gizmos.color = Color.yellow;
        var worldCenter = transform.TransformPoint(def.localBounds.center);
        var worldSize = transform.TransformVector(def.localBounds.size);
        Gizmos.DrawWireCube(worldCenter, worldSize);
    }
}
