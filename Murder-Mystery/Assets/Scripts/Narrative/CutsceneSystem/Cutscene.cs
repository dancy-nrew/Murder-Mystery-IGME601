using System.Collections.Generic;

public class Cutscene
{
    /* 
     * Cutscenes are Composite-pattern objects that defer the execution
     * of the cutscene script to the CutsceneAction that compose them. 
     * 
     * CutsceneActions are stored as a Queue, so any factory methods
     * that create Cutscenes can know the order the actions will execute in.
    */
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

    // Can be useful for setting flags for future actions
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
        // Loads the next action in the queue or sets itself to be over
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
