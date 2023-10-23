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
        dialogueData.UpdateParameter(parameter, value);
    }
}
