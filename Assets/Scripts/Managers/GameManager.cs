using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RouletteAnimator rouletteAnimator;
    public static GameManager Instance { get; private set; }
    private IRouletteAnimator _animator;
    private int _currentNumber;
    private int _selectedBetNumber;
    public BetManager betManager; 
    private GameSaveData _saveData = new GameSaveData();
    public List<int> WinningNumbers => _saveData.winningNumbers;

    private void Awake()
    {
        _animator = rouletteAnimator;
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
        betManager.EvaluateBets(resultNumber);
        _saveData.winningNumbers.Add(resultNumber);
        SaveSystem.Save(_saveData, "WinningNumbers");
    }

    public void OnSpinButtonPressed()
    {
        _currentNumber = _selectedBetNumber == -1
            ? rouletteAnimator.NumberOrder[Random.Range(0, rouletteAnimator.NumberOrder.Length)]
            : _selectedBetNumber;
        _animator.AnimateSpinToNumber(_currentNumber, OnSpinCompleted);
    }

    public void SetCurrentSpinNumber(int number)
    {
        _selectedBetNumber = number;
    }
}