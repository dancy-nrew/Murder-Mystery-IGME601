using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDataWriter : MonoBehaviour
{
    public static DialogueDataWriter Instance { get; private set; }

    public DialogueData dialogueData;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void UpdateDialogueData(string parameter, bool value)
    {
        if(!dialogueData)
        {
            Debug.Log("No DialoguData asset set in DialogueWriter");
            return;
        }

        dialogueData.UpdateParameter(parameter, value);
    }

    public bool CheckCondition(string parameter, bool value) 
    {
        if (!dialogueData)
        {
            Debug.Log("No DialoguData asset set in DialogueWriter");
            return false;
        }

        return dialogueData.CheckCondition(parameter, value);
    }

    public void SetDialogueDataAsset(DialogueData dialogueDataAsset)
    {
        dialogueData = dialogueDataAsset;
    }
}
