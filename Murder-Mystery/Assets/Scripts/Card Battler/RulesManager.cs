using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesManager : MonoBehaviour
{
    int current_turn = 0;
    bool game_ongoing = true;
    public void RunTurn(int winner)
    {
        current_turn++;

        if (current_turn >= ConstantParameters.MAX_TURNS){
            game_ongoing = false;
        }

        // Sudden Death rules until no more cards in hand
        if (!game_ongoing && winner == 0 && current_turn < ConstantParameters.MAX_HAND_SIZE)
        {
            game_ongoing = true;
        }

        if (!game_ongoing)
        {
            // The game has stopped.
            HandContainer player1 = GameObject.Find("ContainerPlayer1").GetComponent<HandContainer>();
            player1.FreezeCards();

            // Report results to UI
            GUIManager gm = gameObject.GetComponent<GUIManager>();
            gm.DisplayGameEndMessage(winner);
            gm.ShowButton();
        }
    }    
}
