using System;
using System.Collections.Generic;
using UnityEngine;

public class BetManager : MonoBehaviour
{
    public int CurrentBetAmount { get; private set; } = 5000;
    private int PlayerChips { get; set; } = 100000;

    [SerializeField] private int betStep = 5000, minBet = 5000, maxBet = 100000;
    [SerializeField] private RouletteBetRaycaster raycaster;

    private readonly List<IPlacedBet> _placedBets = new();
    private Dictionary<string, int> _activeChips = new();
    private Dictionary<string, BetBox> _betBoxMap;
    
    private IGameUIManager UIManager { get; set; }
    private ICoinService CoinService { get; set; }
    private ISaveService SaveService { get; set; }
    private GameSaveData SaveData { get; set; }


    private void Awake()
    {
        SaveService ??= new SaveService();
        UIManager ??= FindObjectOfType<GameUIManager>();
        CoinService ??= FindObjectOfType<CoinSpawner>();

        _betBoxMap = new();
        foreach (var betBox in FindObjectsOfType<BetBox>())
            _betBoxMap[betBox.betType.ToString()] = betBox;

        SaveData = SaveService.Load("GameSave") ?? new GameSaveData();
        PlayerChips = SaveData.currentMoney;
        _activeChips = ToDictionary(SaveData.activeChips);

        _placedBets.Clear();
        if (SaveData.activeChips != null)
        {
            foreach (var kv in SaveData.activeChips)
            {
                string key = kv.key;
                int amount = kv.value;
                if (int.TryParse(key, out int number))
                    _placedBets.Add(new NumberBet(number, amount));
                else if (key.Contains("-"))
                {
                    var nums = key.Split('-');
                    int[] numbers = Array.ConvertAll(nums, int.Parse);
                    int payout = RoulettePayouts.GetPayoutForNumberCount(numbers.Length);
                    _placedBets.Add(new GroupBet(numbers, amount, payout));
                }
                else if (System.Enum.TryParse<BetType>(key, out var betType))
                    _placedBets.Add(new SpecialBet(betType, amount));
            }
        }

        UIManager.UpdateChipsText(PlayerChips);
        CoinService.RestoreAllCoins(_activeChips);
        RouletteStatisticsStore.Update(SaveData);
    }

    private void OnEnable()
    {
        GameEvents.OnSpinCompleted += EvaluateBets;
        GameEvents.OnBetIncreased += OnBetIncreased;
        GameEvents.OnBetDecreased += OnBetDecreased;
        GameEvents.OnChipsAdjusted += OnChipsAdjusted;
        GameEvents.OnClearBetsRequested += OnClearBetsRequested;
    }

    private void OnDisable()
    {
        GameEvents.OnSpinCompleted -= EvaluateBets;
        GameEvents.OnBetIncreased -= OnBetIncreased;
        GameEvents.OnBetDecreased -= OnBetDecreased;
        GameEvents.OnChipsAdjusted -= OnChipsAdjusted;
        GameEvents.OnClearBetsRequested -= OnClearBetsRequested;
    }

    private void OnBetIncreased()
    {
        IncreaseBet();
    }

    private void OnBetDecreased()
    {
        DecreaseBet();
    }

    private void OnChipsAdjusted(int delta)
    {
        AdjustChips(delta);
    }

    private void IncreaseBet()
    {
        CurrentBetAmount = Mathf.Min(CurrentBetAmount + betStep, maxBet);
        GameEvents.BetAmountChanged(CurrentBetAmount);
    }

    private void DecreaseBet()
    {
        CurrentBetAmount = Mathf.Max(CurrentBetAmount - betStep, minBet);
        GameEvents.BetAmountChanged(CurrentBetAmount);
    }

    // Sets the current bet amount directly
    public void PlaceSpecialBet(BetType betType, int amount, Transform betTransform)
    {
        if (PlayerChips < amount) return;
        _placedBets.Add(new SpecialBet(betType, amount));
        AdjustChips(-amount);

        string key = betType.ToString();
        AddToActiveChips(key, amount);
        CoinService.DropCoinToPosition(betTransform.position + Vector3.up * 2f, key, _activeChips[key]);
        AutoSave();
    }

