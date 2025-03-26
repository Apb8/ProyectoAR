using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public GameObject dialogueUI;
    public Image characterPortrait;
    public TMP_Text characterNameText;
    public TMP_Text dialogueText;
    public Button nextButton;

    private Queue<Dialogue.DialogueLine> dialogueQueue;
    private bool isDialogueActive = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        dialogueQueue = new Queue<Dialogue.DialogueLine>();
        dialogueUI.SetActive(false);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (isDialogueActive) return;
        isDialogueActive = true;
        dialogueQueue.Clear();

        foreach(var line in dialogue.lines)
        {
            dialogueQueue.Enqueue(line);
        }

        dialogueUI.SetActive(true);
        ShowNextLine();
    }

    public void ShowNextLine()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        var line = dialogueQueue.Dequeue();
        characterPortrait.sprite = line.characterPortrait;
        characterNameText.text = line.characterName;
        dialogueText.text = line.dialogueText;
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        dialogueUI.SetActive(false);
    }
}
