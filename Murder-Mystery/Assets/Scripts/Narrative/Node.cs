using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : ScriptableObject
{
    public enum NodeState
    {
        Running,
        Failure,
        Success
    }

    [HideInInspector]
    public NodeState state = NodeState.Running;
    [HideInInspector]
    public bool started = false;
    [HideInInspector]
    public string guid;
    [HideInInspector]
    public Vector2 position;

    public NodeState UpdateNode()
    {
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
    }

    public virtual Node Clone()
    {
        return Instantiate(this);
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract NodeState OnUpdate();

}
