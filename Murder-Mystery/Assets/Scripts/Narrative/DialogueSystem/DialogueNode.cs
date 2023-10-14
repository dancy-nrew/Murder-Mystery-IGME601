using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNode : Node
{
    public Dialogue dialogue;

    protected override void OnStart()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
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
