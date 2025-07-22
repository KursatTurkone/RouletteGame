public static class GameEvents
{
    public static event System.Action OnWin;
    public static event System.Action OnLose;

    public static void TriggerWin() => OnWin?.Invoke();
    public static void TriggerLose() => OnLose?.Invoke();
    
}
