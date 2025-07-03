using UnityEngine;

public class NoteUsable : MonoBehaviour, IUsable
{
    [TextArea] public string noteText;

    public void Use()
    {
        Debug.Log($"📄 Note reads:\n{noteText}");
    }
}
