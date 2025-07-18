public class GroupBet : IPlacedBet
{
    private int[] numbers;
    private int amount;
    private int payoutMultiplier;
    public GroupBet(int[] numbers, int amount, int payoutMultiplier)
    {
        this.numbers = numbers;
        this.amount = amount;
        this.payoutMultiplier = payoutMultiplier;
    }
    public bool IsWin(int spinResult) => System.Array.Exists(numbers, n => n == spinResult);
    public int GetWinAmount(int spinResult) => IsWin(spinResult) ? amount * payoutMultiplier : 0;
}