using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameRuleTests
{
    [Test]
    public void TestCardProperties()
    {
        int card_value = 3;
        Suit card_suit = Suit.LOCATION;
        CardData card = new CardData(card_value, card_suit);
        Assert.AreEqual(card.Face, card_value);
        Assert.AreEqual(card.Suit, card_suit);
    }

    [Test]
    public void TestHandDataProperties()
    {
        HandData handData = new HandData(ConstantParameters.MAX_HAND_SIZE);
        Assert.AreEqual(handData.value, 0);
        Assert.AreEqual(handData.cards.Count, 0);
    }

    [Test]
    public void TestHandDataAddCard()
    {
        HandData handData = new HandData(ConstantParameters.MAX_HAND_SIZE);
        CardData card = new CardData(3, Suit.LOCATION);
        handData.AddCard(card);
        handData.CalculateHandValue();

        // There should be one card and the value of the hand should ONLY increase
        // by the face value of the card added.
        Assert.AreEqual(handData.cards.Count, 1);
        Assert.AreEqual(handData.value, card.Face);
    }

    [Test]
    public void TestHandDataAddTwoCardsNoBonus()
    {
        HandData handData = new HandData(ConstantParameters.MAX_HAND_SIZE);
        CardData card_one = new CardData(3, Suit.LOCATION);
        CardData card_two = new CardData(4, Suit.WITNESS);
        handData.AddCard(card_one);
        handData.AddCard(card_two);
        handData.CalculateHandValue();

        // There should be two card and the value of the hand should ONLY increase
        // by the face value of the cards added.
        Assert.AreEqual(handData.cards.Count, 2);
        Assert.AreEqual(handData.value, card_one.Face + card_two.Face);
    }

    [Test]
    public void TestHandDataPressBonus()
    {
        //Pressing means adding two cards of the same suit
        HandData handData = new HandData(ConstantParameters.MAX_HAND_SIZE);
        CardData card_one = new CardData(3, Suit.LOCATION);
        CardData card_two = new CardData(4, Suit.LOCATION);
        int expected_bonus_value = card_two.Face - card_one.Face;
        int expected_hand_value = card_one.Face + card_two.Face + expected_bonus_value;

        handData.AddCard(card_one);
        handData.AddCard(card_two);
        handData.CalculateHandValue();

        Assert.AreEqual(handData.cards.Count, 2);
        Assert.AreEqual(handData.value, expected_hand_value);
    }

    [Test]
    public void TestHandDataEmphasisBonus()
    {
        //Pressing means adding two cards of the same face value
        HandData handData = new HandData(ConstantParameters.MAX_HAND_SIZE);
        CardData card_one = new CardData(3, Suit.LOCATION);
        CardData card_two = new CardData(3, Suit.WITNESS);

        int expected_hand_value = card_one.Face + card_two.Face + card_one.Face*2;

        handData.AddCard(card_one);
        handData.AddCard(card_two);
        handData.CalculateHandValue();

        Assert.AreEqual(handData.cards.Count, 2);
        Assert.AreEqual(handData.value, expected_hand_value);
    }

    [Test]
    public void TestEmphasisAndPress()
    {
        HandData handData = new HandData(ConstantParameters.MAX_HAND_SIZE);
        CardData card_one = new CardData(3, Suit.LOCATION);
        CardData card_two = new CardData(3, Suit.WITNESS);
        CardData card_three = new CardData(4, Suit.LOCATION);

        int expected_hand_value = card_one.Face + card_two.Face + card_three.Face;
        int expected_emphasis_bonus = card_one.Face * 2;
        int expected_press_bonus = card_three.Face - card_one.Face;
        expected_hand_value += expected_press_bonus + expected_emphasis_bonus;

        handData.AddCard(card_one);
        handData.AddCard(card_two);
        handData.AddCard(card_three);
        handData.CalculateHandValue();

        Assert.AreEqual(handData.cards.Count, 3);
        Assert.AreEqual(handData.value, expected_hand_value);
    }
}
