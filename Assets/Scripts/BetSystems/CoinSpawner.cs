using System;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private BetManager betManager; 
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private RouletteTableRaycaster raycaster;
    private readonly Dictionary<string, GameObject> _activeCoins = new Dictionary<string, GameObject>();
    private readonly Dictionary<string, int> _coinAmounts = new Dictionary<string, int>();
    private readonly Queue<GameObject> _coinPool = new Queue<GameObject>();


    public void DropCoinToPosition(Vector3 targetPos, string key, int amount)
    {
        _coinAmounts[key] = amount;

        _activeCoins.TryGetValue(key, out var oldCoin);

        GameObject coin = GetCoin();
        coin.transform.position = spawnPoint.position;
        coin.SetActive(true);

        coin.TryGetComponent(out CoinDisplay coinDisplay);
        if (coinDisplay)
            coinDisplay.SetAmount(amount);
        _activeCoins[key] = coin;

        MyTween.JumpTo(this, coin.transform, targetPos, 2.5f, 0.65f, () =>
        {
            if (oldCoin != null && oldCoin != coin)
            {
                oldCoin.SetActive(false);
                _coinPool.Enqueue(oldCoin);
            }

            coinDisplay.PlayPutCoinSound();
        }, EaseType.OutCubic);
    }


    private GameObject GetCoin()
    {
        if (_coinPool.Count > 0)
            return _coinPool.Dequeue();
        else
            return Instantiate(coinPrefab);
    }

    public void DestroyAllCoins()
    {
        foreach (var coin in _activeCoins.Values)
        {
            if (coin)
            {
                coin.SetActive(false);
                _coinPool.Enqueue(coin);
            }
        }

        _coinAmounts.Clear();
        _activeCoins.Clear();
    }
    public void RestoreAllCoins(Dictionary<string, int> chipData)
    {
        foreach (var kvp in chipData)
        {
            string key = kvp.Key;
            int amount = kvp.Value;
            Vector3 pos;

            if (int.TryParse(key, out int num)) 
                pos = raycaster.GetCellCenter(num);
            else if (key.Contains("-")) 
            {
                var nums = key.Split('-');
                int[] numbers = Array.ConvertAll(nums, int.Parse);
                pos = raycaster.GetCellsCenter(numbers);
            }
            else 
            {
                pos = betManager.GetSpecialAreaPosition(key); 
            }
            DropCoinToPosition(pos, key, amount);
        }
    }

}