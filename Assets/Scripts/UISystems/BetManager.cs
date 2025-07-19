using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BetManager : MonoBehaviour
{
    [Header("UI")] [SerializeField] private TextMeshProUGUI chipsText;

    public int CurrentBetAmount { get; private set; } = 5000;
    [SerializeField] private int BetStep = 5000;
    [SerializeField] private int MinBet = 5000;
    [SerializeField] private int MaxBet = 100000;
    [SerializeField] private int PlayerChips = 100000;
    [SerializeField] private CoinSpawner coinSpawner;
    [SerializeField] private RouletteTableRaycaster Raycaster;
    [SerializeField] private GameUIManager gameUIManager;
    private readonly List<IPlacedBet> placedBets = new List<IPlacedBet>();

    private void Start() => UpdateChipsText();

    public void IncreaseBet() =>
        CurrentBetAmount = Mathf.Min(CurrentBetAmount + BetStep, MaxBet);

    public void DecreaseBet() =>
        CurrentBetAmount = Mathf.Max(CurrentBetAmount - BetStep, MinBet);

    public bool PlaceSpecialBet(BetType betType, int amount, Transform transformOfSpecial)
    {
        if (PlayerChips < amount) return false;
        placedBets.Add(new SpecialBet(betType, amount));
        PlayerChips -= amount;
        UpdateChipsText();
        coinSpawner.DropCoinToPosition(transformOfSpecial.position + Vector3.up * 2f, betType.ToString(), amount);
        return true;
    }

    public void PlaceNumberBet(int number, int amount)
    {
        if (PlayerChips < amount) return;
        placedBets.Add(new NumberBet(number, amount));
        PlayerChips -= amount;
        UpdateChipsText();
        Vector3 pos = Raycaster.GetCellCenter(number);
        string key = number.ToString();
        coinSpawner.DropCoinToPosition(pos, key, amount);
    }

    public void PlaceGroupBet(int[] numbers, int amount, int payoutMultiplier)
    {
        if (PlayerChips < amount) return;
        placedBets.Add(new GroupBet(numbers, amount, payoutMultiplier));
        PlayerChips -= amount;
        UpdateChipsText();
        string key = string.Join("-", numbers);
        Vector3 pos = Raycaster.GetCellsCenter(numbers);
        coinSpawner.DropCoinToPosition(pos, key, amount);
    }

    public void EvaluateBets(int spinResult)
    {
        int totalWin = 0;
        int totalBet = 0;

        foreach (var bet in placedBets)
        {
            totalBet += bet.Amount;       
            totalWin += bet.GetWinAmount(spinResult);
        }

        int totalLose = Mathf.Max(0, totalBet - totalWin);

        if (totalWin > 0)
        {
            AddChips(totalWin);
            gameUIManager.ShowWinNotification(totalWin);
        }
        else
        {
            gameUIManager.ShowLoseNotification(totalLose);
        }

        coinSpawner.DestroyAllCoins();
        placedBets.Clear();
    }

    private void AddChips(int amount)
    {
        PlayerChips += amount;
        UpdateChipsText();
    }

    private void UpdateChipsText()
    {
        chipsText.text = PlayerChips.ToString("N0");
    }
}

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