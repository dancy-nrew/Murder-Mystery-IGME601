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
        Face = face;
        Suit = suit;
    }
}
