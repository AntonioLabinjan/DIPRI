using UnityEngine;

[CreateAssetMenu(fileName = "ModuleDef",
                 menuName = "Procedural/Module Definition",
                 order = 100)]
public class ModuleDefinition : ScriptableObject
{
    public GameObject prefab;

    public Vector3Int size = Vector3Int.one;
    public Bounds localBounds;
}
