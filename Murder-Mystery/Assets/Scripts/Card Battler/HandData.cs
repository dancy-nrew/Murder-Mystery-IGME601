using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct SuitCalculationStruct
{
    public int maxFaceValue;
    public int summedFaceValue;
    public int count;
    public int bonusValue;

    public SuitCalculationStruct(int mx, int sum, int c, int bonus)
    {
        this.maxFaceValue = mx;
        this.summedFaceValue = sum;
        this.count = c;
        this.bonusValue = bonus;
    }

    public void SetMaxFaceValue(int value)
    {
        this.maxFaceValue = value;
    }

    public void IncreaseCount()
    {
        this.count++;
    }

    public void AddToFaceValue(int value)
    {
        this.summedFaceValue += value;
    }

    public int CalculateBonus()
    {
        this.bonusValue = this.maxFaceValue * this.count - this.summedFaceValue;
        return this.bonusValue;
    }
}

public class HandData
{
    //A hand of cards is basically a container for the cards
    //Lanes could be treated as hands (they contain the cards being played within them)
    public List<CardData> cards;
    int size;
    bool isLane;
    public int value;

    public HandData(int handSize)
    {
        size = handSize;
        cards = new List<CardData>();
        isLane = false;
    }

    public List<CardData> CopyCards()
    {
        List<CardData> newCards = new List<CardData>();
        for (int i = 0; i < cards.Count; i++)
        {
            CardData card = new CardData(cards[i].Face, cards[i].Suit);
            newCards.Add(card);
        }
        return newCards;
    }

    // Clone Support Functions
    public void SetCards(List<CardData> cards)
    {
        this.cards = cards;
    }

    public void SetValue(int value)
    {
        this.value = value;
    }

    public HandData(int size, bool isLane)
    {
        this.size = size;
        this.cards = new List<CardData>();
        this.isLane = isLane;
    }

    public void AddCard(CardData card)
    {
        if (this.cards.Count == this.size)
        {
            Debug.Log("Trying to add a card to a full hand");
        }
        this.cards.Add(card);
    }

    public CardData PopCard(int index)
    {
        //Test if index within list first
        if (index < 0 || index >= this.cards.Count)
        {
            throw new ArgumentException("Index for desired card out of range");
        }

        CardData card = this.cards[index];
        this.cards.RemoveAt(index);
        return card;
    }

    public int CalculateSuitBonuses()
    {
        int result = 0;
        // Initialize the counter (extendable if we change the amount of suits)
        Dictionary<Suit, SuitCalculationStruct> handComposition = new Dictionary<Suit, SuitCalculationStruct>();
        foreach (Suit suit in (Suit[])Enum.GetValues(typeof(Suit)))
        {
            SuitCalculationStruct s = new SuitCalculationStruct(0,0,0,0);
            handComposition.Add(suit, s);
        }

        // Set the information up for each suit
        for(int i = 0; i < this.cards.Count; i++)
        {
            Suit s = this.cards[i].Suit;
            int face = this.cards[i].Face;
            handComposition[s].IncreaseCount();
            handComposition[s].AddToFaceValue(face);
            if (handComposition[s].maxFaceValue < face)
            {
                handComposition[s].SetMaxFaceValue(face);
            }
        }

        // Calculate the final value
        foreach(KeyValuePair<Suit, SuitCalculationStruct> kvp in handComposition)
        {
            result += kvp.Value.CalculateBonus();
        }
        return result;
    }

    public int CalculateSameFaceBonuses()
    {
        int result = 0;
        Dictionary<int, int> counter = new Dictionary<int, int>();
        for (int i = ConstantParameters.MIN_FACE_VALUE; i < ConstantParameters.MAX_FACE_VALUE; i++)
        {
            counter[i] = 0;
        }

        foreach(CardData card in this.cards)
        {
            counter[card.Face]++;
        }

        // Add a bonus equal to the multiplication of the card face times how many times it's appeared
        // Only valid if there is more than one copy of a card.
        foreach(KeyValuePair<int, int> kvp in counter)
        {
            if (kvp.Value > 1)
            {
                result += kvp.Key * kvp.Value;
            }
        }
        return result;
    }

    public int CalculateHandValue()
    {
        if (this.isLane)
        {
            // In case we want to add lane-specific bonuses, add code here
        }
        int total_face_value = 0;
        foreach(CardData card in cards)
        {
            total_face_value += card.Face;
        }
        this.value += total_face_value;
        this.value += this.CalculateSuitBonuses();
        this.value += this.CalculateSameFaceBonuses();
        return this.value;
    }
}
