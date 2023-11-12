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
        if (dialogueTreeRunner == null)
            return;

        Debug.Log("Calling Dialouge tree runner");
        dialogueTreeRunner.UpdateTree();
    }

}
