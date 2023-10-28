using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    bool isFollowingMouse = false;
    public float followDistance;

    // Update is called once per frame
    void Update()
    {
        if (isFollowingMouse)
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane * followDistance));;
            transform.position = point;
        }
    }

    public void ToggleFollow()
    {
        isFollowingMouse = !isFollowingMouse;
        

        //UNDER CONSTRUCTION
        //if (!isFollowingMouse ) { 
        //    PlayToLane ptl = gameObject.GetComponent<PlayToLane>();
        //    ptl.Release();
        //}
    }
}
