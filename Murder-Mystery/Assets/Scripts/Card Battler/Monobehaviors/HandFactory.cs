using System;
using System.Collections.Generic;
using UnityEngine;

public class HandFactory : MonoBehaviour
{
    public HandContainer Player1Hand;
    public HandContainer Player2Hand;
    public GameObject cardPrefab;
    public DealStrategies strategyIdentifier;
    private IDealStrategy dealStrategy;
    public List<Material> witnessFaces;
    public List<Material> locationFaces;
    public List<Material> motiveFaces;
    public float xOffset;
    public float xOrigin;
    public float zOrigin;
    public float zOffset;
    public int maxFaceValue;
    public int minFaceValue;

    [SerializeField]
    public Vector3 scaleFactor;

    public void AssignAndSetupStrategy(List<int> setupData)
    {
        // Assign the strategy object and set it up
        dealStrategy = HandDealStrategyFactory.CreateStrategy(strategyIdentifier);
        dealStrategy.SetUp(setupData);
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

        //Determine if dealing to bottom player or top player
        int zMod = 1;
        if (player == ConstantParameters.PLAYER_1) { handToDealTo = Player1Hand; }
        else { handToDealTo = Player2Hand; zMod *= -1; }

        List<int> dealtWitnessCards = new List<int>();
        List<int> dealtLocationCards = new List<int>();
        List<int> dealtMotiveCards = new List<int>();

        for (int i = 0; i < n; i++)
        {
            GameObject instantiatedCard = CreateCardToDeal(handToDealTo, dealtWitnessCards, dealtLocationCards, dealtMotiveCards, zMod);
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

    private GameObject CreateCardToDeal(HandContainer handToDealTo, List<int> dealtWitnessCards, List<int> dealtLocationCards, List<int> dealtMotiveCards, int zMod)
    {
        int i = handToDealTo.GetCurrentHandSize();

        // Create the card object
        Suit suit = dealStrategy.SelectSuit(i);
        int chosenCard = dealStrategy.GetCard(suit, dealtWitnessCards, dealtLocationCards, dealtMotiveCards);
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
}
