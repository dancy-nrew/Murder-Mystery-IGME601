using System;
using System.Collections.Generic;
using UnityEngine;

public class HandFactory : MonoBehaviour
{

    public HandContainer Player1Hand;
    public HandContainer Player2Hand;
    public float xOffset;
    public float xOrigin;
    public float zOrigin;
    public float zOffset;
    public List<GameObject> WitnessCards = new List<GameObject>();
    public List<GameObject> LocationCards = new List<GameObject>();
    public List<GameObject> MotiveCards = new List<GameObject>();

    [SerializeField]
    public Vector3 scaleFactor;

    void Start()
    {
        DealCards(ConstantParameters.PLAYER_1);
        DealCards(ConstantParameters.PLAYER_2);
    }

    void DealCards(int player)
    {
        /*
            This function deals cards to each players hand. It instantiates the card objects and assigns them to the HandContainer
            for the respective player.

            Inputs:
            player - int to determine which player we're dealing with.
        */
        HandContainer handToDealTo;
        int zMod = 1;
        if (player == ConstantParameters.PLAYER_1) { handToDealTo = Player1Hand; }
        else {  handToDealTo = Player2Hand; zMod *= -1; }

        List<int> dealtWitnessCards = new List<int>();
        List<int> dealtLocationCards = new List<int>();
        List<int> dealtMotiveCards = new List<int>();
        Array suits = Enum.GetValues(typeof(Suit));

        for (int i = 0; i < ConstantParameters.MAX_HAND_SIZE; i++) {

            Suit suit = (Suit)suits.GetValue((int)UnityEngine.Random.Range(0, suits.Length));
            (int, GameObject) chosenCard = GetCardFromSuit(suit, dealtWitnessCards, dealtLocationCards, dealtMotiveCards);
            Vector3 targetPosition = handToDealTo.gameObject.transform.position;
            float zAdjust = (zOffset * (i%2)+ zOrigin) * zMod;
            Vector3 instantiateLocation = new Vector3(targetPosition.x + (xOffset*i) + xOrigin, targetPosition.y, targetPosition.z  + zAdjust);
            GameObject instantiatedCard = Instantiate(chosenCard.Item2, instantiateLocation, Quaternion.identity);
            instantiatedCard.transform.localScale = new Vector3(
                            instantiatedCard.transform.localScale.x * scaleFactor.x,
                            instantiatedCard.transform.localScale.y * scaleFactor.y,
                            instantiatedCard.transform.localScale.z * scaleFactor.z
            );
            if (player == ConstantParameters.PLAYER_2)
            {
                instantiatedCard.layer = LayerMask.NameToLayer("Default");
                instantiatedCard.transform.rotation *= Quaternion.Euler(0, 0, 180f);
            } else
            {
                instantiatedCard.transform.rotation *= Quaternion.Euler(0, 180f, 0);
            }
            handToDealTo.ReceiveCard(instantiatedCard);
            handToDealTo.MoveToHand(instantiatedCard, zOrigin*zMod);
        }
    }

    private (int,GameObject) GetCardFromSuit(Suit suit, List<int> dealtWitness, List<int> dealtLocation, List<int> dealtMotive)
    {
        /*
            This function picks an available card prefab from the deck within the suit specified in the parameters 
            to be instantiated in later.

            Inputs:
            Suit enum - the suit to pick from
            dealtWitness list - a list of already dealt witness cards. We don't want to deal the same card twice. Passed by reference.
            dealtLocation list - same as dealtWitness, but for location cards
            dealtMotive list -  same as dealtWitness, but for motive cards.

            Outputs -
            Tuple of (int, GameObject)
            The int represent the index of the dealt card within the list. 
            The GameObject is the card to instantiate.
        */
        GameObject gO;
        int index;

        if (suit == Suit.WITNESS)
        {
            index = (int)UnityEngine.Random.Range(0, WitnessCards.Count - 1);
            while (dealtWitness.Contains(index))
            {
                index = (int)UnityEngine.Random.Range(0, WitnessCards.Count - 1);
            }
            gO = WitnessCards[index];
            dealtWitness.Add(index);

        } else if (suit == Suit.LOCATION)
        {
            index = (int)UnityEngine.Random.Range(0, LocationCards.Count - 1);
            while (dealtLocation.Contains(index))
            {
                index = (int)UnityEngine.Random.Range(0, LocationCards.Count - 1);
            }
            gO = LocationCards[index];
            dealtLocation.Add(index);
        } else
        {
            index = (int)UnityEngine.Random.Range(0, MotiveCards.Count - 1);
            while (dealtMotive.Contains(index))
            {
                index = (int)UnityEngine.Random.Range(0, MotiveCards.Count - 1);
            }
            gO = MotiveCards[index];
            dealtMotive.Add(index);
        }

        return (index,gO);
    }
}
