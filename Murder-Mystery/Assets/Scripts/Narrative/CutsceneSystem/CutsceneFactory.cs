public static class CutsceneFactory
{
    public static Cutscene MakeCardBattlerIntroCutscene(HandFactory handFactory)
    {
        Cutscene cutscene = new Cutscene();

        cutscene.AddAction(new StartCardDialogueAction());

        // First person speaks on Motive Clue
        cutscene.AddAction(new DialogueAction());
        cutscene.AddAction(new DealDialogueCardsAction(handFactory));

        //Second person speaks on Motive Clue
        cutscene.AddAction(new DialogueAction());
        cutscene.AddAction(new DealDialogueCardsAction(handFactory));

        //First person speaks on Location Clue
        cutscene.AddAction(new DialogueAction());
        cutscene.AddAction(new DealDialogueCardsAction(handFactory));

        //Second person speaks on Location Clue
        cutscene.AddAction(new DialogueAction());
        cutscene.AddAction(new DealDialogueCardsAction(handFactory));

        //First person speaks on Witness Clue
        cutscene.AddAction(new DialogueAction());
        cutscene.AddAction(new DealDialogueCardsAction(handFactory));

        //Second person speaks on Witness Clue
        cutscene.AddAction(new DialogueAction());
        cutscene.AddAction(new DealDialogueCardsAction(handFactory));

        return cutscene;
    }
}
