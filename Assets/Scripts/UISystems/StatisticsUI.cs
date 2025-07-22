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
      winnerNumbersText.SetText(RouletteStatisticsStore.Data.winningNumbers.Count > 0
         ? string.Join(", ", RouletteStatisticsStore.Data.winningNumbers)
         : "No winning numbers yet");
      UpdateStatisticsUI();
   }

   private void UpdateStatisticsUI()
   {
      totalSpinsText.SetText(RouletteStatisticsStore.Data.totalSpins.ToString());
      totalWinsText.SetText(RouletteStatisticsStore.Data.totalWins.ToString());
      totalLossesText.SetText(RouletteStatisticsStore.Data.totalLosses.ToString());
      totalProfitText.SetText(RouletteStatisticsStore.Data.totalProfit.ToString());
      totalMoneyLossText.SetText(RouletteStatisticsStore.Data.totalMoneyLoss.ToString());
   }

}
