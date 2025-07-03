
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(ModuleDefinition))]
public class ModuleDefinitionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ModuleDefinition def = (ModuleDefinition)target;

        if (GUILayout.Button("Bake Bounds From Prefab"))
        {
            BakeBounds(def);
        }
    }

    private void BakeBounds(ModuleDefinition def)
    {
        if (def.prefab == null)
        {
            Debug.LogWarning("Assign a prefab before baking bounds.");
            return;
        }

        GameObject temp = (GameObject)PrefabUtility.InstantiatePrefab(def.prefab);
        if (temp == null)
        {
            Debug.LogError("Could not instantiate prefab.");
            return;
        }

        var renderers = temp.GetComponentsInChildren<Renderer>(true)
            .Where(r => r.GetComponentInParent<SocketAnchor>() == null)
            .ToArray();

        if (renderers.Length == 0)
        {
            Debug.LogWarning($"No valid renderers found on '{def.name}' (after filtering out sockets).");
            DestroyImmediate(temp);
            return;
        }

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        Vector3 localCenter = temp.transform.InverseTransformPoint(bounds.center);
        Vector3 localSize = temp.transform.InverseTransformVector(bounds.size * 0.99f); 

        def.localBounds = new Bounds(localCenter, localSize);

        Debug.Log($"✅ Baked bounds for '{def.name}': center={localCenter}, size={localSize}");

        DestroyImmediate(temp);
        EditorUtility.SetDirty(def);

    }
}
