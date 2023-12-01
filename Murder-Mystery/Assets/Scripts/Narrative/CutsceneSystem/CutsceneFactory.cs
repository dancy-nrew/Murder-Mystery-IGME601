public static class CutsceneFactory
{
    //Static class for producing Cutscenes. Configurations for the different cutscenes can be created
    //here, though there might be more elegant ways of defining cutscene templates.
    private static void AddDialogueAndWait(Cutscene cutscene, CutsceneAction action)
    {
        cutscene.AddAction(action);
        cutscene.AddAction(new WaitAction());
    }

    private static void AddCardDealingActions(Cutscene cutscene, HandFactory handFactory)
    {
        // First person speaks on clue
        AddDialogueAndWait(cutscene, new DialogueAction());
        //Second person speaks on clue
        AddDialogueAndWait(cutscene, new DialogueAction());

        //If we want to add an info message on "you got good cards" or whatev, we can do that here.

        // Deal Cards
        cutscene.AddAction(new DealDialogueCardsAction(handFactory));
        //Deal Cards
        cutscene.AddAction(new DealDialogueCardsAction(handFactory));
    }
    public static Cutscene MakeCardBattlerIntroCutscene(HandFactory handFactory, CharacterSO.ECharacter chr)
    {
        Cutscene cutscene = new Cutscene();
        AddDialogueAndWait(cutscene, new StartCardDialogueAction());
        
        // Set both of these parameters to false
        DialogueDataAction setEndToFalse = new DialogueDataAction("bEndOf" + chr + "CardBattle");
        setEndToFalse.SetFlag();
        DialogueDataAction setWinToFalse = new DialogueDataAction("bHasWon" + chr + "CardBattle");
        setWinToFalse.SetFlag();
        cutscene.AddAction(setEndToFalse);
        cutscene.AddAction(setWinToFalse);
        AddDialogueAndWait(cutscene, new DialogueAction());

        //Motive
        AddCardDealingActions(cutscene, handFactory);
        //Location
        AddCardDealingActions(cutscene, handFactory);
        //Witness
        AddCardDealingActions(cutscene, handFactory);

        //Exit the dialogue
        cutscene.AddAction(new EndDialogueAction());
        return cutscene;
    }

    public static Cutscene MakeCardBattleOutroCutscene(CharacterSO.ECharacter chr)
    {
        Cutscene cutscene = new Cutscene();

        AddDialogueAndWait(cutscene, new StartCardDialogueAction());
        AddDialogueAndWait(cutscene, new DialogueAction());
        AddDialogueAndWait(cutscene, new DialogueAction());

        // Set Dialogue Flag back to false
        DialogueDataAction bEndCardBattle = new DialogueDataAction("bEndOf" + chr + "CardBattle");
        cutscene.AddAction(bEndCardBattle);

        //Exit the dialogue
        cutscene.AddAction(new EndDialogueAction());
        return cutscene;
    }
}
