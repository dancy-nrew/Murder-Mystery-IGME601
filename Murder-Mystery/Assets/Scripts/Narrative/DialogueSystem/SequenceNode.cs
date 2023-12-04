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
        
    }

    protected override void OnStop()
    {
        
    }

    protected override NodeState OnUpdate()
    {
        current = 0;
        while (current < children.Count)
        {
            Node child = children[current];
            if(child.state == NodeState.Success)
            {
                current++;
                continue;
            }
            else
            {
                child.UpdateNode(dialogueTree);
                break;
            }
            
        }

        return current == children.Count ? NodeState.Success : NodeState.Running;

    }

    public override Node Clone(DialogueTree tree)
    {
        SequenceNode node = Instantiate(this);
        tree.nodes.Add(node);
        node.children = children.ConvertAll(c => c .Clone(tree));
        return node;
    }

    protected override void UpdateNodeState()
    {
        current = 0;
        while (current < children.Count)
        {
            Node child = children[current];
            if (child.state != NodeState.Success)
            {
                state = NodeState.Running;
                break;
            }
            current++;
        }

        if(current == children.Count)
        {
            state = NodeState.Success;
        }
        
    }
}
