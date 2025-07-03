using UnityEngine;

public class NotebookUsable : MonoBehaviour, IUsable
{
    private bool isWriting = false;
    private string buffer = "";

    public void Use()
    {
        if (!isWriting)
        {
            buffer = "";
            isWriting = true;
            Debug.Log("✍️ Write in console; press Enter to finish.");
        }
        else
        {
            isWriting = false;
            Debug.Log($"📒 Notebook saved:\n{buffer}");
        }
    }

    void Update()
    {
        if (!isWriting) return;

        foreach (char c in Input.inputString)
        {
            if (c == '\b' && buffer.Length > 0)
                buffer = buffer.Substring(0, buffer.Length - 1);
            else if (c == '\n' || c == '\r')
                Use();      // finish writing
            else
                buffer += c;

            Debug.Log($"Writing: {buffer}");
        }
    }
}
