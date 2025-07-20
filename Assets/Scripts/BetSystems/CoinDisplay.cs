using TMPro;
using UnityEngine;

public class CoinDisplay : MonoBehaviour
{
    [Header("References")] [SerializeField] private TextMeshPro text;
    [SerializeField] private AudioSource audioSource;

    public void SetAmount(int amount)
    {
        text.text = ShortenAmount(amount);
    }

    string ShortenAmount(int amount)
    {
        if (amount >= 1_000_000)
            return (amount / 1_000_000f).ToString("0.#") + "M";
        if (amount >= 1_000)
            return (amount / 1_000f).ToString("0.#") + "k";
        return amount.ToString();
    }

    public void PlayPutCoinSound()
    {
        audioSource.Play();
    }
}