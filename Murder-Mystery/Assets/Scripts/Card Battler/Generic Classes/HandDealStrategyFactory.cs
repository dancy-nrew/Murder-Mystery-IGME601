using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum DealStrategies
{
    Random,
    ClueBased,
    Deterministic
}

public interface IDealStrategy
{
    /*
     Interface for implementing card-dealing strategies. All strategies must implement the following methods

    - SetUp: Take a list of integers to set up some basic information for the strategy.
    - SelectSuit: Select the suit the card to deal will belong to
    - GetCard: Have the strategy deal a card. It takes lists of already-dealt cards to avoid repetition.
     */
    public void SetUp(List<int> values);
    public Suit SelectSuit(int index);
    public int GetCard(Suit suit, List<int> dealtCardsOfThisSuit);
}

public abstract class Strategy : IDealStrategy
{
    /*
        This abstract class extends the interface by adding a protected method that adds validation
        capabilities to each strategy class. Useful to prevent malformed deal strategies from 
        going to production.
     */
    protected virtual bool IsSetupValid(List<int> values) { return true; }
    public abstract void SetUp(List<int> values);
    public abstract Suit SelectSuit(int index);
    public abstract int GetCard(Suit suit, List<int> dealtCardsOfThisSuit);
}

public class RandomStrategy : Strategy
{
    /*
     Deals cards randomly
     */
    int minFaceValue;
    int maxFaceValue;

    public override void SetUp(List<int> values)
    {
        minFaceValue = ConstantParameters.MIN_FACE_VALUE;
        maxFaceValue = ConstantParameters.MAX_FACE_VALUE;
    }

    public override Suit SelectSuit(int index)
    {
        // The random deal strategy doesn't require an index, but we keep it otherwise
        // the interface wouldn't hold

        Array suits = Enum.GetValues(typeof(Suit));
        return (Suit)suits.GetValue((int)UnityEngine.Random.Range(0, suits.Length));
    }
    public override int GetCard(Suit suit, List<int> dealtCardsOfThisSuit)
    {
        /*
            This implementation randomly deals a card that is available to be dealt from the deck

            Inputs:
            Suit enum - the suit to pick from
            dealtWitness list - a list of already dealt witness cards. Passed by reference.
            dealtLocation list - same as dealtWitness, but for location cards
            dealtMotive list -  same as dealtWitness, but for motive cards.

            Outputs -
            face value of the chosen card as int
        */
        int value = UnityEngine.Random.Range(minFaceValue, maxFaceValue+1);
        while (dealtCardsOfThisSuit.Contains(value))
        {
            value = UnityEngine.Random.Range(minFaceValue, maxFaceValue+1);
        }
        dealtCardsOfThisSuit.Add(value);
        return value;
    }
}

public class ClueBasedStrategy : Strategy
{
    /*
     Deals cards based on how many clues the player has gotten. This is a player-only dealing strategy,
     the AI should never be dealt cards using this strategy.
     */
    bool bHasMotive;
    bool bHasLocation;
    bool bHasWitness;
    int minLowValue = ConstantParameters.MIN_FACE_VALUE;
    int maxLowValue;
    int minHighValue;
    int maxHighValue = ConstantParameters.MAX_FACE_VALUE;

 
    protected override bool IsSetupValid(List<int> flags)
    {
        return flags.Count == 3;
    }

    public override void SetUp(List<int> flags)
    {
        /*
         We need to map the inputs to the boolean flags and then construct a few ranges
         to allow for more specific low-rolling or high-rolling depending on whether the
         user has the clues or not.
         */
        if (!IsSetupValid(flags))
        {
            throw new ArgumentException("Setup for Clue Strategy is Invalid");
        }

        // This could probably be hardcoded or better mapped somewhere else
        bHasMotive = flags[0] > 0;
        bHasLocation = flags[1] > 0;
        bHasWitness = flags[2] > 0;

        int valueRange = ConstantParameters.MAX_FACE_VALUE - ConstantParameters.MIN_FACE_VALUE;
        maxLowValue = minLowValue + valueRange / 2;
        minHighValue = maxHighValue - valueRange / 2;
    }

