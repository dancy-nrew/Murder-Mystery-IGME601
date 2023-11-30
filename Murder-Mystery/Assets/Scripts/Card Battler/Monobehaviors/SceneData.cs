using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneData : MonoBehaviour
{
    public Vector3 Lane1Transform;
    public Vector3 Lane2Transform;
    public Vector3 Lane3Transform;
    public HandFactory handFactory;

    private void Start()
    {
        SetUpGame();
    }

    private void SetUpGame()
    {
        List<int> setupData = new List<int>();
        IAIStrategy aiStrategy;

        if (GameManager.Instance.GetLastTalkedTo() == CharacterSO.ECharacter.Ace )
        {
            //Scripted Sequence
            //Ace's Cards
            int[] cardsToDealAce= { 
                6,7,8,8,6,9,3,4,5
            };

            //Connor's Cards
            int[] cardsToDealConnor = {
                3,8,9,4,5,7,5,5,3,4
            };
            aiStrategy = AIStrategyFactory.CreateStrategy(AITypes.Scripted);
            handFactory.strategyIdentifier = DealStrategies.Deterministic;
            handFactory.AssignAndSetupStrategy(new List<int>(cardsToDealAce), ConstantParameters.PLAYER_1);
            handFactory.AssignAndSetupStrategy(new List<int>(cardsToDealConnor), ConstantParameters.PLAYER_2);
            handFactory.DealHand(ConstantParameters.PLAYER_1);
            handFactory.DealHand(ConstantParameters.PLAYER_2);

        } else if (GameManager.Instance.GetLastTalkedTo() == CharacterSO.ECharacter.Connor)
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

            //Add the Cutscene and start it?
            CutsceneManager.Instance.AddCutscene(CutsceneFactory.MakeCardBattlerIntroCutscene(handFactory));
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
