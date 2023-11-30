using UnityEngine;

public abstract class CutsceneAction
{
    public abstract void PerformAction();
    public abstract void OnActionFinish();
    public abstract void SetFlag();
    protected abstract bool IsFlagSet();
}

public class StartCardDialogueAction : CutsceneAction
{
    public StartCardDialogueAction()
    {

    }

    public override void PerformAction()
    {
        CharacterSO character = GameManager.Instance.GetCharacterSOFromKey(GameManager.Instance.GetLastTalkedTo());
        DialogueManager.Instance.ShowDialogue(character.cardBattleDialogueTree);

        DialogueManager.dCharactersFinishedTyping += OnActionFinish;

        CharacterSO character = GameManager.Instance.GetCharacterSOFromKey(GameManager.Instance.GetLastTalkedTo());
        DialogueTree tree = character.GetDialogueTree();
        CutsceneManager.Instance.DebugTree(tree);
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
        DialogueManager.dCharactersFinishedTyping -= OnActionFinish;
        CutsceneManager.Instance.MoveToNextAction();
    }
}

public class DialogueAction : CutsceneAction
{
    public DialogueAction()
    {

    }

    public override void PerformAction()
    {
        DialogueManager.Instance.DisplayNextSentence();
        DialogueManager.dCharactersFinishedTyping += OnActionFinish;

        Debug.Log("Displaying Next Sentence");
        DialogueManager.Instance.DisplayNextSentence();
        
    }

    public override void OnActionFinish()
    {
        DialogueManager.dCharactersFinishedTyping -= OnActionFinish;
        if (DialogueDataWriter.Instance.CheckCondition("bDealCards", true))
        {
            CutsceneManager.Instance.SetFlagForNextAction();
        }
        CutsceneManager.Instance.MoveToNextAction();
    }

    protected override bool IsFlagSet()
    {
        return false;
    }

    public override void SetFlag()
    {
        return;
    }
}

public class DealDialogueCardsAction : CutsceneAction
{
    bool cardDealFlag;
    HandFactory handFactoryReference;
    public DealDialogueCardsAction(HandFactory handFactory)
    {
        handFactoryReference = handFactory;
        cardDealFlag = false;
    }

    public override void PerformAction()
    {
        Debug.Log("Performing Card Dealing");
            handFactoryReference.DialogueDeal();
            OnActionFinish();
        }
    }

    public override void OnActionFinish()
    {
        // Set bDealCards to false
        CutsceneManager.Instance.MoveToNextAction();
    }

    public override void SetFlag()
    {
        cardDealFlag = true;
    }

    protected override bool IsFlagSet()
    {
        return cardDealFlag;
    }

}

public class WaitAction : CutsceneAction
{
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