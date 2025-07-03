using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public int nextLineIndex;

    public string questIdToTrigger;
}
