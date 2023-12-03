using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class CutsceneManager : MonoBehaviour
{
    /*
    The cutscene manager is the only monobehavior class in all of the cutscene system.
    It should be added to scenes as a prefab and can statically manage the cutscene system
    via its instance class.

    A scene can have many cutscenes, so the Cutscene manager uses a two-level Composite-pattern
    to handle cutscenes. In the first level, all Cutscenes are composite objects themselves, stored
    in a queue. Cutscenes are then traversed in a regular queue structure and the CutsceneManager
    is in charge of making sure they operate and move correctly. 
    
    The second level Composite pattern is the Cutscene itself, see more info on the cutscene class
    or on the Technical Architecture part of the design document.
     */
    Queue<Cutscene> cutscenes;
    Cutscene activeCutscene;
    public float delayTimer;
    public delegate void CutsceneEnd();
    public static CutsceneEnd dCutsceneEndSignal;
    public DialogueTree activeConversationTree;
    [SerializeField] public BoardManager boardManager;
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
        // If for whatever reason an action needs to affect the state
        // of a future action, this is an auxiliary method to do so.
        if (activeCutscene != null)
        {
            activeCutscene.SetFlagForNext();
        }
    }

    // Basic Composite-traversal functions for both Cutscene objects and CutsceneAction objects
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
        } else
        {
            Debug.Log("Move To Next Cutscene called and no cutscenes are queued");
        }
    }

    public void MoveToNextAction()
    {
        activeCutscene.NextAction();
        if (activeCutscene.isOver)
        {
            CutsceneHasEnded();
            boardManager.DisplayRoundTitle(1);
        }
    }

    // End of Cutscene events are important and triggers can be tied to this signal
    public void CutsceneHasEnded()
    {
        if (dCutsceneEndSignal != null)
        {
            Debug.Log("Call Cutscene End Signal");
            dCutsceneEndSignal();
        }
    }

    // Delay operations for the WaitActions
    public void DelayNext()
    {
        StartCoroutine(DelayCoroutine());
    }

    IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(delayTimer);
        MoveToNextAction();
    }


    // Debug Methods, implemented but currently unused. Leaving this for future reference
    public void DebugTree(DialogueTree tree)
    {
        activeConversationTree = tree;
    }
}
