using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI_Controller : MonoBehaviour
{
    /*
        Class that defines the controller for the AI opponent for the card battler.
        Behavior algorithm is referenced as a strategy that is composited into this object
        thereby encapsulating the decision behavior and allowing this object to add
        independent of the algorithm implementation
     */
    public static AI_Controller Instance;
    private IAIStrategy _strategy;
    HandContainer hand;
    BoardManager _boardManager;
    BoardState _localState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        hand = gameObject.GetComponent<HandContainer>();
        _boardManager = GameObject.Find("SceneDataManager").GetComponent<BoardManager>();
    }


    // Event delegation is lifetime static, so must move out of start/destroy methods
    private void OnEnable()
    {
        // Subscribe to the board manager receiving game player moves to prepare AI move.
        BoardManager.OnPlay += MakeMove;
    }

    private void OnDisable()
    {
        // Unsubscribe on disable to clean up after scene is done
        BoardManager.OnPlay -= MakeMove;
    }

    public void SetStrategy(IAIStrategy strategy)
    {
        //Sets the AI strategy this controller will use. This is the brains of the operation.
        _strategy = strategy;
    }

    public void MakeMove()
    {
        /*
            Triggered function from the board receiving a player move.
            This sets off the AI to decide how to perform a move and play.
        */
        _localState = _boardManager.boardState.Clone();
        int lane;
        int index;

        //Strategies are defined in the Strategy factory
        (lane, index) = _strategy.DecideMove(_localState, hand.handData);
        
        // Physically move the card
        GameObject physicalCard = hand.PopCardObject(index);
        PlayToLane ptl = physicalCard.GetComponent<PlayToLane>();
        ptl.AnticipationMove(lane);
    }    
}
