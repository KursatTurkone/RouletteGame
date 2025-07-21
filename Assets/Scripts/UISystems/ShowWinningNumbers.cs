using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowWinningNumbers : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI winnerNumbersText;
   private void OnEnable()
   {
      winnerNumbersText.SetText(GameManager.Instance.betManager.WinningNumbers.Count > 0
         ? string.Join(", ", GameManager.Instance.betManager.WinningNumbers)
         : "No winning numbers yet");
   }
}
