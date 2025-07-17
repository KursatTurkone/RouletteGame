using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetManager : MonoBehaviour
{
    public int CurrentBetAmount { get; private set; } = 5000;
    public int BetStep = 5000;
    public int MinBet = 5000;
    public int MaxBet = 100000;

    public void IncreaseBet()
    {
        CurrentBetAmount = Mathf.Min(CurrentBetAmount + BetStep, MaxBet);
    }
    public void DecreaseBet()
    {
        CurrentBetAmount = Mathf.Max(CurrentBetAmount - BetStep, MinBet);
    }
}
