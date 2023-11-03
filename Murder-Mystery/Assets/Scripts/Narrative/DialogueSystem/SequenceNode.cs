using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Adapted from https://youtu.be/nKpM98I7PeM?si=6_zO-Egnx1kB-9Ys
// This node will run each of its children one-by-one.
public class SequenceNode : Node
{

    int current;
    public List<Node> children;

    protected override void OnStart()
    {
        current = 0;
    }

    protected override void OnStop()
    {
        
    }

    protected override NodeState OnUpdate()
    {
        while(current < children.Count)
        {
            Node child = children[current];
            /*switch (child.UpdateNode(dialogueTree))
            {
                case NodeState.Running:
                    return NodeState.Running;
                case NodeState.Failure:
                    return NodeState.Failure;
                case NodeState.Success:
                    current++;
                    break;
            }*/

            child.UpdateNode(dialogueTree);
            if (dialogueTree.bIsInputting)
                return NodeState.Success;

            current++;
        }
       

        return current == children.Count ? NodeState.Success : NodeState.Running;

    }

    public override Node Clone()
    {
        SequenceNode node = Instantiate(this);
        node.children = children.ConvertAll(c => c .Clone());
        return node;
    }
}
