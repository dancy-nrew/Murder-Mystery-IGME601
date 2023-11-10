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

    public bool bIsInteractable = true;
    MeshRenderer meshRenderer;

    protected virtual void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
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
}
