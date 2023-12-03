using System;
using System.Collections.Generic;
using UnityEngine;

public class HandFactory : MonoBehaviour
{
    public HandContainer Player1Hand;
    public HandContainer Player2Hand;
    public GameObject cardPrefab;
    public DealStrategies strategyIdentifier;
    private IDealStrategy dealStrategyPlayer1;
    private IDealStrategy dealStrategyPlayer2;
    public List<Material> witnessFaces;
    public List<Material> locationFaces;
    public List<Material> motiveFaces;
    public float xOffset;
    public float xOrigin;
    public float zOrigin;
    public float zOffset;
    public int maxFaceValue;
    public int minFaceValue;

    private int dialogueDealCounter;

    [SerializeField]
    public Vector3 scaleFactor;

    public void AssignAndSetupStrategy(List<int> setupData, int player)
    {
        // Assign the strategy object and set it up
        if (player == 1)
        {
            dealStrategyPlayer1 = HandDealStrategyFactory.CreateStrategy(strategyIdentifier);
            dealStrategyPlayer1.SetUp(setupData);
        } else
        {
            dealStrategyPlayer2 = HandDealStrategyFactory.CreateStrategy(strategyIdentifier);
            dealStrategyPlayer2.SetUp(setupData);
        }
        
    }

    public void DealCardsInSuit(int player, int n, Suit suit)
    {
        /*
            This function deals a specific amount of cards of a specific suit, according to the deal strategy of this factory. 
            It instantiates the card objects and assigns them to the HandContainer for the respective player.

            Inputs:
            player - the player to whom we'll deal cards to
            n - the amount of cards we'll deal.
            suit - the suit we're picking
         */
        HandContainer handToDealTo;
        IDealStrategy dealStrategy;
        //Determine if dealing to bottom player or top player
        int zMod = 1;
        if (player == ConstantParameters.PLAYER_1) { 
            handToDealTo = Player1Hand;
            dealStrategy = dealStrategyPlayer1;
        }
        else { 
            handToDealTo = Player2Hand; 
            zMod *= -1;
            dealStrategy = dealStrategyPlayer2;
        }

        List<int> dealtCardsOfThisSuit = new List<int>();

        for (int i = 0; i < n; i++)
        {
            int chosenCard = dealStrategy.GetCard(suit, dealtCardsOfThisSuit);
            GameObject instantiatedCard = CreateCardObject(handToDealTo, zMod, chosenCard, suit);
            PrepareCardObject(instantiatedCard, player);
            handToDealTo.ReceiveCard(instantiatedCard);
            handToDealTo.MoveToHand(instantiatedCard, zOrigin * zMod);
        }
    }
    public void DealCards(int player, int n)
    {
        /*
            This function deals a specific amount of cards, according to the deal strategy of this factory. 
            It instantiates the card objects and assigns them to the HandContainer for the respective player.

            Inputs:
            player - the player to whom we'll deal cards to
            n - the amount of cards we'll deal.
         */
        HandContainer handToDealTo;
        IDealStrategy dealStrategy;
        //Determine if dealing to bottom player or top player
        int zMod = 1;
        if (player == ConstantParameters.PLAYER_1) { 
            handToDealTo = Player1Hand;
            dealStrategy = dealStrategyPlayer1;
        }
        else { 
            handToDealTo = Player2Hand; 
            zMod *= -1;
            dealStrategy = dealStrategyPlayer2;
        }

        List<int> dealtWitnessCards = new List<int>();
        List<int> dealtLocationCards = new List<int>();
        List<int> dealtMotiveCards = new List<int>();

        for (int i = 0; i < n; i++)
        {
            (Suit suit, int faceValue) = CreateCardData(handToDealTo, dealStrategy, dealtWitnessCards, dealtLocationCards, dealtMotiveCards);
            GameObject instantiatedCard = CreateCardObject(handToDealTo, zMod, faceValue, suit);
            PrepareCardObject(instantiatedCard, player);
            handToDealTo.ReceiveCard(instantiatedCard);
            handToDealTo.MoveToHand(instantiatedCard, zOrigin * zMod);
        }
    }

    public void DealHand(int player)
    {
        /*
            This function deals a full set of cards to a player's hand. 

            Inputs:
            player - int to determine which player we're dealing with.
        */
        DealCards(player, ConstantParameters.MAX_HAND_SIZE);
    }

    private void PrepareCardObject(GameObject card, int player)
    {
        // Scale the card
        card.transform.localScale = new Vector3(
                card.transform.localScale.x * scaleFactor.x,
                card.transform.localScale.y * scaleFactor.y,
                card.transform.localScale.z * scaleFactor.z
            );

        // Rotate and alter the card
        if (player == ConstantParameters.PLAYER_2)
        {
            card.transform.rotation *= Quaternion.Euler(0, 0, 180f);
        }
        else
        {
            Card cardComponent = card.GetComponent<Card>();
            cardComponent.MakeInteractable();
            card.transform.rotation *= Quaternion.Euler(0, 180f, 0);
        }
    }

