using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Base Class of all Interactables.
 */
public abstract class Interactable : MonoBehaviour
{

    [SerializeField]
    private int objectHighlightMaterialIndex = 1;
    [SerializeField]
    private float higlightScale = 1.15f;
    [SerializeField]
    private DialogueTree dialogueTree;

    protected DialogueTreeRunner dialogueTreeRunner;

    public bool bIsInteractable = true;
    MeshRenderer meshRenderer;

    protected virtual void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        dialogueTreeRunner = GetComponent<DialogueTreeRunner>();
    }

    public abstract void OnInteraction();

    /*
     * Show higlight around object. 
     */
    public void StartHighlightingInteractable()
    {
        if(!meshRenderer)
        {
            return;
        }
        meshRenderer.materials[objectHighlightMaterialIndex].SetFloat("_Scale", higlightScale);
    }

    /*
     * Stop showing higlight around object. 
     */
    public void StopHighlightingInteractable()
    {
        if (!meshRenderer)
        {
            return;
        }
        meshRenderer.materials[objectHighlightMaterialIndex].SetFloat("_Scale", 0.9f);
    }

    protected void StartDialogue()
    {
        if(dialogueTree)
        {
            DialogueTree clonedTree = dialogueTree.Clone();
            DialogueManager.Instance.ShowDialogue(clonedTree);
        }
    }
}
