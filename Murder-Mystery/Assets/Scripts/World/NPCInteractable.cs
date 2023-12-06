using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Responsible for NPC interactions.
 */
public class NPCInteractable : Interactable
{
    [SerializeField]
    public CharacterSO.ECharacter character;

    protected override void Awake()
    {
        base.Awake();
        dialogueTreeRunner = GetComponent<DialogueTreeRunner>();
    }

    /*
     * Run Dialogue Tree on interaction.
     */
    public override void OnInteraction()
    {
        if (dialogueTreeRunner == null)
            return;

        Debug.Log("Calling Dialouge tree runner");
        GameManager.Instance.SetLastTalkedTo(character);
        GameManager.Instance.StorePlayerLastLocation();
        GameManager.Instance.SetLastVisitedScene(SceneManagers.GetCurrentScene());
        dialogueTreeRunner.UpdateTree();
    }

}
