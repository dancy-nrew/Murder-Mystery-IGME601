using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adapted from https://www.youtube.com/watch?v=mOqHVMS7-Nw
public class FadeCheck : MonoBehaviour
{
    private ObjectFader objectFader;
    [SerializeField]
    private PlayerCharacter playerCharacter;

    // Update is called once per frame
    void Update()
    {
        if(playerCharacter != null)
        {
            Vector3 dir = playerCharacter.gameObject.transform.position - transform.position;
            Ray ray = new Ray(transform.position, dir);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                if (hit.collider == null)
                    return;
                if(hit.collider.gameObject.GetComponent<PlayerCharacter>() != null)
                {
                    if(objectFader != null)
                    {
                        objectFader.bDoFade = false;
                        objectFader = null;
                    }
                }
                else
                {
                    objectFader = hit.collider.gameObject.GetComponent<ObjectFader>();
                    if(objectFader != null)
                    {
                        objectFader.bDoFade = true;
                    }
                }
            }
        }
    }
}
