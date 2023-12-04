using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adapted from https://www.youtube.com/watch?v=mOqHVMS7-Nw
public class FadeCheck : MonoBehaviour
{
    private List<ObjectFader> objectFaders;
    [SerializeField]
    private PlayerCharacter playerCharacter;

    private void Start()
    {
        objectFaders = new List<ObjectFader>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCharacter == null)
        {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            if (!playerGO)
            {
                return;
            }
            playerCharacter = playerGO.GetComponent<PlayerCharacter>();
        }

        // Clears all object faders every frame and re-adds any that are still blocking.
        foreach(ObjectFader obj in objectFaders)
        {
            obj.bDoFade = false;
        }
        objectFaders.Clear();

        Vector3 dir = playerCharacter.gameObject.transform.position - transform.position;
        Ray ray = new Ray(transform.position, dir);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);

        // Sorting the hit results so that they are in order of hit.
        Array.Sort(hits, (RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance));

        foreach (RaycastHit hit in hits )
        {
            if (hit.collider == null)
                continue;

            // If the ray has hit the player no need to check if anymore objects are blocking. Fade any objects in front of it.
            else if (hit.collider.gameObject.GetComponent<PlayerCharacter>() != null)
            { 
                foreach(ObjectFader obj in objectFaders)
                {
                    obj.bDoFade = true;
                }
                break;
            }

            // If the ray has hit an object in front of the player add it to the list of objects to be faded.
            else
            {
                ObjectFader objectFader = hit.collider.gameObject.GetComponent<ObjectFader>();
                if (objectFader != null)
                {
                    objectFaders.Add(objectFader);
                }
            }
        }
                
    }
}
