using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class NPCDialogue : MonoBehaviour
{
    public Dialogue dialogueData;
    //private bool playerInRange; //revisar com ho farem
    private Transform playerCamera;
    public float interactionDistance = 5f;
    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }
    private void Start()
    {
        playerCamera = Camera.main?.transform; //Asigna la AR camera automaticamente - revisar - Revisar lo del interrogant q mho han recomanat
        //inputActions = new PlayerInputActions(); //Ho passo al awake perq s'executi abans
        //inputActions.Player.Interact.performed += ctx => TryStartDialogue();
        if(dialogueData == null)
        {
            Debug.LogError($"[NPCDialogue] {gameObject.name} no tiene dialogue asignado en inspector hombre ya");
        }
    }

    //private void OnEnable() => inputActions.Enable();
    //private void OnDisable() => inputActions.Disable();
    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Interact.performed += ctx => TryStartDialogue();
    }

    private void OnDisable()
    {
        inputActions.Player.Interact.performed -= ctx => TryStartDialogue();
        inputActions.Disable();
    }

    private void TryStartDialogue()
    {
        if(dialogueData == null)
        {
            Debug.LogError($"[NPCDialogue] {gameObject.name} intento iniciar un dialogo sin datos"); //debug despres borrem
            return;
        }

        if(playerCamera == null)
        {
            Debug.LogError("[NPCDialogue] no se encontro la camara principal"); //debug despres borrem
            return;
        }

        if (Vector3.Distance(transform.position, playerCamera.position) < interactionDistance)
        {
            if(DialogueManager.Instance != null)
            {
                DialogueManager.Instance.StartDialogue(dialogueData);
            }
            else
            {
                Debug.LogError("[NPCDialogue] DialogueManager.Instance es null"); //debug despues borro
            }
        }
    }
    //private void Update()
    //{
    //    //if (playerInRange && Input.GetKeyDown(KeyCode.E))//ja ho canviarem deixo aixo per debug
    //    //{
    //    //    DialogueManager.Instance.StartDialogue(dialogueData);
    //    //}
    //    if (Vector3.Distance(transform.position, playerCamera.position) < interactionDistance)
    //    {
    //        if (Input.GetKeyDown(KeyCode.E))//para debug en pc, luego se cambia
    //        {
    //            DialogueManager.Instance.StartDialogue(dialogueData);
    //        }
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerInRange = true;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerInRange = false;
    //    }
    //}
}
