using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    [SerializeField]
    private Transform playerTransform;
    private GameObject _audioManagerObject;

    private Vector3 previousPlayerRightComponent;
    private Vector3 previousPlayerUpComponent;
    private Vector3 cameraRight;
    private Vector3 cameraUp;
    // Update is called once per frame

    private void Start()
    {
        previousPlayerRightComponent = Vector3.Project(playerTransform.position, cameraRight);
        previousPlayerUpComponent = Vector3.Project(playerTransform.position, cameraUp);
        cameraRight = transform.right;
        cameraUp = transform.up;
        _audioManagerObject = GameObject.Find("AudioManager");
    }

    /*
     * Gets projection of player on to camera's local coordinates.
     * Then checks if there is any change in that projections vector and moves the camera if there is.
     */ 
    void LateUpdate()
    {
        if(playerTransform == null)
        {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            if (!playerGO)
            {
                return;
            }
            playerTransform = playerGO.transform;
        }

        Vector3 playerRightComponent = Vector3.Project(playerTransform.position, cameraRight);
        Vector3 playerUpComponent = Vector3.Project(playerTransform.position, cameraUp);


        Vector3 deltaRightComponent = playerRightComponent - previousPlayerRightComponent;
        Vector3 deltaUpComponent = playerUpComponent - previousPlayerUpComponent;

        if (deltaRightComponent != Vector3.zero)
        {
            transform.position += deltaRightComponent;
        }
        if (deltaUpComponent != Vector3.zero)
        {
            transform.position += deltaUpComponent;
        }

        //Update the location of all audio sources to make sure they move with the camera
        _audioManagerObject.transform.position = transform.position;

        previousPlayerRightComponent = playerRightComponent;
        previousPlayerUpComponent = playerUpComponent;
    }
}
