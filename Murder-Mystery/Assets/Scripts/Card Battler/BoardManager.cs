using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // TO DO: need to add how to load in lane locks
    BoardState boardState = new BoardState(false, false, false);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCardToLane(int player, int lane, CardData card){
        // Have a player play a card to a lane. Refer to BoardState for more details.
        // This function changes the lane number into the index value by subtracting one.
        boardState.PlayerAddCardToLane(player, card, lane-1);
    }

    public int GetCardsInLaneForPlayer(int player, int lane){
        // Return the number of cards in a lane for a given player
        // The lane number is transformed into the lane index by subtracting one
        // Refer to BoardState for more details
        return boardState.GetCardsInLaneForPlayer(player, lane-1);
    }
}
