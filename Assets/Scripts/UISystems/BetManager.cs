using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BetManager : MonoBehaviour
{
    [Header("UI")] [SerializeField] private TextMeshProUGUI chipsText;
    public int CurrentBetAmount { get; private set; } = 5000;
    public int BetStep = 5000;
    public int MinBet = 5000;
    public int MaxBet = 100000;
    public int PlayerChips = 100000;
    private Dictionary<BetType, int> currentBets = new Dictionary<BetType, int>();

    private void Start()
    {
        UpdateChipsText();
    }

    public void IncreaseBet()
    {
        CurrentBetAmount = Mathf.Min(CurrentBetAmount + BetStep, MaxBet);
    }

    public void DecreaseBet()
    {
        CurrentBetAmount = Mathf.Max(CurrentBetAmount - BetStep, MinBet);
    }

    public bool PlaceBet(BetType betType, int amount, float multiplier)
    {
        if (PlayerChips < amount) return false;

        if (!currentBets.ContainsKey(betType))
            currentBets[betType] = 0;

        currentBets[betType] += amount;
        PlayerChips -= amount;
        UpdateChipsText();
        return true;
    }

    public Dictionary<BetType, int> GetAllBets()
    {
        return new Dictionary<BetType, int>(currentBets); 
    }

    public void ClearAllBets()
    {
        currentBets.Clear();
    }

    public void AddChips(int amount)
    {
        PlayerChips += amount;
        UpdateChipsText();
    }

    public void EvaluateBets(int spinResult)
    {
        var bets = GetAllBets();
        int totalWin = 0;

        foreach (var bet in bets)
        {
            bool isWin = IsBetWin(bet.Key, spinResult);
            if (isWin)
            {
                int multiplier = RoulettePayouts.GetPayoutMultiplier(bet.Key);
                int win = bet.Value * multiplier;
                totalWin += win;
                Debug.Log($"{bet.Key}: WIN : {win}");
            }
            else
            {
                Debug.Log($"{bet.Key}: LOST!");
            }
        }

        if (totalWin > 0)
        {
            AddChips(totalWin);
            Debug.Log($"Total Win: {totalWin}");
        }
        else
        {
            Debug.Log("No Win!");
        }

        ClearAllBets(); 
    }

    public bool IsBetWin(BetType betType, int spinResult)
    {
        // Numbers (Num_0, Num_1, ..., Num_36)
        if (betType.ToString() == $"Num_{spinResult}")
            return true;

        // Colors
        if (betType == BetType.Red && Array.Exists(RouletteBets.RedNumbers, n => n == spinResult))
            return true;
        if (betType == BetType.Black && Array.Exists(RouletteBets.BlackNumbers, n => n == spinResult))
            return true;
        if (betType == BetType.Green && spinResult == 0)
            return true;

        // Even/Odd
        if (betType == BetType.Even && spinResult != 0 && spinResult % 2 == 0)
            return true;
        if (betType == BetType.Odd && spinResult % 2 == 1)
            return true;

        // Low/High
        if (betType == BetType.Low && spinResult >= 1 && spinResult <= 18)
            return true;
        if (betType == BetType.High && spinResult >= 19 && spinResult <= 36)
            return true;

        // Dozen Bets
        if (betType == BetType.First12 && Array.Exists(RouletteBets.First12, n => n == spinResult))
            return true;
        if (betType == BetType.Second12 && Array.Exists(RouletteBets.Second12, n => n == spinResult))
            return true;
        if (betType == BetType.Third12 && Array.Exists(RouletteBets.Third12, n => n == spinResult))
            return true;

        // Column
        if (betType == BetType.Column1 && Array.Exists(RouletteBets.Column1, n => n == spinResult))
            return true;
        if (betType == BetType.Column2 && Array.Exists(RouletteBets.Column2, n => n == spinResult))
            return true;
        if (betType == BetType.Column3 && Array.Exists(RouletteBets.Column3, n => n == spinResult))
            return true;

        //  None Of The Above
        return false;
    }

    private void UpdateChipsText()
    {
        chipsText.text = PlayerChips.ToString("N0");
    }
}