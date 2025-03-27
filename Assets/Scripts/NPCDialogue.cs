using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class NPCDialogue : MonoBehaviour
{
    public Dialogue dialogueData;
    //private bool playerInRange; //revisar com ho farem
    private Transform playerCamera;
    public float interactionDistance = 1.5f;
    private PlayerInputActions inputActions;

    private void Start()
    {
        playerCamera = Camera.main.transform; //Asigna la AR camera automaticamente - revisar
        inputActions = new PlayerInputActions();
        inputActions.Player.Interact.performed += ctx => TryStartDialogue();
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void TryStartDialogue()
    {
        if (Vector3.Distance(transform.position, playerCamera.position) < interactionDistance)
        {
            DialogueManager.Instance.StartDialogue(dialogueData);
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
