using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Responsible for NPC interactions.
 */
public class NPCInteractable : Interactable
{
    private DialogueTreeRunner dialogueTreeRunner;

    protected override void Awake()
    {
        base.Awake();
        dialogueTreeRunner = GetComponent<DialogueTreeRunner>();
    }

    /*
     * Run Dialogue Tree on interaction.
     */
    public override void OnInteraction()
    {
        Debug.Log("Interacted");
        if (dialogueTreeRunner == null)
            return;
        dialogueTreeRunner.UpdateTree();
    }

}
