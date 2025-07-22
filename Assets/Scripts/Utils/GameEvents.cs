public static class GameEvents
{
    public static event System.Action OnWin;
    public static event System.Action OnLose;

    public static void TriggerWin() => OnWin?.Invoke();
    public static void TriggerLose() => OnLose?.Invoke();
    public static event System.Action<int> OnSpinCompleted;

    public static void SpinCompleted(int resultNumber)
    {
        OnSpinCompleted?.Invoke(resultNumber);
    }

    public static event System.Action OnBetIncreased;
    public static event System.Action OnBetDecreased;

    public static void BetIncreased() => OnBetIncreased?.Invoke();
    public static void BetDecreased() => OnBetDecreased?.Invoke();
    public static event System.Action<int> OnChipsAdjusted;
    public static void ChipsAdjusted(int delta) => OnChipsAdjusted?.Invoke(delta);
    public static event System.Action<int> OnBetAmountChanged;
    public static void BetAmountChanged(int amount) => OnBetAmountChanged?.Invoke(amount);
    public static event System.Action<int> OnSpinRequested;
    public static void SpinRequested(int number) => OnSpinRequested?.Invoke(number);
    public static event System.Action OnSpinButtonPressedRequested;
    public static void SpinButtonPressedRequested() => OnSpinButtonPressedRequested?.Invoke();
    public static event System.Action<int> OnSetSpinNumberRequested;
    public static void SetSpinNumberRequested(int number) => OnSetSpinNumberRequested?.Invoke(number);
    public static event System.Action OnClearStatisticsRequested;
    public static void ClearStatisticsRequested() => OnClearStatisticsRequested?.Invoke();
    public static event System.Action OnClearBetsRequested;
    public static void ClearBetsRequested() => OnClearBetsRequested?.Invoke();
}
