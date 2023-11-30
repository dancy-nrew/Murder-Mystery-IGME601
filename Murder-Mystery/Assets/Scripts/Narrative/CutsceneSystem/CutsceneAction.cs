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
        if (cardDealFlag)
        {
            handFactoryReference.DialogueDeal();
            OnActionFinish();
        }
    }

    public override void OnActionFinish()
    {
        // Set bDealCards to false
        DialogueDataWriter.Instance.UpdateDialogueData("bDealCards", false);
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
    private float _delay;
    public WaitAction(float delayTime)
    {
        _delay = delayTime;
    }

    public override void PerformAction()
    {
        
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