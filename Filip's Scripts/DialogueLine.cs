using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea(2, 5)]
    public string dialogueText;
    public List<DialogueChoice> choices; 
}