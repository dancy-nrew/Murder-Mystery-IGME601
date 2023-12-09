using UnityEngine;

public abstract class CutsceneAction
{
    // All CutsceneAction subclasses are implementations of what is meant to happen in a cutscene
    // they can be as granular as needed but must inherit from this abstract class
    public abstract void PerformAction();
    public abstract void OnActionFinish();
    public abstract void SetFlag();
    protected abstract bool IsFlagSet();
}


public class StartCardDialogueAction : CutsceneAction
{
    // This action starts a card dialogue cutscene.
    // Its PerformAction loads the dialogue tree into the DialogueManager and gets the ball rolling
    public StartCardDialogueAction()
    {
        // This constructor is empty and only exists for compliance with the abstract class
    }

    public override void PerformAction()
    {
        DialogueManager.dCharactersFinishedTyping += OnActionFinish;
        CharacterSO character = GameManager.Instance.GetCharacterSOFromKey(GameManager.Instance.GetLastTalkedTo());
        DialogueManager.Instance.ShowDialogue(character.GetDialogueTree());
    }

    protected override bool IsFlagSet()
    {
        return false;
    }

    public override void SetFlag()
    {
        
    }

    public override void OnActionFinish()
    {
        // Once the initial sentence has finished typing, a signal to move to the next action is given
        DialogueManager.dCharactersFinishedTyping -= OnActionFinish;
        CutsceneManager.Instance.MoveToNextAction();
    }
}

public class PlaySFXAction : CutsceneAction
{
    // This action plays a sound effect, as specificied by the key given at construction
    string sfxKey;
    public PlaySFXAction(string key)
    {
        sfxKey = key;
    }

    public override void PerformAction()
    {
        AudioManager.Instance.PlaySFX(sfxKey);
        OnActionFinish();
    }

    protected override bool IsFlagSet()
    {
        return false;
    }

    public override void SetFlag()
    {

    }

    public override void OnActionFinish()
    {
        // Once the initial sentence has finished typing, a signal to move to the next action is given
        CutsceneManager.Instance.MoveToNextAction();
    }
}

public class LoadDialogueAndStart : CutsceneAction
{
    // This action starts a card dialogue cutscene.
    // Its PerformAction loads the dialogue tree into the DialogueManager and gets the ball rolling
    DialogueTree treeReference;
    public LoadDialogueAndStart(DialogueTree tree)
    {
        treeReference = tree;
    }

    public override void PerformAction()
    {
        DialogueManager.dCharactersFinishedTyping += OnActionFinish;
        DialogueManager.Instance.ShowDialogue(treeReference.Clone());
    }

    protected override bool IsFlagSet()
    {
        return false;
    }

    public override void SetFlag()
    {

    }

    public override void OnActionFinish()
    {
        // Once the initial sentence has finished typing, a signal to move to the next action is given
        DialogueManager.dCharactersFinishedTyping -= OnActionFinish;
        CutsceneManager.Instance.MoveToNextAction();
    }
}

public class DialogueAction : CutsceneAction
{
    // This action informs the dialogue tree that it must show the next sentence
    public DialogueAction()
    {

    }

    public override void PerformAction()
    {
        DialogueManager.dCharactersFinishedTyping += OnActionFinish;
        DialogueManager.Instance.DisplayNextSentence();
        
    }

    public override void OnActionFinish()
    {
        // On action finish, it informs the manager to load the next action in the scene script
        DialogueManager.dCharactersFinishedTyping -= OnActionFinish;
        CutsceneManager.Instance.MoveToNextAction();
    }


    // These classes are here to comply with the abstract class but they do nothing
    protected override bool IsFlagSet()
    {
        return false;
    }

    public override void SetFlag()
    {
        return;
    }
}

public class DialogueDataAction : CutsceneAction
{   
    //This action is in charge of setting a parameter in the DialogueDataWriter to a specific value
    string parameterToSet;
    bool parameterValue;
    public DialogueDataAction(string parameter)
    {
        //By default, parameters are set to false
        parameterToSet = parameter;
        parameterValue = false;
    }

    public override void PerformAction()
    {
        DialogueDataWriter.Instance.UpdateDialogueData(parameterToSet, IsFlagSet());
        OnActionFinish();
    }

    public override void SetFlag()
    {
        parameterValue = true;
    }

    protected override bool IsFlagSet()
    {
        return parameterValue;
    }

    public override void OnActionFinish()
    {
        CutsceneManager.Instance.MoveToNextAction();
    }
}

public class EndDialogueAction : CutsceneAction
{
    //This Action ends a dialogue by asking to move to the next sentence
    //The DialogueManager will call QueryTree at this stage, which in turn causes EndDialogue to be called
    public EndDialogueAction()
    {
    }

    public override void PerformAction()
    {
        DialogueManager.Instance.DisplayNextSentence();
        OnActionFinish();
    }

    public override void SetFlag()
    {
    }

    protected override bool IsFlagSet()
    {
        return false;
    }

    public override void OnActionFinish()
    {
        Debug.Log("OnActionFinish - End Dialogue Action");
        //This should essentially cause the cutscene to end
        CutsceneManager.Instance.MoveToNextAction();
    }
}

