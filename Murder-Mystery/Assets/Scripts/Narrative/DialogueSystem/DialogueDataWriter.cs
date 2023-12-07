using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

public class DialogueDataWriter : MonoBehaviour
{
    public static DialogueDataWriter Instance { get; private set; }

    public DialogueData dialogueData;

    public delegate void DParameterUpdated(string paramName, bool paramValue);
    public DParameterUpdated dParameterUpdated;

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
        Debug.Log("Updating paramter in data writer");
        dParameterUpdated?.Invoke(parameter, value);
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

    public List<DialogueData.DialogueParameter> GetParameters()
    {
        if (!dialogueData)
        {
            Debug.Log("No DialoguData asset set in DialogueWriter");
            return null;
        }

        return dialogueData.parameters;
    }
    public void SetDialogueDataAsset(DialogueData dialogueDataAsset)
    {
        dialogueData = dialogueDataAsset;
    }
}
