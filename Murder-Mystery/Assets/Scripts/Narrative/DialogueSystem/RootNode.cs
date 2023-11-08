using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adapted from https://youtu.be/nKpM98I7PeM?si=6_zO-Egnx1kB-9Ys
// Root node of a dialogue graph.
public class RootNode : Node
{
    public Node child;

    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override NodeState OnUpdate()
    {
        if(child.state != NodeState.Success)
        {
            return child.UpdateNode(dialogueTree);
        }
        
        return NodeState.Success; 
    }

    public override Node Clone(DialogueTree tree)
    {
        RootNode node = Instantiate(this);
        tree.nodes.Add(node);
        node.child = child.Clone(tree);
        return node;
    }
}
