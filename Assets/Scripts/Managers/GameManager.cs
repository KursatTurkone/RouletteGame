using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    
    private int _currentNumber;
    private int _selectedBetNumber;
    private bool _isSpinning;
    
    private void OnSpinCompleted(int resultNumber)
    {
        _isSpinning = false;
    }

    private void OnSpinButtonPressed()
    {
        if (_isSpinning)
            return;
        _currentNumber = _selectedBetNumber == -1
            ? RouletteNumbers.NumberOrder[Random.Range(0, RouletteNumbers.NumberOrder.Length)]
            : _selectedBetNumber;
        _isSpinning = true;
        GameEvents.SpinRequested(_currentNumber);
    }

    private void SetCurrentSpinNumber(int number)
    {
        _selectedBetNumber = number;
    }

    private void OnEnable()
    {
        GameEvents.OnSpinButtonPressedRequested += OnSpinButtonPressed;
        GameEvents.OnSetSpinNumberRequested += SetCurrentSpinNumber;
        GameEvents.OnSpinCompleted += OnSpinCompleted;
    }

    private void OnDisable()
    {
        GameEvents.OnSpinButtonPressedRequested -= OnSpinButtonPressed;
        GameEvents.OnSetSpinNumberRequested -= SetCurrentSpinNumber;
        GameEvents.OnSpinCompleted -= OnSpinCompleted;
    }
}