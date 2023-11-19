using UnityEngine;

public class Card : MonoBehaviour
{
    public int face;
    public Suit suit;
    public CardData cardData;

    // Start is called before the first frame update
    void Awake()
    {
        cardData = new CardData(face, suit);
    }
}
