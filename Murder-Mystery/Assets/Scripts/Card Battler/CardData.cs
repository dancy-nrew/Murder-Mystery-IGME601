using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Suit
{
    LOCATION,
    WITNESS,
    MOTIVE
}


public class CardData
{
    public int Face { get; private set; }
    public Suit Suit { get; private set; }
    public CardData(int face, Suit suit)
    {
        this.Face = face;
        this.Suit = suit;
    }
}
