using UnityEngine;

public class RulesManager : MonoBehaviour
{
    /*
        This class is in charge of making sure the game rules are followed.
        Currently it makes sure the turns run and evaluates when the game must end.
     */
    int current_turn = 0;
    bool game_ongoing = true;
    public void RunTurn(int winner)
    {
        /*
            This function keeps track of the current turn we're in. It will stop
            the game when the current turn has reached the established maximum, but has
            support for sudden death in case of a tie.

            Inputs:
                - Winner - int that indicates which player is currently winning the game. 
                            Only really important at the last turn but nothing 
                            is wasted by passing it before the game ends.
         */
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
