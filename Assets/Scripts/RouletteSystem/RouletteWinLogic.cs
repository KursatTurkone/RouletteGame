using System.Collections.Generic;

public static class RouletteWinLogic
{
    private static readonly Dictionary<BetType, System.Func<int, bool>> betWinStrategies =
        new Dictionary<BetType, System.Func<int, bool>>
        {
            { BetType.Red, n => System.Array.Exists(RouletteBets.RedNumbers, x => x == n) },
            { BetType.Black, n => System.Array.Exists(RouletteBets.BlackNumbers, x => x == n) },
            { BetType.Green, n => n == 0 },
            { BetType.Even, n => n != 0 && n % 2 == 0 },
            { BetType.Odd, n => n % 2 == 1 },
            { BetType.Low, n => n >= 1 && n <= 18 },
            { BetType.High, n => n >= 19 && n <= 36 },
            { BetType.First12, n => System.Array.Exists(RouletteBets.First12, x => x == n) },
            { BetType.Second12, n => System.Array.Exists(RouletteBets.Second12, x => x == n) },
            { BetType.Third12, n => System.Array.Exists(RouletteBets.Third12, x => x == n) },
            { BetType.Column1, n => System.Array.Exists(RouletteBets.Column1, x => x == n) },
            { BetType.Column2, n => System.Array.Exists(RouletteBets.Column2, x => x == n) },
            { BetType.Column3, n => System.Array.Exists(RouletteBets.Column3, x => x == n) }
        };

    public static bool IsBetWin(BetType betType, int spinResult)
    {
        if (betWinStrategies.TryGetValue(betType, out var strategy))
            return strategy(spinResult);
        return false;
    }
}