    // Places a bet on a single number
    public void PlaceNumberBet(int number, int amount)
    {
        if (PlayerChips < amount) return;
        _placedBets.Add(new NumberBet(number, amount));
        AdjustChips(-amount);

        string key = number.ToString();
        AddToActiveChips(key, amount);
        Vector3 pos = raycaster.GetCellCenter(number);
        CoinService.DropCoinToPosition(pos, key, _activeChips[key]);
        AutoSave();
    }

    // Places a bet on a group of numbers (e.g., split, corner, street)
    public void PlaceGroupBet(int[] numbers, int amount, int payoutMultiplier)
    {
        if (PlayerChips < amount) return;
        _placedBets.Add(new GroupBet(numbers, amount, payoutMultiplier));
        AdjustChips(-amount);

        string key = string.Join("-", numbers);
        AddToActiveChips(key, amount);
        Vector3 pos = raycaster.GetCellsCenter(numbers);
        CoinService.DropCoinToPosition(pos, key, _activeChips[key]);
        AutoSave();
    }

    // Evaluates all placed bets against the spin result
    public void EvaluateBets(int spinResult)
    {
        SaveData.winningNumbers.Add(spinResult);
        SaveData.totalSpins++;

        int totalWin = 0, totalBet = 0;
        foreach (var bet in _placedBets)
        {
            totalBet += bet.Amount;
            totalWin += bet.GetWinAmount(spinResult);
        }

        int totalLose = Mathf.Max(0, totalBet - totalWin);

        if (totalWin > 0)
        {
            AdjustChips(totalWin);
            SaveData.totalWins++;
            SaveData.totalProfit += (totalWin - totalBet);
            GameEvents.TriggerWin();
            UIManager.ShowResultNotification(true, totalWin, spinResult,
                RouletteColorHelper.GetUnityColor(RouletteColorHelper.GetNumberColor(spinResult)));
        }
        else
        {
            SaveData.totalLosses++;
            SaveData.totalMoneyLoss += totalLose;
            GameEvents.TriggerLose();
            UIManager.ShowResultNotification(false, totalLose, spinResult,
                RouletteColorHelper.GetUnityColor(RouletteColorHelper.GetNumberColor(spinResult)));
        }

        CoinService.DestroyAllCoins();
        _placedBets.Clear();
        _activeChips.Clear();
        AutoSave();
    }

    public void AdjustChips(int delta)
    {
        PlayerChips += delta;
        UIManager.UpdateChipsText(PlayerChips);
    }

    private void AutoSave()
    {
        SaveData.currentMoney = PlayerChips;
        SaveData.activeChips = ToKeyValueList(_activeChips);
        SaveService.Save(SaveData, "GameSave");
        RouletteStatisticsStore.Update(SaveData);
    }

    public Vector3 GetSpecialAreaPosition(string key)
    {
        if (_betBoxMap != null && _betBoxMap.TryGetValue(key, out var box))
            return box.transform.position + Vector3.up * 2f;
        return Vector3.zero;
    }

    // Helpers
    private static List<KeyValue> ToKeyValueList(Dictionary<string, int> dict)
    {
        var list = new List<KeyValue>();
        foreach (var kv in dict)
            list.Add(new KeyValue { key = kv.Key, value = kv.Value });
        return list;
    }

    private static Dictionary<string, int> ToDictionary(List<KeyValue> list)
    {
        var dict = new Dictionary<string, int>();
        if (list != null)
        {
            foreach (var kv in list)
                dict[kv.key] = kv.value;
        }

        return dict;
    }

    private void AddToActiveChips(string key, int amount)
    {
        _activeChips ??= new();
        if (!_activeChips.ContainsKey(key)) _activeChips[key] = 0;
        _activeChips[key] += amount;
    }

    private void OnApplicationQuit() => AutoSave();

    private void OnApplicationPause(bool pause)
    {
        if (pause) AutoSave();
    }

    private void OnClearBetsRequested()
    {
        CoinService.DestroyAllCoins();
        int totalBet = 0;
        foreach (var bet in _placedBets)
            totalBet += bet.Amount;
        AdjustChips(totalBet);
        _placedBets.Clear();
        _activeChips.Clear();
        SaveData.activeChips = new List<KeyValue>();
        AutoSave();
    }
}