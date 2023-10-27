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
    void FixedUpdate()
    {
        skipRelease = false;
        int layerMask = 1 << 8;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction*maxRayDistance, Color.yellow);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxRayDistance, layerMask))
        {
            highlightedObject = hit.collider.gameObject;
            if (Input.GetMouseButtonDown(0) && selectedObject is null)
            {
                Debug.Log("Selecting");
                selectedObject = highlightedObject;
                FollowMouse fm = selectedObject.GetComponent<FollowMouse>();
                fm.ToggleFollow();
                skipRelease = true;
            }
        }

        if (Input.GetMouseButtonDown(0) && selectedObject != null && !skipRelease)
        {
            Debug.Log("Releasing");
            FollowMouse fm = selectedObject.GetComponent<FollowMouse>();
            fm.ToggleFollow();
            selectedObject = null;
        }
    }
}
