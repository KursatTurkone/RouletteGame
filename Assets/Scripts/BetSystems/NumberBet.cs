public class NumberBet : IPlacedBet
{
    private int Number { get; }
    public int Amount { get; }

    public NumberBet(int number, int amount)
    {
        Number = number;
        Amount = amount; 
    }

    public bool IsWin(int spinResult) => Number == spinResult;
    public int GetWinAmount(int spinResult) => IsWin(spinResult) ? Amount * 36 : 0;
}