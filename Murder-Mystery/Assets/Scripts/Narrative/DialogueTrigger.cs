using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    DialogueManager dialogueManager;
    [SerializeField]
    private Dialogue dialogue;

    public void TriggerDialogue()
    {
        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue(dialogue);
        }
    }
}
