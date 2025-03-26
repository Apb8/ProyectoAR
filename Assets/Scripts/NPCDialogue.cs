using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    public Dialogue dialogueData;
    private bool playerInRange; //revisar com ho farem

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))//ja ho canviarem deixo aixo per debug
        {
            DialogueManager.Instance.StartDialogue(dialogueData);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
