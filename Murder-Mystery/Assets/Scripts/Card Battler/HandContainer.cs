using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandContainer : MonoBehaviour
{
    private HandData handData;
    private List<GameObject> cards;
    // Start is called before the first frame update
    void Start()
    {
        handData = new HandData(ConstantParameters.MAX_HAND_SIZE);
        cards = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DealCard(GameObject card)
    {
        CardData cardData = card.GetComponent<CardData>();
        handData.AddCard(cardData);
        cards.Add(card);
    }

}
