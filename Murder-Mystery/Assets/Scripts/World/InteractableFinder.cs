using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Responsible for highlight interactable objects in the scene.
public class InteractableFinder : MonoBehaviour
{
    // Overriding Unity method to use sphere collider to find interactables close to player and highlight them.
    private void OnTriggerEnter(Collider other)
    {
        Interactable interactable = other.gameObject.GetComponentInParent<Interactable>();
        if (interactable && interactable.bIsInteractable)
        {
            interactable.StartHighlightingInteractable();
        }    
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable interactable = other.gameObject.GetComponentInParent<Interactable>();
        if (interactable && interactable.bIsInteractable)
        {
            interactable.StopHighlightingInteractable();
        }
    }
}