    private List<int> PickRelevantList(Suit suit, List<int> dealtWitnessCards, List<int> dealtLocationCards, List<int> dealtMotiveCards)
    {
        // Pick the list of dealt indeces that will be relevant for the card creation function
        switch (suit)
        {
            case Suit.LOCATION:
                return dealtLocationCards;
            case Suit.MOTIVE:
                return dealtMotiveCards;
            default:
                return dealtWitnessCards;
        }
    }

    private (Suit, int) CreateCardData(HandContainer handToDealTo, IDealStrategy dealStrategy, List<int> dealtWitnessCards, List<int> dealtLocationCards, List<int> dealtMotiveCards)
    {
        // Create the card data
        int i = handToDealTo.GetCurrentHandSize();
        Suit suit = dealStrategy.SelectSuit(i);
        List<int> relevantList = PickRelevantList(suit, dealtWitnessCards, dealtLocationCards, dealtMotiveCards);
        int chosenCard = dealStrategy.GetCard(suit, relevantList);
        return (suit, chosenCard);
    }

    private GameObject CreateCardObject(HandContainer handToDealTo, int zMod, int chosenCard, Suit suit)
    {
        // Create the GameObject for this card
        int i = handToDealTo.GetCurrentHandSize();
        Vector3 targetPosition = handToDealTo.gameObject.transform.position;
        float zAdjust = (zOffset * (i % 2) + zOrigin) * zMod;
        Vector3 instantiateLocation = new Vector3(targetPosition.x + (xOffset * i) + xOrigin, targetPosition.y, targetPosition.z + zAdjust);
        GameObject instantiatedCard = Instantiate(cardPrefab, instantiateLocation, Quaternion.identity);

        // Set the data for the card
        Card cardComponent = instantiatedCard.GetComponent<Card>();
        cardComponent.SetCardData(chosenCard, suit);
        cardComponent.SetFrontFaceMaterial(_GetMaterial(chosenCard, suit));
        
        return instantiatedCard;
    }
    private int _ValueToMaterialIndex(int value)
    {
        // Face values start at a minimum value and indeces in the list start from 0.
        // Therefore, subtracting the minvalue will make the ranges equivalent
        return value - minFaceValue;
    }

    private Material _GetMaterial(int value, Suit suit)
    {
        /*
         This function selects the material to apply to a card prefab face from the list of
         textures that exist for the cards. It then returns that material to the calling function

        Inputs:
        - Face value of the card
        - Suit value of the card

        Outputs:
        Material to apply to card face
         */
        List<Material> faces;
        if (suit == Suit.WITNESS)
        {
            faces = witnessFaces;
        } else if (suit == Suit.LOCATION) {
            faces = locationFaces;
        } else
        {
            faces = motiveFaces;
        }

        return faces[_ValueToMaterialIndex(value)];
    }

    public void DialogueDeal()
    {
        /*
         This function is in charge of dealing cards during the regular dialogue cutscene

        It does some basic arithmetic to determine who to deal to. The logic is as follows:
            - Calculate which clue must be dealt. We change clues every two times we deal cards (one for the player, one
            for the adversary), so we divide the deal counter by two.
            - Determine whether this is the second dialogue box. This will be used to determine which
                player to deal to, as it depends on the status of whether the player has found the clue
            - We deal to the player in the following exclusive OR scenario:
                - Either the player has a clue and we're dealing to the first side (so we deal to the player)
                - OR the player doesn't have a clue and we're dealing to the second side (so, again, we deal to the player)
        At the end we increase the deal counter, to keep track of where we are with dealing cards.
         */
        CharacterSO charSO = GameManager.Instance.GetCharacterSOFromKey(GameManager.Instance.GetLastTalkedTo());
        int clueToCheck = dialogueDealCounter / 2;

        bool isSecondTalk = dialogueDealCounter % 2 == 1;
        bool isMissingClue;
        int whoToDealTo;
        Suit suitToDeal;
        if (clueToCheck == 0)
        {
            isMissingClue = DialogueDataWriter.Instance.CheckCondition(charSO.motiveParameter, false);
            suitToDeal = Suit.MOTIVE;
        }
        else if (clueToCheck == 1)
        {
            suitToDeal = Suit.LOCATION;
            isMissingClue = DialogueDataWriter.Instance.CheckCondition(charSO.locationParameter, false);
        }
        else
        {
            suitToDeal = Suit.WITNESS;
            isMissingClue = DialogueDataWriter.Instance.CheckCondition(charSO.witnessParameter, false);
        }
        bool dealToPlayerOne = (!isMissingClue && !isSecondTalk) || (isSecondTalk && isMissingClue);
        whoToDealTo = dealToPlayerOne ? ConstantParameters.PLAYER_1 : ConstantParameters.PLAYER_2;
        DealCardsInSuit(whoToDealTo, 3, suitToDeal);
        dialogueDealCounter++;
    }
}
