using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BetManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI chipsText;

    public int CurrentBetAmount { get; private set; } = 5000;
    public int BetStep = 5000;
    public int MinBet = 5000;
    public int MaxBet = 100000;
    public int PlayerChips = 100000;
    
    private Dictionary<BetType, int> specialBets = new Dictionary<BetType, int>();
    private Dictionary<int, int> numberBets = new Dictionary<int, int>();
    private List<NumberGroupBet> groupBets = new List<NumberGroupBet>();

    private void Start() => UpdateChipsText();

    public void IncreaseBet()
    {
        CurrentBetAmount = Mathf.Min(CurrentBetAmount + BetStep, MaxBet);
    }

    public void DecreaseBet()
    {
        CurrentBetAmount = Mathf.Max(CurrentBetAmount - BetStep, MinBet);
    }
    
    public bool PlaceSpecialBet(BetType betType, int amount)
    {
        if (PlayerChips < amount) return false;
        if (!specialBets.ContainsKey(betType))
            specialBets[betType] = 0;
        specialBets[betType] += amount;
        PlayerChips -= amount;
        UpdateChipsText();
        return true;
    }
    
    public bool PlaceNumberBet(int number, int amount)
    {
        if (PlayerChips < amount) return false;
        if (!numberBets.ContainsKey(number))
            numberBets[number] = 0;
        numberBets[number] += amount;
        PlayerChips -= amount;
        UpdateChipsText();
        return true;
    }
    
    public bool PlaceGroupBet(int[] numbers, int amount, int payoutMultiplier)
    {
        if (PlayerChips < amount) return false;
        groupBets.Add(new NumberGroupBet { Numbers = numbers, Amount = amount, Multiplier = payoutMultiplier });
        PlayerChips -= amount;
        UpdateChipsText();
        return true;
    }

    public void EvaluateBets(int spinResult)
    {
        int totalWin = 0;
        
        foreach (var bet in specialBets)
        {
            Debug.Log($"Checking bet {bet.Key} for {spinResult}");
            if (IsBetWin(bet.Key, spinResult))
            {
                int multiplier = RoulettePayouts.GetPayoutMultiplier(bet.Key);
                int win = bet.Value * multiplier;
                Debug.Log($"Kazandın: {bet.Key} - {win}");
                totalWin += win;
            }
        }
        
        foreach (var bet in numberBets)
        {
            if (bet.Key == spinResult)
            {
                int win = bet.Value * 36;
                totalWin += win;
            }
        }
        
        foreach (var bet in groupBets)
        {
            if (Array.Exists(bet.Numbers, n => n == spinResult))
            {
                int win = bet.Amount * bet.Multiplier;
                totalWin += win;
            }
        }

        if (totalWin > 0)
            AddChips(totalWin);

        ClearAllBets();
    }

    public bool IsBetWin(BetType betType, int spinResult)
    {
        if (betType == BetType.Red && Array.Exists(RouletteBets.RedNumbers, n => n == spinResult)) return true;
        if (betType == BetType.Black && Array.Exists(RouletteBets.BlackNumbers, n => n == spinResult)) return true;
        if (betType == BetType.Green && spinResult == 0) return true;
        if (betType == BetType.Even && spinResult != 0 && spinResult % 2 == 0) return true;
        if (betType == BetType.Odd && spinResult % 2 == 1) return true;
        if (betType == BetType.Low && spinResult >= 1 && spinResult <= 18) return true;
        if (betType == BetType.High && spinResult >= 19 && spinResult <= 36) return true;
        if (betType == BetType.First12 && Array.Exists(RouletteBets.First12, n => n == spinResult)) return true;
        if (betType == BetType.Second12 && Array.Exists(RouletteBets.Second12, n => n == spinResult)) return true;
        if (betType == BetType.Third12 && Array.Exists(RouletteBets.Third12, n => n == spinResult)) return true;
        if (betType == BetType.Column1 && Array.Exists(RouletteBets.Column1, n => n == spinResult)) return true;
        if (betType == BetType.Column2 && Array.Exists(RouletteBets.Column2, n => n == spinResult)) return true;
        if (betType == BetType.Column3 && Array.Exists(RouletteBets.Column3, n => n == spinResult)) return true;
        return false;
    }

    public void ClearAllBets()
    {
        specialBets.Clear();
        numberBets.Clear();
        groupBets.Clear();
    }

    public void AddChips(int amount)
    {
        PlayerChips += amount;
        UpdateChipsText();
    }

    private void UpdateChipsText()
    {
        chipsText.text = PlayerChips.ToString("N0");
    }

    public class NumberGroupBet
    {
        public int[] Numbers;
        public int Amount;
        public int Multiplier;
    }
}
