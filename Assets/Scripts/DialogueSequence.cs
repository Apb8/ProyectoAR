using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCDialogueSequence", menuName = "Dialogue/NPC Dialogue Sequence")]
public class DialogueSequence : ScriptableObject
{
    public List<Dialogue> dialogues;
}
