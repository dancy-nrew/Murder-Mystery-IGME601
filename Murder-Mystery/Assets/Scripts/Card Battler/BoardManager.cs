using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardManager : MonoBehaviour
{
    /*
        This class manages the state of the board. It is interacted with by other classes
        and will make sure the Rules Manager knows when cards have been played.
     */

    // TO DO: need to add how to load in lane locks
    public BoardState boardState = new BoardState(false, false, false);
    public delegate void AIMove();
    public static event AIMove OnPlay;
    private RulesManager rm;
    public TMP_Text laneOnePlayerScore;
    public TMP_Text laneTwoPlayerScore;
    public TMP_Text laneThreePlayerScore;
    public TMP_Text laneOneAIScore;
    public TMP_Text laneTwoAIScore;
    public TMP_Text laneThreeAIScore;

    private void Awake()
    {
        // Cache reference for the Rules Manager
        rm = gameObject.GetComponent<RulesManager>();
    }

    public void PlayCardToLane(int player, int lane, CardData card){
        // Have a player play a card to a lane. Refer to BoardState for more details.
        // This function changes the lane number into the index value by subtracting one.
        
        // If the player is making a move, inform the AI it's time to make a move too
        // The AI should decide it's own action before the player's move affects the score on the board
        if (player == ConstantParameters.PLAYER_1)
        {
            if (OnPlay != null)
            {
                OnPlay();
            }
        }

        // Update the state of the board
        boardState.PlayerAddCardToLane(player, card, lane-1);

        if (player == ConstantParameters.PLAYER_1)
        {
            // Because the AI plays before the human player, if the human player has
            // affected the board, it means the turn is over.
            // Get scores for each lane
            boardState.CalculateBoardValues();
            GetLaneValues();
            int game_winner = boardState.GetGameWinner();
            rm.RunTurn(game_winner);
        }

    }

    public int GetCardsInLaneForPlayer(int player, int lane){
        // Return the number of cards in a lane for a given player
        // The lane number is transformed into the lane index by subtracting one
        // Refer to BoardState for more details
        return boardState.GetCardsInLaneForPlayer(player, lane-1);
    }

    public void GetLaneValues()
    {
        int lane1ScorePlayer1 = boardState.GetLaneValue(ConstantParameters.PLAYER_1, ConstantParameters.LANE_1);
        int lane2ScorePlayer1 = boardState.GetLaneValue(ConstantParameters.PLAYER_1, ConstantParameters.LANE_2);
        int lane3ScorePlayer1 = boardState.GetLaneValue(ConstantParameters.PLAYER_1, ConstantParameters.LANE_3);

        int lane1ScorePlayer2 = boardState.GetLaneValue(ConstantParameters.PLAYER_2, ConstantParameters.LANE_1);
        int lane2ScorePlayer2 = boardState.GetLaneValue(ConstantParameters.PLAYER_2, ConstantParameters.LANE_2);
        int lane3ScorePlayer2 = boardState.GetLaneValue(ConstantParameters.PLAYER_2, ConstantParameters.LANE_3);

        laneOnePlayerScore.text = lane1ScorePlayer1.ToString();
        laneTwoPlayerScore.text = lane2ScorePlayer1.ToString();
        laneThreePlayerScore.text = lane3ScorePlayer1.ToString();
        laneOneAIScore.text = lane1ScorePlayer2.ToString();
        laneTwoAIScore.text = lane2ScorePlayer2.ToString();
        laneThreeAIScore.text = lane3ScorePlayer2.ToString();
    }
}
