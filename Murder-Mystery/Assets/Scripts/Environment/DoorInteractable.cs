using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteractable : Interactable
{
    [SerializeField] private Animator doorAnimatorRef;
    [SerializeField] private int sceneIndex;
    public override void OnInteraction()
    {
        StartCoroutine(OpenDoorAndLoadScene());
    }

    private IEnumerator OpenDoorAndLoadScene()
    {
        doorAnimatorRef.SetBool("IsOpen", true);
        Debug.Log("Door Opened");

        float doorAnimationDuration = GetDoorAnimationDuration(); // get the animation duration

        // Wait for the door animation to complete
        yield return new WaitForSeconds(doorAnimationDuration);

        SceneManager.LoadScene(sceneIndex);
    }

    private float GetDoorAnimationDuration()
    {
        return 2.0f;
    }
}
