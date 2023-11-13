using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    bool isFollowingMouse = false;
    public float followDistance;
    public bool _inmovable = false;

    // Update is called once per frame
    void Update()
    {
        if (isFollowingMouse && !_inmovable)
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane * followDistance));;
            transform.position = point;
        }
    }

    public void ToggleFollow()
    {
        // Toggle whether this object is following the mouse.
        isFollowingMouse = !isFollowingMouse;
        

        // If an object is released, movement for it needs to be resolved
        if (!isFollowingMouse && !_inmovable) { 
           PlayToLane ptl = gameObject.GetComponent<PlayToLane>();
           ptl.Release();
        }
    }

    public void ToggleInmovable(){
        // Toggle whether an object can be moved at all.
        _inmovable = !_inmovable;
    }
}
