public class GroupBet : IPlacedBet
{
    public int[] Numbers { get; }
    public int Amount { get; }
    public int PayoutMultiplier { get; }

    public GroupBet(int[] numbers, int amount, int payoutMultiplier)
    {
        Numbers = numbers;
        Amount = amount;
        PayoutMultiplier = payoutMultiplier;
    }

    public bool IsWin(int spinResult) => System.Array.Exists(Numbers, n => n == spinResult);
    public int GetWinAmount(int spinResult) => IsWin(spinResult) ? Amount * PayoutMultiplier : 0;
}