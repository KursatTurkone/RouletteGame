public static class RoulettePayouts
{
    public static int GetPayoutMultiplier(BetType betType)
    {
     
        if (betType.ToString().StartsWith("Num_"))
            return 36;

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
}