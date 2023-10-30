using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueNode : Node
{
    public Dialogue dialogue;

   public List<DialogueData.DialogueParameter> gateConditions = new List<DialogueData.DialogueParameter>(); 
    
    protected override void OnStart()
    {
        bool result = true;
        foreach(var cond  in gateConditions)
        {
            result = result && (DialogueDataWriter.Instance.CheckCondition(cond.parameterKey, cond.parameterValue));
            if (result == false)
                return;
        }

        if(dialogueTree != null)
        {
            dialogueTree.dialogues.Add(dialogue);
        }
        //Debug.Log(characterName);
    }

    protected override void OnStop()
    {
        state = NodeState.Running;
    }

    protected override NodeState OnUpdate()
    {
        return NodeState.Success;
    }

}
