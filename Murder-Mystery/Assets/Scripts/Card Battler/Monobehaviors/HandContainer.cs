using System.Collections.Generic;
using UnityEngine;

public class HandContainer : MonoBehaviour
{
    public HandData handData;
    private List<GameObject> cards;

    private void Awake()
    {
        handData = new HandData(ConstantParameters.MAX_HAND_SIZE);
        cards = new List<GameObject>();
    }

    public void FreezeCards()
    {
        //Make cards unselectable
        foreach (GameObject card in cards)
        {
            card.layer = LayerMask.NameToLayer("Default");
        }
    }

    public void ReceiveCard(GameObject card)
    {
        // Receive the card game object and process its data
        Card cardComponent = card.GetComponent<Card>();
        CardData cardData = cardComponent.cardData;
        handData.AddCard(cardData);
        cards.Add(card);
    }

    public void MoveToHand(GameObject card, float zOffset){
        /* Little animation to run at the start of the match to show the cards being dealt to players
        Inputs:
        Card - the card to move to the hand
        zOffset - How much cards are offset to move them back into hand
        */
        MovementController mc = card.GetComponent<MovementController>();
        PlayToLane ptl = card.GetComponent<PlayToLane>();
        Vector3 cardPos = card.transform.position;
        mc.SetOrigin(card.transform.position);
        mc.SetOriginScale(card.transform.localScale);
        Vector3 finalPosition = new Vector3(cardPos.x, cardPos.y, cardPos.z + zOffset * -1);
        mc.AddMovement(finalPosition, ConstantParameters.RETURN_TO_HAND_DURATION);
        mc.ToggleMovement();
        ptl.SetHandOrigin(finalPosition);
    }

    public GameObject GetPhysicalCardReference(int index)
    {
        /*
            Returns a reference to the card Game Object as specified by the index
        */
        GameObject card = cards[index];
        return card;
    }

    public GameObject PopCardObject(int index)
    {
        /*
            Same as GetPhysicalCardReference but also removes the card from the hand.
        */
        GameObject card = GetPhysicalCardReference(index);
        cards.RemoveAt(index);
        return card;
    }

    public int GetCurrentHandSize()
    {
        return handData.cards.Count;
    }
}
