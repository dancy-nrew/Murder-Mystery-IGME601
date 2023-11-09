public class MathUtil
{
    public static int RemapToPosAndNeg(int zeroOrOne)
    {
        /* Utility method that maps 0 or 1 to -1 and 1, respectively
        Inputs:
        zeroOrOne: An int that is either 0 or 1
        Outputs:
        An int that is either -1 or 1
        */

        return zeroOrOne * 2 - 1;
    }
}
