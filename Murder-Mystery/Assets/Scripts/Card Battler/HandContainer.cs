using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandContainer : MonoBehaviour
{
    private HandData handData = new HandData(ConstantParameters.MAX_HAND_SIZE);
    private List<GameObject> cards = new List<GameObject>();
    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DealCard(GameObject card)
    {
        Card cardComponent = card.GetComponent<Card>();
        CardData cardData = cardComponent.cardData;
        handData.AddCard(cardData);
        cards.Add(card);
    }

}
