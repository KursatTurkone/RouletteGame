using UnityEngine;

public class BetBox : MonoBehaviour
{
    public BetType betType;
    public float Multiplier => RoulettePayouts.GetPayoutMultiplier(betType); 
    public Renderer highlightRenderer; 

    public void OnBetPlaced(int amount)
    {
      
        Debug.Log($"Bet placed on {betType}: {amount}");
        if (highlightRenderer != null)
            highlightRenderer.material.SetColor("_EmissionColor", Color.yellow);
    }
}