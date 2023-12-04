using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adapted from https://youtu.be/nKpM98I7PeM?si=6_zO-Egnx1kB-9Ys
// Base class of all nodes.
public abstract class Node : ScriptableObject
{
    [Serializable]
    public enum NodeState
    {
        Running,
        Failure,
        Success
    }

    public NodeState state = NodeState.Running;
    [HideInInspector]
    public bool started = false;
    [HideInInspector]
    public string guid;
    [HideInInspector]
    public Vector2 position;

    protected DialogueTree dialogueTree;



   /* public NodeState UpdateNode(DialogueTree dialogueTree)
    {
        this.dialogueTree = dialogueTree;
        if(!started)
        {
            OnStart();
            started = true;
        }

        state = OnUpdate();

        if(state == NodeState.Failure || state == NodeState.Success)
        {
            OnStop();
            started = false;
        }

        return state;
    }*/

    public NodeState UpdateNode(DialogueTree dialogueTree)
    {
        this.dialogueTree = dialogueTree;
        if (!started)
        {
            OnStart();
            started = true;
        }
        
        state = OnUpdate();

        UpdateNodeState();

        if (state == NodeState.Failure || state == NodeState.Success)
        {
            OnStop();
            started = false;
        }

        return state;
    }

    public virtual Node Clone(DialogueTree tree)
    {
        Node node =  Instantiate(this);
        tree.nodes.Add(node);
        return node;
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract NodeState OnUpdate();

    protected virtual void UpdateNodeState()
    {

    }

}
