using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteractable : Interactable
{
    public string doorKey;
    public Transform doorDropOff;

    [SerializeField] private Animator doorAnimatorRef;
    [SerializeField] private int sceneIndex;
    [SerializeField] private float doorAnimationDuration;
    public override void OnInteraction()
    {
        GameManager.Instance.SaveDoorInfo(doorKey);
        StartCoroutine(OpenDoorAndLoadScene());
    }

    private IEnumerator OpenDoorAndLoadScene()
    {
        doorAnimatorRef.SetBool("IsOpen", true);
        Debug.Log("Door Opened");


        // Wait for the door animation to complete
        yield return new WaitForSeconds(doorAnimationDuration);

        SceneManager.LoadScene(sceneIndex);
    }
}

   
