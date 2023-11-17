using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * IN PROGRESS CLASS
 */
public class InputNode : Node
{

    public List<Node> children = new List<Node>();
    public List<string> choices = new List<string>();
    [HideInInspector]
    public int choice = -1;
    protected override void OnStart()
    {
        if (dialogueTree != null && choice == -1)
        {
            dialogueTree.bIsInputting = true;
            dialogueTree.currentInputNode = this;
        }
       
    }

    protected override void OnStop()
    {
        if(choice == -1)
        {
            state = NodeState.Running;
        }
    }

    protected override NodeState OnUpdate()
    {
        if(choice == -1)
        {
            return NodeState.Success;
        }
        else
        {
            return children[choice].UpdateNode(dialogueTree);
        }
        
    }

    public override Node Clone(DialogueTree tree)
    {
        InputNode node = Instantiate(this);
        tree.nodes.Add(node);
        node.children = children.ConvertAll(c => c.Clone(tree));
        return node;
    }
}
