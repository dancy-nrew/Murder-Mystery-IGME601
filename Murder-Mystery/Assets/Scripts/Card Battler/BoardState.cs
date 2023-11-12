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
        // Constructor for the board state. Inputs are which lanes are currently locked for the player, where cards can't be played.

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
        /* Give a set that indicates which lanes are available for player to play.
        
        Inputs:
        Player - int representing the player
        */
        HashSet<int> result = new HashSet<int>();

        if (player == ConstantParameters.PLAYER_2)
        {
            // The adversary can never have blocked lanes
            result.Add(ConstantParameters.LANE_1);
            result.Add(ConstantParameters.LANE_2);
            result.Add(ConstantParameters.LANE_3);
            return result;
        }

        // Check to see if locks are off for player 1
        if (!lane1_lock)
        {
            result.Add(ConstantParameters.LANE_1);
        }
        if (!lane2_lock)
        {
            result.Add(ConstantParameters.LANE_2);
        }
        if (!lane3_lock)
        {
            result.Add(ConstantParameters.LANE_3);
        }

        return result;
    }

    public HashSet<int> GetUnclaimedLanesForPlayer(int player)
    {
        /* Get a set of the lanes a player doens't own yet.
        Inputs:
        Player - int representing the player
        Outputs:
        A HashSet of integers that represents the lanes not owned by the player
        */
        HashSet<int> result = new HashSet<int>();
        if (lane1_winner != player)
        {
            result.Add(ConstantParameters.LANE_1);
        }
        if (lane2_winner != player)
        {
            result.Add(ConstantParameters.LANE_2);
        }
        if (lane3_winner != player)
        {
            result.Add(ConstantParameters.LANE_3);
        }
        return result;
    }

    private int DecideLaneVictor(int score1, int score2)
    {
        // Return an integer that indicates which player is currently winning a lane
        // Takes the scores of both players as input, returns the winning player or 0 in case of a tie
        if (score1 > score2)
        {
             return ConstantParameters.PLAYER_1;
        }
        else if (score2 > score1)
        {
            return ConstantParameters.PLAYER_2;
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
        // Evaluate each lane and then see which player has the most lanes.
        // Outputs the winning player as an integer
        List<int> p1Lanes = new List<int>();
        List<int> p2Lanes = new List<int>();

        // Lane 1 evaluation
        if (lane1_winner == ConstantParameters.PLAYER_1)
        {
            p1Lanes.Add(ConstantParameters.LANE_1);
        } else if (lane1_winner == ConstantParameters.PLAYER_2)
        {
            p2Lanes.Add(ConstantParameters.LANE_1);
        }

        // Lane 2 evaluation
        if (lane2_winner == ConstantParameters.PLAYER_1)
        {
            p1Lanes.Add(ConstantParameters.LANE_2);
        }
        else if (lane2_winner == ConstantParameters.PLAYER_2)
        {
            p2Lanes.Add(ConstantParameters.LANE_2);
        }

        // Lane 3 evaluation
        if (lane3_winner == ConstantParameters.PLAYER_1)
        {
            p1Lanes.Add(ConstantParameters.LANE_3);
        }
        else if (lane3_winner == ConstantParameters.PLAYER_2)
        {
            p2Lanes.Add(ConstantParameters.LANE_3);
        }

        // Determine the winner
        if (p1Lanes.Count > p2Lanes.Count) {
            return ConstantParameters.PLAYER_1;
        } else if (p2Lanes.Count > p1Lanes.Count)
        {
            return ConstantParameters.PLAYER_2;
        }
        else
        {
            return 0;
        }
    }

    public void PlayerAddCardToLane(int player, CardData card, int lane_index)
    {
        /* Add a card to a lane as played by a player
        Inputs:
        Player - The player playing the card, represented by an int
        Card - The data for the card being played
        Lane_Index - The index for the lane for lookup in the board state data store
        */ 

        List<HandData> laneData;
        if (player == ConstantParameters.PLAYER_1)
        {
            laneData = player1_lanes;
        } else
        {
            laneData = player2_lanes;
        }
        laneData[lane_index].AddCard(card);
    }

    public int GetCardsInLaneForPlayer(int player, int lane_index){
        /* Get the count of the cards a lane has for a particular player
        Inputs:

        Lane Index - An int to know which lane to lookup in the lists
        Player - the side of the board to which consult
        */
        HandData laneContainer;
        if (player == ConstantParameters.PLAYER_1){
            laneContainer = player1_lanes[lane_index];
        } else {
            laneContainer = player2_lanes[lane_index];
        }

        return laneContainer.cards.Count;
    }

    public int GetLaneValue(int player, int lane)
    {
        int lane_index = lane - 1;
        HandData laneContainer;
        if (player == ConstantParameters.PLAYER_1)
        {
            laneContainer = player1_lanes[lane_index];
        } else
        {
            laneContainer = player2_lanes[lane_index];
        }
        return laneContainer.value;
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
        willWin = DecideLaneVictor(laneValue, opponentValue) == ConstantParameters.PLAYER_2;

        // Remove the last added card to reset the simulation
        laneContainer.PopCard(laneContainer.cards.Count-1);

        return willWin;
    }

    // Simulate what value a lane will have if a certain card is played
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


    // Evaluate which lane is the weakest for the player.
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
