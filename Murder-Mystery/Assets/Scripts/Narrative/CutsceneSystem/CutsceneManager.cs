using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class CutsceneManager : MonoBehaviour
{
    Queue<Cutscene> cutscenes;
    Cutscene activeCutscene;
    public float delayTimer;
    public delegate void CutsceneEnd();
    public static CutsceneEnd dCutsceneEndSignal;
    public DialogueTree activeConversationTree;
    public static CutsceneManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        cutscenes = new Queue<Cutscene>();
    }

    public void SetFlagForNextAction()
    {
        if (activeCutscene != null)
        {
            activeCutscene.SetFlagForNext();
        }
    }

    public void AddCutscene(Cutscene cutscene)
    {
        cutscenes.Enqueue(cutscene);
    }

    public void MoveToNextCutscene()
    {
        if (cutscenes.Count > 0)
        {
            activeCutscene = cutscenes.Dequeue();
            activeCutscene.NextAction();
        }
    }

    public void MoveToNextAction()
    {
        activeCutscene.NextAction();
        if (activeCutscene.isOver)
        {
            CutsceneHasEnded();
        }
    }

    public void CutsceneHasEnded()
    {
        if (dCutsceneEndSignal != null)
        {
            dCutsceneEndSignal();
        }
    }

    public void DelayNext()
    {
        StartCoroutine(DelayCoroutine());
    }

    IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(delayTimer);
        MoveToNextAction();
    }

    public void DebugTree(DialogueTree tree)
    {
        activeConversationTree = tree;
    }
}
