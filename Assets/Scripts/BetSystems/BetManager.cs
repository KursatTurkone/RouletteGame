using System.Collections.Generic;
using UnityEngine;

public class BetManager : MonoBehaviour
{
    public int CurrentBetAmount { get; private set; } = 5000;
    [SerializeField] private int betStep = 5000;
    [SerializeField] private int minBet = 5000;
    [SerializeField] private int maxBet = 100000;
    [SerializeField] private int playerChips = 100000;
    [SerializeField] private CoinSpawner coinSpawner;
    [SerializeField] private RouletteTableRaycaster raycaster;
    private GameUIManager _gameUIManager;
    private readonly List<IPlacedBet> _placedBets = new List<IPlacedBet>();

    private void Start()
    {
        GameManager.Instance.betManager = this;
        _gameUIManager = FindObjectOfType<GameUIManager>();
        _gameUIManager.UpdateChipsText(playerChips);
    }

    public void IncreaseBet() =>
        CurrentBetAmount = Mathf.Min(CurrentBetAmount + betStep, maxBet);

    public void DecreaseBet() =>
        CurrentBetAmount = Mathf.Max(CurrentBetAmount - betStep, minBet);

    public bool PlaceSpecialBet(BetType betType, int amount, Transform transformOfSpecial)
    {
        if (playerChips < amount) return false;
        _placedBets.Add(new SpecialBet(betType, amount));
        playerChips -= amount;
        _gameUIManager.UpdateChipsText(playerChips);
        coinSpawner.DropCoinToPosition(transformOfSpecial.position + Vector3.up * 2f, betType.ToString(), amount);
        return true;
    }

    public void PlaceNumberBet(int number, int amount)
    {
        if (playerChips < amount) return;
        _placedBets.Add(new NumberBet(number, amount));
        playerChips -= amount;
        _gameUIManager.UpdateChipsText(playerChips);
        Vector3 pos = raycaster.GetCellCenter(number);
        string key = number.ToString();
        coinSpawner.DropCoinToPosition(pos, key, amount);
    }

    public void PlaceGroupBet(int[] numbers, int amount, int payoutMultiplier)
    {
        if (playerChips < amount) return;
        _placedBets.Add(new GroupBet(numbers, amount, payoutMultiplier));
        playerChips -= amount;
        _gameUIManager.UpdateChipsText(playerChips);
        string key = string.Join("-", numbers);
        Vector3 pos = raycaster.GetCellsCenter(numbers);
        coinSpawner.DropCoinToPosition(pos, key, amount);
    }

    public void EvaluateBets(int spinResult)
    {
        int totalWin = 0;
        int totalBet = 0;

        foreach (var bet in _placedBets)
        {
            totalBet += bet.Amount;
            totalWin += bet.GetWinAmount(spinResult);
        }

        int totalLose = Mathf.Max(0, totalBet - totalWin);

        if (totalWin > 0)
        {
            AddChips(totalWin);
            _gameUIManager.ShowWinNotification(totalWin);
        }
        else
        {
            _gameUIManager.ShowLoseNotification(totalLose);
        }

        coinSpawner.DestroyAllCoins();
        _placedBets.Clear();
    }

    public void AddChips(int amount)
    {
        playerChips += amount;
        _gameUIManager.UpdateChipsText(playerChips);
    }
}

