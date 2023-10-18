using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardState
{
    int lane1_winner = 0;
    int lane2_winner = 0;
    int lane3_winner = 0;

    bool lane1_lock;
    bool lane2_lock;
    bool lane3_lock;

    List<HandData> player1_lanes;
    List<HandData> player2_lanes;

    public BoardState(bool lock_l1, bool lock_l2, bool lock_l3)
    {
        player1_lanes = new List<HandData> {
            new HandData(ConstantParameters.MAX_HAND_SIZE, true),
            new HandData(ConstantParameters.MAX_HAND_SIZE, true),
            new HandData(ConstantParameters.MAX_HAND_SIZE, true)
        };
        player2_lanes = new List<HandData>
        {
            new HandData(ConstantParameters.MAX_HAND_SIZE, true),
            new HandData(ConstantParameters.MAX_HAND_SIZE, true),
            new HandData(ConstantParameters.MAX_HAND_SIZE, true)
        };
        lane1_lock = lock_l1;
        lane2_lock = lock_l2;
        lane3_lock = lock_l3;
    }

    public BoardState Clone()
    {
        // For the AI we don't want them to reference the original board, but rather, have an
        // internal representation of it, so that it can run its simulations without affecting
        // the actual game board. That's why cloning doesn't return a reference to the original
        // but a rather an entirely new object.
        BoardState bs = new BoardState(this.lane1_lock, this.lane2_lock, this.lane3_lock);
        for (int i = 0; i < player1_lanes.Count; i++)
        {
            // Make a copy of the cards in the original
            bs.player1_lanes[i].SetCards(player1_lanes[i].CopyCards());
            bs.player2_lanes[i].SetCards(player2_lanes[i].CopyCards());

            // Calculate the lane
            bs.player1_lanes[i].SetValue(player1_lanes[i].value);
            bs.player2_lanes[i].SetValue(player2_lanes[i].value);
        }

        return bs;
    }

    public HashSet<int> GetAvailableLanesForPlayer(int player)
    {
        HashSet<int> result = new HashSet<int>();

        if (player == 2)
        {
            // The adversary can never have blocked lanes
            result.Add(1);
            result.Add(2);
            result.Add(3);
            return result;
        }

        // Check to see if locks are off for player 1
        if (!lane1_lock)
        {
            result.Add(1);
        }
        if (!lane2_lock)
        {
            result.Add(2);
        }
        if (!lane3_lock)
        {
            result.Add(3);
        }

        return result;
    }

    public HashSet<int> GetUnclaimedLanesForPlayer(int player)
    {
        HashSet<int> result = new HashSet<int>();
        if (lane1_winner != player)
        {
            result.Add(1);
        }
        if (lane2_winner != player)
        {
            result.Add(2);
        }
        if (lane3_winner != player)
        {
            result.Add(3);
        }
        return result;
    }

    private int DecideLaneVictor(int score1, int score2)
    {
        if (score1 > score2)
        {
             return 1;
        }
        else if (score2 > score1)
        {
            return 2;
        }

        return 0;
    }

    public void CalculateBoardValues()
    {
        // For each lane, calculate each side and check to see who's winning
        int l1p1_total_score = player1_lanes[0].CalculateHandValue();
        int l1p2_total_score = player2_lanes[0].CalculateHandValue();
        lane1_winner = DecideLaneVictor(l1p1_total_score, l1p2_total_score);

        int l2p1_total_score = player1_lanes[1].CalculateHandValue();
        int l2p2_total_score = player2_lanes[1].CalculateHandValue();
        lane2_winner = DecideLaneVictor(l2p1_total_score, l2p2_total_score);

        int l3p1_total_score = player1_lanes[2].CalculateHandValue();
        int l3p2_total_score = player2_lanes[2].CalculateHandValue();
        lane3_winner = DecideLaneVictor(l3p1_total_score, l3p2_total_score);
    }

    public int GetGameWinner()
    {
        List<int> p1Lanes = new List<int>();
        List<int> p2Lanes = new List<int>();

        // Lane 1 evaluation
        if (lane1_winner == 1)
        {
            p1Lanes.Add(1);
        } else if (lane1_winner == 2)
        {
            p2Lanes.Add(1);
        }

        // Lane 2 evaluation
        if (lane2_winner == 1)
        {
            p1Lanes.Add(2);
        }
        else if (lane2_winner == 2)
        {
            p2Lanes.Add(2);
        }

        // Lane 3 evaluation
        if (lane3_winner == 1)
        {
            p1Lanes.Add(3);
        }
        else if (lane3_winner == 2)
        {
            p2Lanes.Add(3);
        }

        // Determine the winner
        if (p1Lanes.Count > p2Lanes.Count) {
            return 1;
        } else if (p2Lanes.Count > p1Lanes.Count)
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }

    public void PlayerAddCardToLane(int player, CardData card, int lane)
    {
        int index = lane - 1;
        List<HandData> laneData;
        if (player == 1)
        {
            laneData = player1_lanes;
        } else
        {
            laneData = player2_lanes;
        }
        laneData[index].AddCard(card);

    }

    #region AI Support Methods

    //Simulate if action will result in lane win
    public bool TestLaneWin(int lane, CardData card)
    {
        bool willWin;
        int index = lane - 1;
        int laneValue;
        int opponentValue;
        HandData laneContainer = player2_lanes[index];
        HandData opponentLane = player1_lanes[index];

        opponentValue = opponentLane.value;
        laneContainer.AddCard(card);
        laneValue = laneContainer.CalculateHandValue();
        willWin = DecideLaneVictor(laneValue, opponentValue) == 1;

        // Remove the last added card to reset the simulation
        laneContainer.PopCard(laneContainer.cards.Count-1);

        return willWin;
    }
    public int TestNewLaneValue(int lane, CardData card)
    {
        int index = lane - 1;
        HandData laneContainer = player2_lanes[index];
        laneContainer.AddCard(card);
        int laneValue = laneContainer.CalculateHandValue();
        
        //Remove the last added card to reset the simulation
        laneContainer.PopCard(laneContainer.cards.Count - 1);
        return laneValue;
    }

    public int GetPlayerWeakestLane()
    {
        int minValue = 999;
        int lane = 0;
        for (int i = 0; i < player2_lanes.Count; i++)
        {
            if (player2_lanes[i].value < minValue)
            {
                minValue = player2_lanes[i].value;
                lane = i + 1;
            }
        }
        return lane;
    }
    #endregion
}
