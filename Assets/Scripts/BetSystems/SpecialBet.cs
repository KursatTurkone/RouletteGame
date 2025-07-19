public class SpecialBet : IPlacedBet
{
    private BetType betType;
    private int amount;
    public SpecialBet(BetType betType, int amount)
    {
        this.betType = betType;
        this.amount = amount;
    }
    public bool IsWin(int spinResult) => RouletteWinLogic.IsBetWin(betType, spinResult);
    public int GetWinAmount(int spinResult)
    {
        if (!IsWin(spinResult)) return 0;
        return amount * RoulettePayouts.GetPayoutMultiplier(betType);
    }
}