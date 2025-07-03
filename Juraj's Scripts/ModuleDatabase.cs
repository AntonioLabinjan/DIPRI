using UnityEngine;

[CreateAssetMenu(fileName = "ModuleDB", menuName = "Procedural/Module Database")]
public class ModuleDatabase : ScriptableObject
{
    public ModuleDefinition[] modules;
}
