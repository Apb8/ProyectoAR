using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//scriptable object para almacenar dialogos luego os explico
[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/DialogueData")]
public class Dialogue : ScriptableObject
{
    [System.Serializable]
    public struct DialogueLine
    {
        public string characterName;
        public Sprite characterPortrait;
        [TextArea(3, 5)] public string dialogueText;
        public Response[] responses;
    }
    [System.Serializable]
    public struct Response
    {
        public string responseText;
        public Dialogue nextDialogue; //apunta al siguiente dialogo (revisar como lo haremos)
    }

    public DialogueLine[] lines;
}
