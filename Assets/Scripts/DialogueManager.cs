using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; } //revisar aixo
    public GameObject dialogueUI;
    public Image characterPortrait;
    public TMP_Text characterNameText;
    public TMP_Text dialogueText;
    public Button nextButton;
    public Transform responseContainer;
    public Button responsePrefab;

    private Queue<Dialogue.DialogueLine> dialogueQueue;
    private bool isDialogueActive = false;
    private Dialogue currentDialogue;

    public NPCDialogue lastNPC;

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
        //if (isDialogueActive || dialogue == null) return; aquesta putisima lñinea feia q falles
        if (dialogue == null) return;
        if (isDialogueActive) EndDialogue(); //tancar dialeg actual abans de començar el nou

        isDialogueActive = true;
        currentDialogue = dialogue;
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

        if (lastNPC != null)
        {
            lastNPC.ApplyAnimationState(line.animationState);
        }

        if (line.responses != null && line.responses.Length > 0)
        {
            ShowResponses(line.responses);
        }
        else
        {
            nextButton.gameObject.SetActive(true);
            responseContainer.gameObject.SetActive(false);
        }

    }

    private void ShowResponses(Dialogue.Response[] responses)
    {
        nextButton.gameObject.SetActive(false);
        responseContainer.gameObject.SetActive(true);

        foreach(Transform child in responseContainer)
        {
            Destroy(child.gameObject);
        }

        foreach(var response in responses)
        {
            Button newButton = Instantiate(responsePrefab, responseContainer);
            TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = response.responseText;

            newButton.onClick.AddListener(() => OnResponseSelected(response));
        }
    }

    private void OnResponseSelected(Dialogue.Response response)
    {
        responseContainer.gameObject.SetActive(false);

        Debug.Log($"Respuesta seleccionada: {response.responseText} | nextDialogue: {(response.nextDialogue ? response.nextDialogue.name : "NULL")}");


        if (response.nextDialogue != null)
        {
            StartDialogue(response.nextDialogue);
        }
        else
        {
            ShowNextLine();
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        dialogueUI.SetActive(false);

        dialogueText.text = ""; //Limpia texto
        characterNameText.text = "";
        characterPortrait.sprite = null;

        if (lastNPC != null)
        {
            lastNPC.AdvanceDialogue();
            lastNPC = null;
        }
    }
}
