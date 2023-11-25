using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI_Controller : MonoBehaviour
{
    /*
        Class that defines the behavior of the AI opponent for the card battler.
        Currently only has one important Inspector option which is controlling
        whether the opponent behaves randomly or tries to make intelligent decisions.
     */
    public static AI_Controller Instance;
    private IAIStrategy _strategy;
    HandContainer hand;
    BoardManager _boardManager;
    BoardState _localState;
    public bool isRandomPlayer;

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

        (lane, index) = _strategy.DecideMove(_localState, hand.handData);
        
        // Physically move the card
        GameObject physicalCard = hand.PopCardObject(index);
        PlayToLane ptl = physicalCard.GetComponent<PlayToLane>();
        ptl.AnticipationMove(lane);
    }    
}
