using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private RouletteTableRaycaster raycaster;
    private readonly Dictionary<string, GameObject> activeCoins = new Dictionary<string, GameObject>();
    private readonly Dictionary<string, int> coinAmounts = new Dictionary<string, int>();
    private readonly Queue<GameObject> coinPool = new Queue<GameObject>();


    public void DropCoinToPosition(Vector3 targetPos, string key, int amount)
    {
        int total = amount;
        if (coinAmounts.TryGetValue(key, out int prev))
            total += prev;
        coinAmounts[key] = total;

        GameObject oldCoin = null;
        activeCoins.TryGetValue(key, out oldCoin);

        GameObject coin = GetCoin();
        coin.transform.position = spawnPoint.position;
        coin.SetActive(true);

        coin.TryGetComponent(out CoinDisplay coinDisplay);
        if (coinDisplay != null)
            coinDisplay.SetAmount(total);
        activeCoins[key] = coin;

        MyTween.JumpTo(this, coin.transform, targetPos, 2.5f, 0.65f, () =>
        {
            if (oldCoin != null && oldCoin != coin)
            {
                oldCoin.SetActive(false);
                coinPool.Enqueue(oldCoin);
            }

            coinDisplay.PlayPutCoinSound();
        }, EaseType.OutCubic);
    }


    private GameObject GetCoin()
    {
        if (coinPool.Count > 0)
            return coinPool.Dequeue();
        else
            return Instantiate(coinPrefab);
    }

    public void DestroyAllCoins()
    {
        foreach (var coin in activeCoins.Values)
        {
            if (coin != null)
            {
                coin.SetActive(false);
                coinPool.Enqueue(coin);
            }
        }

        coinAmounts.Clear();
        activeCoins.Clear();
    }
}