using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : Interactable
{
    private DialogueTreeRunner dialogueTreeRunner;

    private void Awake()
    {
        dialogueTreeRunner = GetComponent<DialogueTreeRunner>();
    }

    public override void OnInteraction()
    {
        if (dialogueTreeRunner == null)
            return;

        Debug.Log("Calling Dialouge tree runner");
        dialogueTreeRunner.UpdateTree();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
