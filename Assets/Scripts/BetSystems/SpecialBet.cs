public class SpecialBet : IPlacedBet
{
    public BetType BetType { get; }
    public int Amount { get; }

    public SpecialBet(BetType betType, int amount)
    {
        BetType = betType;
        Amount = amount; 
    }

    public bool IsWin(int spinResult) => RouletteWinLogic.IsBetWin(BetType, spinResult);

    public int GetWinAmount(int spinResult)
    {
        if (!IsWin(spinResult)) return 0;
        return Amount * RoulettePayouts.GetPayoutMultiplier(BetType);
    }
}