using System;
using System.Collections.Generic;
using UnityEngine;

public class HandFactory : MonoBehaviour
{

    public HandContainer Player1Hand;
    public HandContainer Player2Hand;
    public GameObject cardPrefab;
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
            int chosenCard = GetRandomCardFromSuit(suit, dealtWitnessCards, dealtLocationCards, dealtMotiveCards);
            Vector3 targetPosition = handToDealTo.gameObject.transform.position;
            float zAdjust = (zOffset * (i%2)+ zOrigin) * zMod;
            Vector3 instantiateLocation = new Vector3(targetPosition.x + (xOffset*i) + xOrigin, targetPosition.y, targetPosition.z  + zAdjust);
            GameObject instantiatedCard = Instantiate(cardPrefab, instantiateLocation, Quaternion.identity);
            Card cardComponent = instantiatedCard.GetComponent<Card>();

            cardComponent.SetCardData(chosenCard, suit);
            cardComponent.SetFrontFaceMaterial(_GetMaterial(chosenCard, suit));

            instantiatedCard.transform.localScale = new Vector3(
                            instantiatedCard.transform.localScale.x * scaleFactor.x,
                            instantiatedCard.transform.localScale.y * scaleFactor.y,
                            instantiatedCard.transform.localScale.z * scaleFactor.z
            );
            if (player == ConstantParameters.PLAYER_2)
            {
                instantiatedCard.transform.rotation *= Quaternion.Euler(0, 0, 180f);
            } else
            {
                cardComponent.MakeInteractable();
                instantiatedCard.transform.rotation *= Quaternion.Euler(0, 180f, 0);
            }
            handToDealTo.ReceiveCard(instantiatedCard);
            handToDealTo.MoveToHand(instantiatedCard, zOrigin*zMod);
        }
    }

    private int GetRandomCardFromSuit(Suit suit, List<int> dealtWitness, List<int> dealtLocation, List<int> dealtMotive)
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
        List<int> indexList;

        switch (suit)
        {
            case Suit.WITNESS:
                indexList = dealtWitness;
                break;
            case Suit.LOCATION:
                indexList = dealtLocation;
                break;
            case Suit.MOTIVE:
                indexList = dealtWitness;
                break;
            //The default case is here so the compiler doesn't complain. It should never happen.
            default:
                Debug.Log("Default case in Hand Factory reached. This is a major bug.");
                indexList = new List<int>();
                break;
        }
        int value = UnityEngine.Random.Range(minFaceValue, maxFaceValue);
        while (indexList.Contains(value))
        {
            value = UnityEngine.Random.Range(minFaceValue, maxFaceValue);
        }
        indexList.Add(value);
        return value;
    }

    private int _ValueToMaterialIndex(int value)
    {
        return value - minFaceValue;
    }

    private Material _GetMaterial(int value, Suit suit)
    {
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
