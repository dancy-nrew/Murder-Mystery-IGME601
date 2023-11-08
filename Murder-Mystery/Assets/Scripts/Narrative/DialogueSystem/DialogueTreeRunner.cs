using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adapted from https://youtu.be/nKpM98I7PeM?si=6_zO-Egnx1kB-9Ys
// This class runs a dialogue tree and will be places on an interactable object/person.
public class DialogueTreeRunner : MonoBehaviour
{

    [SerializeField]
    DialogueTree dialogueTree;
    [SerializeField]
    private string characterName;

    // Clones dialogue tree in case multiple object hold the same dialogueTree.
    void Start()
    {
        dialogueTree = dialogueTree.Clone();
    }

    // Update is called once per frame
    void Update()
    {
        //dialogueTree.UpdateTree();
    }

    /*
     * Function to ask dialogue manager to run DialogueTree asset.
     */
    public void UpdateTree()
    {
        DialogueManager.Instance.ShowDialogue(dialogueTree);
    }
}
