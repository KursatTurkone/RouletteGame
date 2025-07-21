using System;
using System.Collections.Generic;
using UnityEngine;

public class BetManager : MonoBehaviour
{
    public int CurrentBetAmount { get; private set; } = 5000;
    private int PlayerChips { get; set; } = 100000;

    [SerializeField] private int betStep = 5000;
    [SerializeField] private int minBet = 5000;
    [SerializeField] private int maxBet = 100000;

    [SerializeField] private CoinSpawner coinSpawner;
    [SerializeField] private RouletteTableRaycaster raycaster;

    private GameUIManager _gameUIManager;
    private readonly List<IPlacedBet> _placedBets = new List<IPlacedBet>();
    private GameSaveData _saveData;
    private Dictionary<string, BetBox> _betBoxMap;
    private Dictionary<string, int> _activeChips;
    public List<int> WinningNumbers => _saveData.winningNumbers;

    private void Awake()
    {
        InitBetBoxes();
        LoadGame();
        _gameUIManager = FindObjectOfType<GameUIManager>();
        _gameUIManager.UpdateChipsText(PlayerChips);
        coinSpawner.RestoreAllCoins(_activeChips);
    }

    private void Start()
    {
        GameManager.Instance.betManager = this;
    }

    private void InitBetBoxes()
    {
        _betBoxMap = new Dictionary<string, BetBox>();
        foreach (var betBox in FindObjectsOfType<BetBox>())
            _betBoxMap[betBox.betType.ToString()] = betBox;
    }

    private void LoadGame()
    {
        _saveData = SaveSystem.Load<GameSaveData>("GameSave") ?? new GameSaveData();
        PlayerChips = _saveData.currentMoney;
        _activeChips = ToDictionary(_saveData.activeChips);

        _placedBets.Clear();
        if (_saveData.activeChips != null)
        {
            foreach (var kv in _saveData.activeChips)
            {
                string key = kv.key;
                int amount = kv.value;
                if (int.TryParse(key, out int number))
                    _placedBets.Add(new NumberBet(number, amount));
                else if (key.Contains("-"))
                {
                    var nums = key.Split('-');
                    int[] numbers = new int[nums.Length];
                    for (int i = 0; i < nums.Length; i++)
                        numbers[i] = int.Parse(nums[i]);
                    int payout = RoulettePayouts.GetPayoutForNumberCount(numbers.Length);
                    _placedBets.Add(new GroupBet(numbers, amount, payout));
                }
                else if (System.Enum.TryParse<BetType>(key, out var betType))
                    _placedBets.Add(new SpecialBet(betType, amount));
            }
        }
    }

    public void IncreaseBet() => CurrentBetAmount = Mathf.Min(CurrentBetAmount + betStep, maxBet);
    public void DecreaseBet() => CurrentBetAmount = Mathf.Max(CurrentBetAmount - betStep, minBet);

    public bool PlaceSpecialBet(BetType betType, int amount, Transform transformOfSpecial)
    {
        if (PlayerChips < amount) return false;
        _placedBets.Add(new SpecialBet(betType, amount));
        AdjustChips(-amount);
        AddToActiveChips(betType.ToString(), amount);

        coinSpawner.DropCoinToPosition(transformOfSpecial.position + Vector3.up * 2f, betType.ToString(),
            _activeChips[betType.ToString()]);
        AutoSave();
        return true;
    }

    public void PlaceNumberBet(int number, int amount)
    {
        if (PlayerChips < amount) return;
        _placedBets.Add(new NumberBet(number, amount));
        AdjustChips(-amount);
        string key = number.ToString();
        AddToActiveChips(key, amount);

        Vector3 pos = raycaster.GetCellCenter(number);
        coinSpawner.DropCoinToPosition(pos, key, _activeChips[key]);
        AutoSave();
    }

    public void PlaceGroupBet(int[] numbers, int amount, int payoutMultiplier)
    {
        if (PlayerChips < amount) return;
        _placedBets.Add(new GroupBet(numbers, amount, payoutMultiplier));
        AdjustChips(-amount);
        string key = string.Join("-", numbers);
        AddToActiveChips(key, amount);

        Vector3 pos = raycaster.GetCellsCenter(numbers);
        coinSpawner.DropCoinToPosition(pos, key, _activeChips[key]);
        AutoSave();
    }

    public void EvaluateBets(int spinResult)
    {
        _saveData.winningNumbers.Add(spinResult);

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
            AdjustChips(totalWin);
            _gameUIManager.ShowResultNotification(true, totalWin, spinResult,
                RouletteColorHelper.GetUnityColor(RouletteColorHelper.GetNumberColor(spinResult)));
        }
        else
        {
            _gameUIManager.ShowResultNotification(false, totalWin, spinResult,
                RouletteColorHelper.GetUnityColor(RouletteColorHelper.GetNumberColor(spinResult)));
        }

        coinSpawner.DestroyAllCoins();
        _placedBets.Clear();
        _activeChips.Clear();
        AutoSave();
    }

    public void AdjustChips(int delta)
    {
        PlayerChips += delta;
        _gameUIManager.UpdateChipsText(PlayerChips);
    }

    private void AutoSave()
    {
        _saveData.currentMoney = PlayerChips;
        _saveData.activeChips = ToKeyValueList(_activeChips);
        SaveSystem.Save(_saveData, "GameSave");
    }

    public Vector3 GetSpecialAreaPosition(string key)
    {
        if (_betBoxMap != null && _betBoxMap.TryGetValue(key, out var box))
            return box.transform.position + Vector3.up * 2f;
        return Vector3.zero;
    }

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
        _activeChips.TryAdd(key, 0);
        _activeChips[key] += amount;
    }

    private void OnApplicationQuit() => AutoSave();

    private void OnApplicationPause(bool pause)
    {
        if (pause) AutoSave();
    }
}