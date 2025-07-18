public class NumberBet : IPlacedBet
{
    private int number;
    private int amount;
    public NumberBet(int number, int amount)
    {
        this.number = number;
        this.amount = amount;
    }
    public bool IsWin(int spinResult) => number == spinResult;
    public int GetWinAmount(int spinResult) => IsWin(spinResult) ? amount * 36 : 0;
}
