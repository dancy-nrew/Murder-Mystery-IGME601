public static class CutsceneFactory
{
    //Static class for producing Cutscenes. Configurations for the different cutscenes can be created
    //here, though there might be more elegant ways of defining cutscene templates.
    private static void AddDialogueAndWait(Cutscene cutscene, CutsceneAction action)
    {
        cutscene.AddAction(action);
        cutscene.AddAction(new WaitAction());
    }

    private static void AddOverlayAction(Cutscene cutscene)
    {
        cutscene.AddAction(new ShowRoundOverlayAction());
        cutscene.AddAction(new WaitAction());
    }

    private static void AddCardDealingActions(Cutscene cutscene, HandFactory handFactory)
    {
        // First person speaks on clue
        AddDialogueAndWait(cutscene, new DialogueAction());
        //Second person speaks on clue
        AddDialogueAndWait(cutscene, new DialogueAction());
        
        //If we want to add an info message on "you got good cards" or whatev, we can do that here.
        AddDialogueAndWait(cutscene, new DialogueAction());


        cutscene.AddAction(new PlaySFXAction("aCardDeal"));
        // Deal Cards
        cutscene.AddAction(new DealDialogueCardsAction(handFactory));
        //Deal Cards
        cutscene.AddAction(new DealDialogueCardsAction(handFactory));
        SetCardInteractivityAction freezeCardsAction = new SetCardInteractivityAction();
        freezeCardsAction.SetFlag();
        cutscene.AddAction(freezeCardsAction);

    }
    public static Cutscene MakeCardBattleScriptedMiniScene(DialogueTree treeForScene, int cardToUnfreeze, int lanesToUnfreeze)
    {
        Cutscene cutscene = new Cutscene();
        cutscene.AddAction(new WaitAction());
        SetCardInteractivityAction freezeCards = new SetCardInteractivityAction();
        freezeCards.SetFlag();
        cutscene.AddAction(freezeCards);
        AddOverlayAction(cutscene);
        AddDialogueAndWait(cutscene, new LoadDialogueAndStart(treeForScene));
        AddDialogueAndWait(cutscene, new DialogueAction());
        AddDialogueAndWait(cutscene, new DialogueAction());
        cutscene.AddAction(new FreeSpecificCardAction(cardToUnfreeze));
        cutscene.AddAction(new LockLanesAndFreeOneAction(lanesToUnfreeze));

        //Exit the dialogue
        cutscene.AddAction(new EndDialogueAction());
        return cutscene;
    }
    public static Cutscene MakeCardBattlerIntroCutscene(HandFactory handFactory, CharacterSO.ECharacter chr)
    {
        // Set both of these parameters to false
        DialogueDataAction setEndToFalse = new DialogueDataAction("bEndOf" + chr + "CardBattle");
        DialogueDataAction setWinToFalse = new DialogueDataAction("bHasWon" + chr + "CardBattle");
        Cutscene cutscene = new Cutscene();
        cutscene.AddAction(setEndToFalse);
        cutscene.AddAction(setWinToFalse);

        AddDialogueAndWait(cutscene, new StartCardDialogueAction());
        AddDialogueAndWait(cutscene, new DialogueAction());

        //Motive
        AddCardDealingActions(cutscene, handFactory);
        //Location
        AddCardDealingActions(cutscene, handFactory);
        //Witness
        AddCardDealingActions(cutscene, handFactory);

        //Unfreeze cards
        cutscene.AddAction(new SetCardInteractivityAction());

        //Exit the dialogue
        cutscene.AddAction(new EndDialogueAction());
        return cutscene;
    }

    public static Cutscene MakeCardBattleOutroCutscene(CharacterSO.ECharacter chr, int battleWinner) 
    {

        Cutscene cutscene = new Cutscene();

        //Set end of battle flags
        DialogueDataAction endBattleFlagAction = new DialogueDataAction("bEndOf" + chr + "CardBattle");
        endBattleFlagAction.SetFlag();
        DialogueDataAction hasWonBattleFlagAction = new DialogueDataAction("bHasWon" + chr + "CardBattle");
        if (battleWinner == ConstantParameters.PLAYER_1)
        {
            hasWonBattleFlagAction.SetFlag();
        }

        // Apply flags
        cutscene.AddAction(endBattleFlagAction);
        cutscene.AddAction(hasWonBattleFlagAction);
        // Compose Dialogue
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
