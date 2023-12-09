using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.TestTools;

public class AITests
{
    #region Test Fixtures
    private HandData MakeRandomHand()
    {
        RandomStrategy rs = new RandomStrategy();
        rs.SetUp(new List<int> { ConstantParameters.MIN_FACE_VALUE, ConstantParameters.MAX_FACE_VALUE });
        HandData hand = new HandData(ConstantParameters.MAX_HAND_SIZE);
        List<int> dealtWitness = new List<int>() ;
        List<int> dealtLocation = new List<int>();
        List<int> dealtMotive = new List<int>();
        List<int> limitList;
        for (int i = 0; i < ConstantParameters.MAX_HAND_SIZE; i++)
        {
            Suit suit = rs.SelectSuit(i);
            if (suit == Suit.WITNESS)
            {
                limitList = dealtWitness;
            } else if (suit == Suit.MOTIVE)
            {
                limitList = dealtMotive;
            } else
            {
                limitList = dealtLocation;
            }
            int card = rs.GetCard(suit,limitList);
            hand.AddCard(new CardData(card, suit));
        }
        return hand;
    }

    private HandData MakeFixtureHand()
    {
        HandData hand = new HandData(ConstantParameters.MAX_HAND_SIZE);
        hand.AddCard(new CardData(3, Suit.WITNESS));
        hand.AddCard(new CardData(7, Suit.WITNESS));
        hand.AddCard(new CardData(6, Suit.MOTIVE));
        hand.AddCard(new CardData(6, Suit.LOCATION));
        return hand;
    }

    private BoardState MakeBoardStateWithLaneVulnerability()
    {
        // Lane 2 is vulnerable
        BoardState boardState = new BoardState();
        boardState.PlayerAddCardToLane(ConstantParameters.PLAYER_1, new CardData(8, Suit.LOCATION), 0);
        boardState.PlayerAddCardToLane(ConstantParameters.PLAYER_1, new CardData(3, Suit.WITNESS), 1);
        boardState.PlayerAddCardToLane(ConstantParameters.PLAYER_1, new CardData(10, Suit.MOTIVE), 2);
        return boardState;
    }

    private BoardState MakeBoardWithPressLaneWin()
    {
        BoardState boardState = new BoardState();

        // Lock Other Lanes using huge impossible points
        boardState.PlayerAddCardToLane(ConstantParameters.PLAYER_1, new CardData(10, Suit.LOCATION), 0);
        boardState.PlayerAddCardToLane(ConstantParameters.PLAYER_1, new CardData(10, Suit.MOTIVE), 0);
        boardState.PlayerAddCardToLane(ConstantParameters.PLAYER_1, new CardData(10, Suit.WITNESS), 1);
        boardState.PlayerAddCardToLane(ConstantParameters.PLAYER_1, new CardData(10, Suit.MOTIVE), 1);
        
        // 13 in last lane
        boardState.PlayerAddCardToLane(ConstantParameters.PLAYER_1, new CardData(3, Suit.LOCATION), 2);
        boardState.PlayerAddCardToLane(ConstantParameters.PLAYER_1, new CardData(10, Suit.WITNESS), 2);

        // Player 2 is primed by being given a witness card in the last lane
        boardState.PlayerAddCardToLane(ConstantParameters.PLAYER_2, new CardData(8, Suit.WITNESS), 2);

        // Decision SHOULD be play the 3 of witness over any other cards

        return boardState;
    }

    private BoardState MakeBoardWithEmphasisLaneWin()
    {
        BoardState boardState = new BoardState();
        boardState.PlayerAddCardToLane(ConstantParameters.PLAYER_1, new CardData(10, Suit.LOCATION), 0);
        boardState.PlayerAddCardToLane(ConstantParameters.PLAYER_1, new CardData(10, Suit.MOTIVE), 0);
        boardState.PlayerAddCardToLane(ConstantParameters.PLAYER_1, new CardData(10, Suit.WITNESS), 1);
        boardState.PlayerAddCardToLane(ConstantParameters.PLAYER_1, new CardData(10, Suit.MOTIVE), 1);

        // 13 in last lane
        boardState.PlayerAddCardToLane(ConstantParameters.PLAYER_1, new CardData(8, Suit.LOCATION), 2);
        boardState.PlayerAddCardToLane(ConstantParameters.PLAYER_1, new CardData(10, Suit.WITNESS), 2);

        // Player 2 is primed by being given a witness card in the last lane
        boardState.PlayerAddCardToLane(ConstantParameters.PLAYER_2, new CardData(6, Suit.WITNESS), 2);
        return boardState;
    }
    #endregion
    #region Tests
    [Test]
    public void TestScriptedAI()
    {
        ScriptedAI testAI = new ScriptedAI();
        int lane, card = 0;
        BoardState nothingState = new BoardState();
        HandData nothingHand = new HandData(ConstantParameters.MAX_HAND_SIZE);
        for (int i = 0; i < ConstantParameters.MAX_TURNS; i++)
        {
            (lane, card) = testAI.DecideMove(nothingState, nothingHand);
            Assert.AreEqual(lane, i % 3 + 1);
            Assert.AreEqual(card, 0);
        }
    }

