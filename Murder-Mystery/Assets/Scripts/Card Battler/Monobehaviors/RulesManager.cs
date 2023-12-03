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

        if (!game_ongoing)
        {
            // Report results to UI
            CharacterSO.ECharacter chr = GameManager.Instance.GetLastTalkedTo();
            CutsceneManager.Instance.AddCutscene(CutsceneFactory.MakeCardBattleOutroCutscene(chr, winner));
            CutsceneManager.dCutsceneEndSignal += ShowExitButton;
            CutsceneManager.Instance.MoveToNextCutscene();
        }
    }

    public void ShowExitButton()
    {
        GUIManager gm = gameObject.GetComponent<GUIManager>();
        gm.ShowButton();
    }

    private void OnDisable()
    {
        CutsceneManager.dCutsceneEndSignal -= ShowExitButton;   
    }
}
