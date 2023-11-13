using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * This class reperesents the data of a dialogue node.
 */
public class DialogueNode : Node
{
    public Dialogue dialogue;

    public List<DialogueData.DialogueParameter> gateConditions = new List<DialogueData.DialogueParameter>();

    public List<DialogueData.DialogueParameter> dataUpdates = new List<DialogueData.DialogueParameter>();

    /*
     * If gate conditions are met, set currentDialogue to this dialogue.
     */
    protected override void OnStart()
    {
        bool result = true;
        
        foreach(var cond  in gateConditions)
        {
            result = result && (DialogueDataWriter.Instance.CheckCondition(cond.parameterKey, cond.parameterValue));
            if (result == false)
                return;
        }

        if (dialogueTree != null)
        {
            dialogueTree.currentDialogue = dialogue;

            foreach (var cond in dataUpdates)
            {
                DialogueDataWriter.Instance.UpdateDialogueData(cond.parameterKey, cond.parameterValue);
            }
        }
    }

    protected override void OnStop()
    {

    }

    protected override NodeState OnUpdate()
    {
        return NodeState.Success;
    }

}
