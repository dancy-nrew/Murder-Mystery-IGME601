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
    HandContainer hand;
    BoardManager _boardManager;
    BoardState _localState;
    public bool isRandomPlayer;
    private void Start()
    {
        hand = gameObject.GetComponent<HandContainer>();
        _boardManager = GameObject.Find("SceneDataManager").GetComponent<BoardManager>();

        // Subscribe to the board manager receiving game player moves to prepare AI move.
        BoardManager.OnPlay += DecideMove;
    }

    public void DecideMove()
    {
        /*
            Triggered function from the board receiving a player move.
            This sets off the AI to decide how to perform a move and play.
        */
        _localState = _boardManager.boardState.Clone();
        int lane;
        CardData card;
        int index;

        // Make the choice
        if (isRandomPlayer)
        {
            (lane, index, card) = MakeRandomMove();
        } else
        {
            (lane, index, card) = MakeInformedMove();
        }

        // Physically move the card
        GameObject physicalCard = hand.PopCardObject(index);
        PlayToLane ptl = physicalCard.GetComponent<PlayToLane>();
        ptl.AnticipationMove(lane);
    }

    private (int, int, CardData) MakeRandomMove()
    {
        /*
            Decides on a random move to make. A random move consists of choosing a random lane
            and a random card in hand and playing that.

            Outputs:
            lane - int indicating which lane to play the card to
            card - which card to play
         */
        HashSet<int> available_lanes = _localState.GetAvailableLanesForPlayer(ConstantParameters.PLAYER_2);
        int[] lanes_array = available_lanes.ToArray();
        int lane = lanes_array[UnityEngine.Random.Range(0, lanes_array.Length)];
        HandData handData = hand.handData;
        int index = UnityEngine.Random.Range(0, handData.cards.Count);
        CardData card = handData.PopCard(index);
        return (lane, index, card);
    }

    private (int, int, CardData) MakeInformedMove()
    {
        /*
            Makes an informed move, as defined by the following algorithm:
            1. Test to see if any lane can be won
                1.1 For each lane, iterate over every card in hand
                1.2 If a lane can be won, record which lane it was, what card was used and how many points the move would add
                1.3 Search over the entire lane-card space and maximize the value added to a lane
                1.4 After search, if a lane can be won, return the highest value play.
            2. If no lane can be won, reinforce weakest lane to set up possible lane-winning move next turn
                2.1 Identify weakest lane
                2.2 Iterate over every card in hand, recording the new value on every test
                2.3 Play the card that will increase lane value the most

            Outputs:
                lane - int to indicate which lane to play to
                card - card data to indicate which card to play
         */
        int returnable_lane;
        CardData card;

        HashSet<int> missing_lanes = _localState.GetUnclaimedLanesForPlayer(ConstantParameters.PLAYER_2);
        bool can_win_lane = false;
        
        if (missing_lanes.Count> 0)
        {
            int lane_to_win = 0;
            int winning_index = 0;
            int highest_delta = 0;
            int lane_current_value = 0;
            int delta = 0;
            bool at_least_one_winning_lane = false;

            foreach(int _lane in missing_lanes)
            {
                for (int i = 0; i < hand.handData.cards.Count; i++)
                {
                    card = hand.handData.cards[i];
                    lane_current_value = _localState.GetLaneValue(ConstantParameters.PLAYER_2, _lane);
                    delta = _localState.TestNewLaneValue(_lane, card) - lane_current_value;
                    can_win_lane = _localState.TestLaneWin(_lane, card);
                    if (can_win_lane && delta > highest_delta)
                    {
                        at_least_one_winning_lane = true;
                        winning_index = i;
                        lane_to_win = _lane;
                        highest_delta = delta;
                    }
                }

                if (at_least_one_winning_lane)
                {
                    card = hand.handData.PopCard(winning_index);
                    return (lane_to_win, winning_index, card);
                }
            }
        }

        returnable_lane = _localState.GetPlayerWeakestLane();
        int weakest_lane_val = _localState.GetLaneValue(ConstantParameters.PLAYER_2, returnable_lane);
        int new_value = 0;
        int pop_index = 0;
        for (int i = 0; i < hand.handData.cards.Count; i++)
        {
            card = hand.handData.cards[i];
            new_value = _localState.TestNewLaneValue(returnable_lane, card);
            if (new_value > weakest_lane_val)
            {
                weakest_lane_val = new_value;
                pop_index = i;
            }
        }
        card = hand.handData.PopCard(pop_index);
        return (returnable_lane, pop_index, card);
    }

    
}
