using System;
using System.Collections.Generic;
using UnityEngine;

public class HandFactory : MonoBehaviour
{
    public HandContainer Player1Hand;
    public HandContainer Player2Hand;
    public float xOffset;
    public float xOrigin;
    public List<GameObject> WitnessCards = new List<GameObject>();
    public List<GameObject> LocationCards = new List<GameObject>();
    public List<GameObject> MotiveCards = new List<GameObject>();

    void Awake()
    {
        DealCards(1);
        DealCards(2);
    }

    void DealCards(int player)
    {
        HandContainer handToDealTo;
        float xRot = 35.0f;
        if (player == 1) { handToDealTo = Player1Hand; xRot = xRot * -1; }
        else {  handToDealTo = Player2Hand; xRot -= 180.0f; }

        List<int> dealtWitnessCards = new List<int>();
        List<int> dealtLocationCards = new List<int>();
        List<int> dealtMotiveCards = new List<int>();
        Array suits = Enum.GetValues(typeof(Suit));

        for (int i = 0; i < ConstantParameters.MAX_HAND_SIZE; i++) {
            Suit suit = (Suit)suits.GetValue((int)UnityEngine.Random.Range(0, suits.Length));
            (int, GameObject) chosenCard = GetCardFromSuit(suit, dealtWitnessCards, dealtLocationCards, dealtMotiveCards);
            Quaternion rotation = Quaternion.Euler(xRot, 0, 0);
            Vector3 targetPosition = handToDealTo.gameObject.transform.position;
            Vector3 instantiateLocation = new Vector3(targetPosition.x + xOrigin + (xOffset*i), targetPosition.y + (0.25f*i), targetPosition.z - xOffset*i*0.5f);
            GameObject instantiatedCard = Instantiate(chosenCard.Item2, instantiateLocation, rotation);
            if (player == 1)
            {
                instantiatedCard.AddComponent<MouseSelect>();
            }
            handToDealTo.DealCard(instantiatedCard);
        }
    }

    private (int,GameObject) GetCardFromSuit(Suit suit, List<int> dealtWitness, List<int> dealtLocation, List<int> dealtMotive)
    {
        GameObject gO;
        int index;

        if (suit == Suit.WITNESS)
        {
            // We don't deal the same card twice
            index = (int)UnityEngine.Random.Range(0, WitnessCards.Count - 1);
            while (dealtWitness.Contains(index))
            {
                index = (int)UnityEngine.Random.Range(0, WitnessCards.Count - 1);
            }
            gO = WitnessCards[index];
            dealtWitness.Add(index);

        } else if (suit == Suit.LOCATION)
        {
            // We don't deal the same card twice
            index = (int)UnityEngine.Random.Range(0, LocationCards.Count - 1);
            while (dealtLocation.Contains(index))
            {
                index = (int)UnityEngine.Random.Range(0, LocationCards.Count - 1);
            }
            gO = LocationCards[index];
            dealtLocation.Add(index);
        } else
        {
            // We don't deal the same card twice
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
