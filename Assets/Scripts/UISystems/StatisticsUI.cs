using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatisticsUI : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI winnerNumbersText;
   [SerializeField] private TextMeshProUGUI totalSpinsText;
   [SerializeField] private TextMeshProUGUI totalWinsText;
   [SerializeField] private TextMeshProUGUI totalLossesText;
   [SerializeField] private TextMeshProUGUI totalProfitText;
   [SerializeField] private TextMeshProUGUI totalMoneyLossText;
   private void OnEnable()
   {
      winnerNumbersText.SetText(GameManager.Instance.betManager.WinningNumbers.Count > 0
         ? string.Join(", ", GameManager.Instance.betManager.WinningNumbers)
         : "No winning numbers yet");
      UpdateStatisticsUI();
   }

   private void UpdateStatisticsUI()
   {
      var betManager = GameManager.Instance.betManager;
      totalSpinsText.SetText(betManager.TotalSpins.ToString());
      totalWinsText.SetText(betManager.TotalWins.ToString());
      totalLossesText.SetText(betManager.TotalLosses.ToString());
      totalProfitText.SetText(betManager.TotalProfit.ToString());
      totalMoneyLossText.SetText(betManager.TotalMoneyLoss.ToString());
   }

}
