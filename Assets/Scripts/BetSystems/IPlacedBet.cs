public interface IPlacedBet
{
    bool IsWin(int spinResult);
    int GetWinAmount(int spinResult);
    int Amount { get; }
}