    [Test]
    public void TestRandomAI()
    {
        RandomAI testAI = new RandomAI();
        int lane, card = 0;
        BoardState nothingState = new BoardState();
        HandData hand = MakeRandomHand();
        (lane, card) = testAI.DecideMove(nothingState, hand);

        Assert.Less(lane, 4);
        Assert.Greater(lane, 0);

        Assert.Less(card, ConstantParameters.MAX_HAND_SIZE);
        Assert.GreaterOrEqual(card, 0);
    }

    [Test]
    public void TestInformedAIFirstMove()
    {
        InformedAI testAI = new InformedAI();
        int lane, cardIndex = 0;
        BoardState nothingState = new BoardState();
        HandData hand = MakeFixtureHand();

        int expectedCardIndex = 1;
        int expectedLaneValue = 1;
        (lane, cardIndex) = testAI.DecideMove(nothingState, hand);
        Assert.AreEqual(expectedLaneValue, lane);
        Assert.AreEqual(expectedCardIndex, cardIndex);
    }

    [Test]
    public void TestLaneWinEvaluator()
    {
        BoardState boardState = MakeBoardStateWithLaneVulnerability();
        bool isWinner = boardState.TestLaneWin(2, new CardData(4, Suit.LOCATION));
        Assert.IsTrue(isWinner);

        bool isLoser = boardState.TestLaneWin(2, new CardData(3, Suit.LOCATION));
        Assert.IsFalse(isLoser);
    }

    [Test]
    public void TestInformedAIWillTryWinLane()
    {
        InformedAI testAI = new InformedAI();
        BoardState boardState = MakeBoardStateWithLaneVulnerability();
        HashSet<int> missing_lanes = boardState.GetUnclaimedLanesForPlayer(ConstantParameters.PLAYER_2);
        int lane, cardIndex = 0;
        HandData hand = MakeFixtureHand();

        int expectedCardIndex = 1;
        int expectedLaneValue = 2;
        int expectedMissingLanes = 3;

        (lane, cardIndex) = testAI.DecideMove(boardState, hand);
        Assert.AreEqual(expectedMissingLanes, missing_lanes.Count);
        Assert.AreEqual(expectedLaneValue, lane);
        Assert.AreEqual(expectedCardIndex, cardIndex);
    }

    [Test]
    public void TestInformedWillGoForPress()
    {
        InformedAI testAI = new InformedAI();
        BoardState boardState = MakeBoardWithPressLaneWin();
        HashSet<int> missing_lanes = boardState.GetUnclaimedLanesForPlayer(ConstantParameters.PLAYER_2);
        int lane, cardIndex = 0;
        HandData hand = MakeFixtureHand();

        int expectedCardIndex = 0;
        int expectedLaneValue = 3;
        int expectedMissingLanes = 3;

        (lane, cardIndex) = testAI.DecideMove(boardState, hand);
        Assert.AreEqual(expectedMissingLanes, missing_lanes.Count);
        Assert.AreEqual(expectedLaneValue, lane);
        Assert.AreEqual(expectedCardIndex, cardIndex);
    }

    [Test]
    public void TestInformedWillGoForEmphasis()
    {
        InformedAI testAI = new InformedAI();
        BoardState boardState = MakeBoardWithEmphasisLaneWin();
        HashSet<int> missing_lanes = boardState.GetUnclaimedLanesForPlayer(ConstantParameters.PLAYER_2);
        int lane, cardIndex = 0;
        HandData hand = MakeFixtureHand();

        int expectedCardIndex = 2;
        int expectedLaneValue = 3;
        int expectedMissingLanes = 3;

        (lane, cardIndex) = testAI.DecideMove(boardState, hand);
        Assert.AreEqual(expectedMissingLanes, missing_lanes.Count);
        Assert.AreEqual(expectedLaneValue, lane);
        Assert.AreEqual(expectedCardIndex, cardIndex);
    }
    #endregion
}