using System.Linq;
using System.Collections.Generic;
using UnityEngine;
public enum AITypes
{
    Random,
    Informed,
    Scripted
}

public interface IAIStrategy
{
    // Interface for an AI Strategy. All AI Strategies need to implement this public method
    // They are meant to return a tuple that represent the lane and the index of the card to play
    public (int, int) DecideMove(BoardState _localState, HandData hand);
}

public class RandomAI : IAIStrategy
{
    public (int, int) DecideMove(BoardState _localState, HandData hand)
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
        int index = UnityEngine.Random.Range(0, hand.cards.Count);
        hand.PopCard(index);
        return (lane, index);
    }
}

public class InformedAI : IAIStrategy
{
    public (int, int) DecideMove(BoardState _localState, HandData hand)
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
        if (missing_lanes.Count > 0)
        {

            int lane_to_win = 0;
            int winning_index = 0;
            int highest_delta = 0;
            int lane_current_value = 0;
            int delta = 0;
            bool at_least_one_winning_lane = false;

            foreach (int _lane in missing_lanes)
            {

                for (int i = 0; i < hand.cards.Count; i++)
                {
                    
                    card = hand.cards[i];
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
            }

            if (at_least_one_winning_lane)
            {
                hand.PopCard(winning_index);
                return (lane_to_win, winning_index);
            }
        }

        returnable_lane = _localState.GetPlayerWeakestLane();
        int weakest_lane_val = _localState.GetLaneValue(ConstantParameters.PLAYER_2, returnable_lane);
        int new_value = 0;
        int pop_index = 0;
        for (int i = 0; i < hand.cards.Count; i++)
        {
            card = hand.cards[i];
            new_value = _localState.TestNewLaneValue(returnable_lane, card);
            if (new_value > weakest_lane_val)
            {
                weakest_lane_val = new_value;
                pop_index = i;
            }
        }

        hand.PopCard(pop_index);
        return (returnable_lane, pop_index);
    }
}

public class ScriptedAI : IAIStrategy
{
    //Very simple scripted AI controller. Can be extended but
    //we have to figure out a way of doing so without compromising the interface
    //Untested so far
    private int _currentTurn = 0;

    public (int, int) DecideMove(BoardState _localState, HandData hand)
    {
        // Moves are decided by doing one card in each lane
        // and cards are played in order. Nothing fancy.
        int card, lane;
        lane = _currentTurn%3 + 1;
        card = 0;
        _currentTurn++;
        return (lane, card);
    }
}

public static class AIStrategyFactory
{
    /*
        Factory class that creates AI Strategies to set for the AI controller.
     */
    public static IAIStrategy CreateStrategy(AITypes type)
    {
        switch (type)
        {
            case AITypes.Informed:
                return new InformedAI();
            case AITypes.Scripted:
                return new ScriptedAI();
            default:
                return new RandomAI();
        }
    }
}