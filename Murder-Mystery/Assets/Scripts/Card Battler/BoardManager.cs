using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // TO DO: need to add how to load in lane locks
    public BoardState boardState = new BoardState(false, false, false);
    public delegate void AIMove();
    public static event AIMove OnPlay;


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

            int game_winner = boardState.GetGameWinner();
            RulesManager rm = gameObject.GetComponent<RulesManager>();
            rm.RunTurn(game_winner);
        }

    }

    public int GetCardsInLaneForPlayer(int player, int lane){
        // Return the number of cards in a lane for a given player
        // The lane number is transformed into the lane index by subtracting one
        // Refer to BoardState for more details
        return boardState.GetCardsInLaneForPlayer(player, lane-1);
    }
}
