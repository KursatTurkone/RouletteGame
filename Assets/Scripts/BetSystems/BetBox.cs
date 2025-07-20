using UnityEngine;

public class BetBox : MonoBehaviour
{
    public BetType betType;
    public void OnBetPlaced(int amount)
    {
        Debug.Log($"Bet placed on {betType}: {amount}");
    }
}