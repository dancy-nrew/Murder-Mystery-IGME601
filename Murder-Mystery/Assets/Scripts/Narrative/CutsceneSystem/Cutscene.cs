using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene
{
    Queue<CutsceneAction> actions;
    CutsceneAction activeAction;
    public bool isOver;

    public Cutscene()
    {
        actions = new Queue<CutsceneAction>();
        isOver = false;
    }

    public void AddAction(CutsceneAction action)
    {
        actions.Enqueue(action);
    }

    public CutsceneAction AuditNextAction()
    {
        return actions.Peek();
    }

    public void SetFlagForNext()
    {
        CutsceneAction nextAction = actions.Peek();
        nextAction.SetFlag();
    }

    public void NextAction()
    {
        if (actions.Count > 0)
        {
            activeAction = actions.Dequeue();
            activeAction.PerformAction();
        } else
        {
            isOver = true;
        }
    }

}