    public override Suit SelectSuit(int index)
    {
        // The random deal strategy doesn't require an index, but we keep it otherwise
        // the interface wouldn't hold

        Array suits = Enum.GetValues(typeof(Suit));
        return (Suit)suits.GetValue(index % suits.Length);
    }

    public override int GetCard(Suit suit, List<int> dealtCardsOfThisSuit)
    {
        /*
            This function determines whether to pick from a high value or low value list
            depending on whether the player has the clues for this card battler.

            Inputs:
            Suit enum - the suit to pick from
            dealtWitness list - a list of already dealt witness cards. We don't want to deal the same card twice. Passed by reference.
            dealtLocation list - same as dealtWitness, but for location cards
            dealtMotive list -  same as dealtWitness, but for motive cards.

            Outputs -
            face value of the chosen card as int
        */
        HashSet<int> pickedIndeces = dealtCardsOfThisSuit.ToHashSet();
        bool clueToCheck;
        switch (suit)
        {
            case Suit.WITNESS:
                clueToCheck = bHasWitness;
                break;
            case Suit.LOCATION:
                clueToCheck = bHasLocation;
                break;
            case Suit.MOTIVE:
                clueToCheck = bHasMotive;
                break;
            //The default case is here so the compiler doesn't complain. It should never happen.
            default:
                Debug.Log("Default case in Hand Factory reached. This is a major bug.");
                clueToCheck = false;
                break;
        }
        int minFaceValue, maxFaceValue;
        if (clueToCheck)
        {
            minFaceValue = minHighValue;
            maxFaceValue = maxHighValue;
        } else
        {
            minFaceValue = minLowValue;
            maxFaceValue = maxLowValue;
        }

        // Pick a random card within the range
        HashSet<int> possibleValues = new HashSet<int>();
        for (int i = minFaceValue; i <= maxFaceValue; i++)
        {
            possibleValues.Add(i);
        }

        //Narrow down possibilities using set operations
        possibleValues.ExceptWith(pickedIndeces);

        // This shouldn't happen but it's important to check it.
        if (possibleValues.Count == 0)
        {
            throw new IndexOutOfRangeException("No possible values to give for this suit");
        }

        int value = possibleValues.ElementAt(UnityEngine.Random.Range(0, possibleValues.Count));
        dealtCardsOfThisSuit.Add(value);
        return value;
    }
}

public class DeterministicStrategy : Strategy 
{
    /*
        Deals cards deterministically, meaning we as developers establish which
        cards will be dealt in what order.
    */
    Queue<int> cardValues;

    protected override bool IsSetupValid(List<int> values)
    {
        foreach (int i in values)
        {
            if (i < ConstantParameters.MIN_FACE_VALUE || i > ConstantParameters.MAX_FACE_VALUE)
            {
                return false;
            }
        }
        return true;
    }

    public override Suit SelectSuit(int index)
    {
        // Iterate through the suits
        
        Array suits = Enum.GetValues(typeof(Suit));
        int suitIndex = index / suits.Length;
        return (Suit)suits.GetValue(suitIndex);
    }
    public override void SetUp(List<int> values)
    {
        /*
         In this case, the values represent the face values of the cards we want to deal.
         They should be enqueued, we will deal them in FIFO style.
         */
        cardValues = new Queue<int>();
        if (!IsSetupValid(values))
        {
            throw new ArgumentException("Setup for Deterministic Strategy is Invalid");
        }
        
        for (int i = 0; i < values.Count; i++)
        {
            cardValues.Enqueue(values[i]);
        }
    }

    public override int GetCard(Suit suit, List<int> dealtCardsOfThisSuit)
    {
        return cardValues.Dequeue();
    }
}

public class HandDealStrategyFactory : MonoBehaviour
{
    /*
     Simple factory to create strategies. Allow dealing strategies to be changed at runtime.
     */
    public static IDealStrategy CreateStrategy(DealStrategies strategy)
    {
        switch (strategy)
        {
            case DealStrategies.ClueBased:
                return new ClueBasedStrategy();
            case DealStrategies.Deterministic:
                return new DeterministicStrategy();
            default:
                return new RandomStrategy();
        }
    }
}
