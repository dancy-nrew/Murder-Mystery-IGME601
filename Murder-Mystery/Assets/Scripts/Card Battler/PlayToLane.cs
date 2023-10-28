using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UNDER CONSTRUCTION
public class PlayToLane : MonoBehaviour
{
    public float xLane1;
    public float xLane2;
    public float xLane3;
    public float maxX;
    public float playY;
    public bool deciding;

    // Start is called before the first frame update
    void Start()
    {
        deciding = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Nothing to do if a player is not deciding to play their cards
        if (!deciding)
        {
            return;
        }

        if (transform.position.z < -14) { 
            // Not looking for an active lane, this is the same height as the hand
            // Releasing here means returning to hand
        }


    }

    public void Release() { 
    
    }
}
