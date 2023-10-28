using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSelect : MonoBehaviour
{
    private GameObject highlightedObject;
    private GameObject selectedObject;
    public float maxRayDistance;
    bool skipRelease;

    // Update is called once per frame
    private void Update()
    {
        // Handle inputs from the mouse
        skipRelease = false;
        if (Input.GetMouseButtonDown(0) && selectedObject is null && highlightedObject != null)
        {
            selectedObject = highlightedObject;
            FollowMouse fm = selectedObject.GetComponent<FollowMouse>();
            fm.ToggleFollow();
            skipRelease = true;
        }
        if (Input.GetMouseButtonDown(0) && selectedObject != null && !skipRelease)
        {
            FollowMouse fm = selectedObject.GetComponent<FollowMouse>();
            fm.ToggleFollow();
            selectedObject = null;
        }
    }

    //FixedUpdate is called in static time intervals and is best recommended when working with Unity Physics
    void FixedUpdate()
    {
        // Detect which card the mouse is hovering over
        int layerMask = 1 << 8;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction*maxRayDistance, Color.yellow);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxRayDistance, layerMask))
        {
            highlightedObject = hit.collider.gameObject;
            
        } else
        {
            highlightedObject = null;
        }

        
    }
}
