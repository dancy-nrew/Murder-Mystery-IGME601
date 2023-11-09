using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandContainer : MonoBehaviour
{
    public HandData handData = new HandData(ConstantParameters.MAX_HAND_SIZE);
    private List<GameObject> cards = new List<GameObject>();

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
        Vector3 finalPosition = new Vector3(cardPos.x, cardPos.y, cardPos.z + zOffset * -1); 
        mc.SetDestination(finalPosition);
        mc.SetMovementDuration(10);
        mc.ToggleMovement();
        ptl.SetHandOrigin(finalPosition);
    }

    public GameObject GetPhysicalCardReference(int index)
    {
        GameObject card = cards[index];
        return card;
    }

    public GameObject PopCardObject(int index)
    {
        GameObject card = GetPhysicalCardReference(index);
        cards.RemoveAt(index);
        return card;
    }
}
