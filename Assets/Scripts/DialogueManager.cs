using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


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

    private Queue<string> currentDialogueChunks = new Queue<string>();
    private const int MaxCharsPerPage = 80;

    private Coroutine typingCoroutine;
    private bool isTyping = false;

    public GameObject continueIcon;

    private bool awaitingNextSequence = false;//testing to fix movile bug

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
        if (isTyping)
        {            
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            dialogueText.maxVisibleCharacters = int.MaxValue;
            isTyping = false;
            nextButton.gameObject.SetActive(true);
            return;
        }

        if (currentDialogueChunks.Count > 0)
        {
            StartTyping(currentDialogueChunks.Dequeue());
            return;
        }

        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        var line = dialogueQueue.Dequeue();
        characterPortrait.sprite = line.characterPortrait;
        characterNameText.text = line.characterName;
        //dialogueText.text = line.dialogueText;

        //paginado
        currentDialogueChunks.Clear();
        var pages = SplitTextIntoPages(line.dialogueText, MaxCharsPerPage);
        foreach (var page in pages)
            currentDialogueChunks.Enqueue(page);

        if (lastNPC != null)
        {
            if (!string.IsNullOrEmpty(line.animationState))
                lastNPC.ApplyAnimationState(line.animationState);
            else
                lastNPC.ResetAnimationState(); //para asegurarme q vuelve a idle
        }

        //primera pag con typeefect
        StartTyping(currentDialogueChunks.Dequeue());

        if (line.responses != null && line.responses.Length > 0)
        {
            nextButton.gameObject.SetActive(false);//ocultamos boton para prevenir bug en movil , igual hay q comentar esta linea
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
            awaitingNextSequence = false; //test debug to fix build bug
            StartDialogue(response.nextDialogue);
        }
        else
        {
            awaitingNextSequence = true; //test debug to fix build bug
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
            if (awaitingNextSequence)//debug test, si segueix igual treure condition
            {
                lastNPC.AdvanceDialogue();
            }
            
            lastNPC = null;
        }
        awaitingNextSequence = false; //debug test
    }

    private List<string> SplitTextIntoPages(string fullText, int maxChars)
    {
        List<string> pages = new List<string>();
        string[] words = fullText.Split(' ');

        string current = "";

        foreach (string word in words)
        {
            if ((current + word).Length + 1 > maxChars)
            {
                pages.Add(current.TrimEnd());
                current = "";
            }
            current += word + " ";
        }

        if (!string.IsNullOrWhiteSpace(current))
            pages.Add(current.TrimEnd());

        return pages;
    }

    private void StartTyping(string text)
    {
        if (continueIcon != null)
            continueIcon.SetActive(false);//test

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(text));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = text;
        dialogueText.maxVisibleCharacters = 0;

        yield return null;

        int totalChars = text.Length;

        for (int i = 0; i <= totalChars; i++)
        {
            dialogueText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(0.02f); //velocidad
        }

        isTyping = false;
        nextButton.gameObject.SetActive(true);

        if (continueIcon != null)
            continueIcon.SetActive(true);//test
    }

}
