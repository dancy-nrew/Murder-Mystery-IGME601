public static class CutsceneFactory
{

    private static void AddDialogueAndWait(Cutscene cutscene)
    {
        cutscene.AddAction(new DialogueAction());
        cutscene.AddAction(new WaitAction());
    }

    private static void AddCardDealingActions(Cutscene cutscene, HandFactory handFactory)
    {
        // First person speaks on clue
        AddDialogueAndWait(cutscene);
        //Second person speaks on clue
        AddDialogueAndWait(cutscene);
        
        //If we want to add an info message on "you got good cards" or whatev, we can do that here.

        // Deal Cards
        cutscene.AddAction(new DealDialogueCardsAction(handFactory));
        cutscene.AddAction(new DealDialogueCardsAction(handFactory));
    }
    public static Cutscene MakeCardBattlerIntroCutscene(HandFactory handFactory)
    {
        Cutscene cutscene = new Cutscene();

        cutscene.AddAction(new StartCardDialogueAction());
        cutscene.AddAction(new WaitAction());
        AddDialogueAndWait(cutscene);

        //Motive
        AddCardDealingActions(cutscene, handFactory);
        //Location
        AddCardDealingActions(cutscene, handFactory);
        //Witness
        AddCardDealingActions(cutscene, handFactory);

        return cutscene;
    }
}
