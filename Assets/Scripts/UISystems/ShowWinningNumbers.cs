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
      winnerNumbersText.SetText(GameManager.Instance.WinningNumbers.Count > 0
         ? string.Join(", ", GameManager.Instance.WinningNumbers)
         : "No winning numbers yet");
   }
}
