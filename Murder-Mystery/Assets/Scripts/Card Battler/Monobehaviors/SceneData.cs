using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{
    public Vector3 Lane1Transform;
    public Vector3 Lane2Transform;
    public Vector3 Lane3Transform;
    public HandFactory handFactory;

    [SerializeField]
    public List<DialogueTree> scriptedSequenceTrees;

    private void Start()
    {
        SetUpGame();
    }

    private void SetUpGame()
    {
        // Set up the data for the card battle mini game.
        // This function is in charge of setting up the deal strategies for the game
        // As well as loading the AI decision engine for the type of game being played


        List<int> setupData = new List<int>();
        IAIStrategy aiStrategy;
        CharacterSO.ECharacter lastTalkedTo = GameManager.Instance.GetLastTalkedTo();

        if (lastTalkedTo == CharacterSO.ECharacter.Connor )
        {
            //To Do: Implement and test scripted sequence

            //Scripted Sequence
            //Ace's Cards
            int[] cardsToDealAce = { 
                6,8,7,8,6,9,3,4,5
            };

            //Connor's Cards
            int[] cardsToDealConnor = {
                3,8,9,7,5,7,5,4,3
            };

            int[] indecesToFree =
            {
                0,1,3,2,4,-1
            };
            int[] lanesToFree =
            {
                1,3,3,2,1,-1
            };
            aiStrategy = AIStrategyFactory.CreateStrategy(AITypes.Scripted);
            handFactory.strategyIdentifier = DealStrategies.Deterministic;
            handFactory.AssignAndSetupStrategy(new List<int>(cardsToDealAce), ConstantParameters.PLAYER_1);
            handFactory.AssignAndSetupStrategy(new List<int>(cardsToDealConnor), ConstantParameters.PLAYER_2);
            CutsceneManager.Instance.AddCutscene(CutsceneFactory.MakeCardBattlerIntroCutscene(handFactory, lastTalkedTo));
            for (int i = 0; i < scriptedSequenceTrees.Count; i++)
            {
                Cutscene scene = CutsceneFactory.MakeCardBattleScriptedMiniScene(scriptedSequenceTrees[i], indecesToFree[i], lanesToFree[i]);
                if (i == scriptedSequenceTrees.Count - 1)
                {
                    // Add an action to restore the regular round showing thingy
                    scene.AddAction(new RestoreNormalSceneOverlayAction());
                }
                CutsceneManager.Instance.AddCutscene(scene);
            }

            CutsceneManager.Instance.MoveToNextCutscene();
            RulesManager rm = GetComponent<RulesManager>();
            GetComponent<BoardManager>().showingOverlayNormally = false;
            CutsceneManager.dCutsceneEndSignal += rm.PlayConnorScript;

        } else if (lastTalkedTo != CharacterSO.ECharacter.Ace)
        {

            // Load clue data
            CharacterSO charSO = GameManager.Instance.GetCharacterSOFromKey(GameManager.Instance.GetLastTalkedTo());
            setupData.Add(DialogueDataWriter.Instance.CheckCondition(charSO.motiveParameter, true) ? 1 : 0);
            setupData.Add(DialogueDataWriter.Instance.CheckCondition(charSO.locationParameter, true) ? 1 : 0);
            setupData.Add(DialogueDataWriter.Instance.CheckCondition(charSO.witnessParameter, true) ? 1 : 0);
            handFactory.strategyIdentifier = DealStrategies.ClueBased;

            // Deal Clue Based cards to Player
            handFactory.AssignAndSetupStrategy(setupData, ConstantParameters.PLAYER_1);

            // Deal the inverse Cards to AI
            setupData.Clear();
            setupData.Add(DialogueDataWriter.Instance.CheckCondition(charSO.motiveParameter, true) ? 0 : 1);
            setupData.Add(DialogueDataWriter.Instance.CheckCondition(charSO.locationParameter, true) ? 0 : 1);
            setupData.Add(DialogueDataWriter.Instance.CheckCondition(charSO.witnessParameter, true) ? 0 : 1);
            handFactory.AssignAndSetupStrategy(setupData, ConstantParameters.PLAYER_2);
            aiStrategy = AIStrategyFactory.CreateStrategy(AITypes.Informed);

            //Add the Cutscene and start it
            CutsceneManager.Instance.AddCutscene(CutsceneFactory.MakeCardBattlerIntroCutscene(handFactory, lastTalkedTo));
            CutsceneManager.dCutsceneEndSignal += GetComponent<BoardManager>().EnableGUI;
            CutsceneManager.Instance.MoveToNextCutscene();

        } else
        {
            // Ace was the last talked-to character, which is impossible. Thus:
            Debug.Log("Random Strategy");
            handFactory.strategyIdentifier = DealStrategies.Random;
            handFactory.AssignAndSetupStrategy(setupData, ConstantParameters.PLAYER_1);
            handFactory.AssignAndSetupStrategy(setupData, ConstantParameters.PLAYER_2);
            handFactory.DealHand(ConstantParameters.PLAYER_1);
            handFactory.DealHand(ConstantParameters.PLAYER_2);
            aiStrategy = AIStrategyFactory.CreateStrategy(AITypes.Random);
        }

        // Load the decided strategy into the AI and add an outro cutscene
        AI_Controller.Instance.SetStrategy(aiStrategy);
        
    }

    public Vector3 GetLaneTransform(int lane){
        // Provide the relevant transform information for the lane objects
        // Input is the lane as an integer
        Vector3 transform;

        if (lane == ConstantParameters.LANE_1){
            transform = Lane1Transform;
        } else if (lane == ConstantParameters.LANE_2){
            transform = Lane2Transform;
        } else {
            transform = Lane3Transform;
        }
        return transform;
    }
    
}
