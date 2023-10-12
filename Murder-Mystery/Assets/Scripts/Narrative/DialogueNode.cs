using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNode : Node
{
    public string dialogueText;

    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override NodeState OnUpdate()
    {
        return NodeState.Success;
    }
}
