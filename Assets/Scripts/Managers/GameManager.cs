using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
   [HideInInspector] public RouletteAnimator rouletteAnimator;
    public static GameManager Instance { get; private set; }
    private int _currentNumber;
    private int _selectedBetNumber;
    public BetManager betManager;
    private GameSaveData _saveData = new GameSaveData();
    public List<int> WinningNumbers => _saveData.winningNumbers;
    private bool _isSpinning;
    private void Awake()
    {
        var wNumb = SaveSystem.Load<GameSaveData>("WinningNumbers");
        if (wNumb != null)
            _saveData = wNumb;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnSpinCompleted(int resultNumber)
    {
        _isSpinning = false;
        betManager.EvaluateBets(resultNumber);
        _saveData.winningNumbers.Add(resultNumber);
        SaveSystem.Save(_saveData, "WinningNumbers");
    }

    public void OnSpinButtonPressed()
    {
        if (_isSpinning)
            return;
        _currentNumber = _selectedBetNumber == -1
            ? rouletteAnimator.numberOrder[Random.Range(0, rouletteAnimator.numberOrder.Length)]
            : _selectedBetNumber;
        _isSpinning = true;
        rouletteAnimator.AnimateSpinToNumber(_currentNumber, OnSpinCompleted);
    }

    public void SetCurrentSpinNumber(int number)
    {
        _selectedBetNumber = number;
    }
}