public static class RoulettePayouts
{
    public static int GetPayoutMultiplier(BetType betType)
    {
        switch (betType)
        {
            case BetType.Red:
            case BetType.Black:
            case BetType.Even:
            case BetType.Odd:
            case BetType.Low:
            case BetType.High:
                return 2;
            case BetType.First12:
            case BetType.Second12:
            case BetType.Third12:
            case BetType.Column1:
            case BetType.Column2:
            case BetType.Column3:
                return 3;
        }
        return 0;
    }
    public static int GetPayoutForNumberCount(int count)
    {
        switch (count)
        {
            case 1: return 36; // Straight
            case 2: return 18; // Split
            case 3: return 12; // Street
            case 4: return 9;  // Corner
            case 6: return 6;  // SixLine
            default: return 0;
        }
    }
}