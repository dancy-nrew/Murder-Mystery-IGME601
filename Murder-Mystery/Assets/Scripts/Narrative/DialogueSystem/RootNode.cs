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
        state = NodeState.Running;
    }

    protected override NodeState OnUpdate()
    {
        return child.UpdateNode();
    }

    public override Node Clone()
    {
        RootNode node = Instantiate(this);
        node.child = child.Clone();
        return node;
    }
}
