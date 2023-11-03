using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputNode : Node
{
    public Dialogue dialogue;
    public List<Node> children = new List<Node>();
    public List<string> choices = new List<string>();
    protected override void OnStart()
    {
        if (dialogueTree != null)
        {
            dialogueTree.dialogues.Add(dialogue);
            dialogueTree.bIsInputting = true;
            dialogueTree.inputtingNode = this;
        }
       
    }

    protected override void OnStop()
    {
        state = NodeState.Running;
    }

    protected override NodeState OnUpdate()
    {
        return NodeState.Success;
    }

    public override Node Clone()
    {
        InputNode node = Instantiate(this);
        node.children = children.ConvertAll(c => c.Clone());
        return node;
    }
}
