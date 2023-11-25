using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public enum DealStrategies
{
    Random,
    ClueBased,
    Deterministic
}

public interface IDealStrategy
{
    public void SetUp(List<int> values);
    public Suit SelectSuit(int index);
    public int GetCard(Suit suit, List<int> dealtWitness, List<int> dealtLocation, List<int> dealtMotive);
}

public abstract class Strategy : IDealStrategy
{
    protected virtual bool IsSetupValid(List<int> values) { return true; }
    public abstract void SetUp(List<int> values);
    public abstract Suit SelectSuit(int index);
    public abstract int GetCard(Suit suit, List<int> dealtWitness, List<int> dealtLocation, List<int> dealtMotive);
}

public class RandomStrategy : Strategy
{
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
    public override int GetCard(Suit suit, List<int> dealtWitness, List<int> dealtLocation, List<int> dealtMotive)
    {
        /*
            This function picks an available face value from the deck of cards, working under the assumption that
            no duplicate cards can exist within a deck.

            Inputs:
            Suit enum - the suit to pick from
            dealtWitness list - a list of already dealt witness cards. We don't want to deal the same card twice. Passed by reference.
            dealtLocation list - same as dealtWitness, but for location cards
            dealtMotive list -  same as dealtWitness, but for motive cards.

            Outputs -
            face value of the chosen card as int
        */
        List<int> indexList;

        switch (suit)
        {
            case Suit.WITNESS:
                indexList = dealtWitness;
                break;
            case Suit.LOCATION:
                indexList = dealtLocation;
                break;
            case Suit.MOTIVE:
                indexList = dealtMotive;
                break;
            //The default case is here so the compiler doesn't complain. It should never happen.
            default:
                Debug.Log("Default case in Hand Factory reached. This is a major bug.");
                indexList = new List<int>();
                break;
        }
        int value = UnityEngine.Random.Range(minFaceValue, maxFaceValue);
        while (indexList.Contains(value))
        {
            value = UnityEngine.Random.Range(minFaceValue, maxFaceValue);
        }
        indexList.Add(value);
        return value;
    }
}

public class ClueBasedStrategy : Strategy
{
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
        if (!IsSetupValid(flags))
        {
            throw new ArgumentException("Setup for Clue Strategy is Invalid");
        }
        bHasMotive = flags[0] > 0;
        bHasLocation = flags[1] > 0;
        bHasWitness = flags[2] > 0;

        int valueRange = ConstantParameters.MAX_FACE_VALUE - ConstantParameters.MIN_FACE_VALUE;
        maxLowValue = minLowValue + valueRange / 2;
        minHighValue = maxHighValue - valueRange / 2;
        Debug.Log(maxLowValue.ToString());
        Debug.Log(minHighValue.ToString());
    }

    public override Suit SelectSuit(int index)
    {
        // The random deal strategy doesn't require an index, but we keep it otherwise
        // the interface wouldn't hold

        Array suits = Enum.GetValues(typeof(Suit));
        return (Suit)suits.GetValue(index % suits.Length);
    }

    public override int GetCard(Suit suit, List<int> dealtWitness, List<int> dealtLocation, List<int> dealtMotive)
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
        HashSet<int> pickedIndeces;
        List<int> pickedList;
        bool clueToCheck;
        switch (suit)
        {
            case Suit.WITNESS:
                pickedIndeces = dealtWitness.ToHashSet();
                pickedList = dealtWitness;
                clueToCheck = bHasWitness;
                break;
            case Suit.LOCATION:
                pickedIndeces = dealtLocation.ToHashSet();
                pickedList = dealtLocation;
                clueToCheck = bHasLocation;
                break;
            case Suit.MOTIVE:
                pickedIndeces = dealtMotive.ToHashSet();
                pickedList = dealtMotive;
                clueToCheck = bHasMotive;
                break;
            //The default case is here so the compiler doesn't complain. It should never happen.
            default:
                Debug.Log("Default case in Hand Factory reached. This is a major bug.");
                pickedIndeces = new HashSet<int>();
                clueToCheck = false;
                pickedList = new List<int>();
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
        possibleValues.ExceptWith(pickedIndeces);
        if (possibleValues.Count == 0)
        {
            throw new IndexOutOfRangeException("No possible values to give for this suit");
        }
        int value = possibleValues.ElementAt(UnityEngine.Random.Range(0, possibleValues.Count));
        pickedList.Add(value);
        return value;
    }
}

public class DeterministicStrategy : Strategy 
{
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
        return (Suit)suits.GetValue(index % suits.Length);
    }
    public override void SetUp(List<int> values)
    {
        if (!IsSetupValid(values))
        {
            throw new ArgumentException("Setup for Deterministic Strategy is Invalid");
        }

        for (int i = 0; i < values.Count; i++)
        {
            cardValues.Enqueue(values[i]);
        }
    }

    public override int GetCard(Suit suit, List<int> dealtWitness, List<int> dealtLocation, List<int> dealtMotive)
    {
        return cardValues.Dequeue();
    }
}

public class HandDealStrategyFactory : MonoBehaviour
{
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