public class SetCardInteractivityAction : CutsceneAction
{
    bool freezeCards;
    public SetCardInteractivityAction()
    {
        freezeCards = false;
    }

    public override void PerformAction()
    {
        HandContainer player1HandContainer = GameObject.Find("ContainerPlayer1").GetComponent<HandContainer>();
        if (freezeCards)
        {
            player1HandContainer.FreezeCards();
        } else
        {
            player1HandContainer.UnfreezeCards();
        }
        OnActionFinish();
        
    }

    public override void OnActionFinish()
    {
        CutsceneManager.Instance.MoveToNextAction();
    }

    public override void SetFlag()
    {
        freezeCards = true;
    }

    protected override bool IsFlagSet()
    {
        return freezeCards;
    }
}

public class FreeSpecificCardAction : CutsceneAction
{
    int index;
    public FreeSpecificCardAction(int cardToFreeAsIndex)
    {
        index = cardToFreeAsIndex;
    }

    public override void PerformAction()
    {
        HandContainer player1HandContainer = GameObject.Find("ContainerPlayer1").GetComponent<HandContainer>();
        
        // Sneaky if statement to allow this action to also unfreeze everything
        if (index == -1)
        {
            player1HandContainer.UnfreezeCards();
        } else
        {
            player1HandContainer.UnfreezeByIndex(index);
        }
        OnActionFinish();
    }

    public override void OnActionFinish()
    {
        CutsceneManager.Instance.MoveToNextAction();
    }

    public override void SetFlag()
    {
        
    }

    protected override bool IsFlagSet()
    {
        return false;
    }
}

public class LockLanesAndFreeOneAction : CutsceneAction
{
    int lane;
    public LockLanesAndFreeOneAction(int laneMarker)
    {
        lane = laneMarker;
    }

    public override void PerformAction()
    {
        GameObject[] lanes = GameObject.FindGameObjectsWithTag("Lane");
        foreach (GameObject lane in lanes)
        {
            lane.layer = LayerMask.NameToLayer("Default");
        }
        // Sneaky if statement to allow this action to also unfreeze everything
        if (lane == -1)
        {
            foreach(GameObject lane in lanes)
            {
                lane.layer = LayerMask.NameToLayer("Ground");
            }
        }
        else
        {
            GameObject laneToFree = GameObject.Find("Lane"+lane.ToString());
            laneToFree.layer = LayerMask.NameToLayer("Ground");
        }
        OnActionFinish();
    }

    public override void OnActionFinish()
    {
        CutsceneManager.Instance.MoveToNextAction();
    }

    public override void SetFlag()
    {

    }

    protected override bool IsFlagSet()
    {
        return false;
    }
}

public class ShowRoundOverlayAction : CutsceneAction
{
    public ShowRoundOverlayAction() { }

    public override void PerformAction()
    {
        // The DialogueDeal function has ways of tracking which player to deal to and all that.
        BoardManager bm = GameObject.Find("SceneDataManager").GetComponent<BoardManager>();
        bm.DisplayRoundTitle();
        OnActionFinish();
    }

    public override void OnActionFinish()
    {
        CutsceneManager.Instance.MoveToNextAction();
    }

    public override void SetFlag()
    {
    }

    protected override bool IsFlagSet()
    {
        return false;
    }
}

public class RestoreNormalSceneOverlayAction : CutsceneAction
{
    public RestoreNormalSceneOverlayAction() { }

    public override void PerformAction()
    {
        // The DialogueDeal function has ways of tracking which player to deal to and all that.
        BoardManager bm = GameObject.Find("SceneDataManager").GetComponent<BoardManager>();
        bm.showingOverlayNormally = true;
        OnActionFinish();
    }

    public override void OnActionFinish()
    {
        CutsceneManager.Instance.MoveToNextAction();
    }

    public override void SetFlag()
    {
    }

    protected override bool IsFlagSet()
    {
        return false;
    }
}

public class DealDialogueCardsAction : CutsceneAction
{
    // This Action informs the hand factory it should deal cards
    HandFactory handFactoryReference;
    public DealDialogueCardsAction(HandFactory handFactory)
    {
        handFactoryReference = handFactory;
    }

    public override void PerformAction()
    {
        // The DialogueDeal function has ways of tracking which player to deal to and all that.
        handFactoryReference.DialogueDeal();
        OnActionFinish();
    }

    public override void OnActionFinish()
    {
        CutsceneManager.Instance.MoveToNextAction();
    }

    public override void SetFlag()
    {
    }

    protected override bool IsFlagSet()
    {
        return false;
    }

}

public class WaitAction : CutsceneAction
{

    //This action adds a little delay before the next action is called
    //Good for pauses or time between DialogueAction calls
    public WaitAction()
    {

    }

    public override void PerformAction()
    {
        CutsceneManager.Instance.DelayNext();
    }

    public override void OnActionFinish()
    {
        CutsceneManager.Instance.MoveToNextAction();
    }

    public override void SetFlag()
    {
    }

    protected override bool IsFlagSet()
    {
        return false;
    }
}