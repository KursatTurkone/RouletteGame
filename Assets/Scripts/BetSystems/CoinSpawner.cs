using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public Transform spawnPoint;
    public RouletteTableRaycaster raycaster;

    private readonly List<GameObject> activeCoins = new List<GameObject>();
    private readonly Queue<GameObject> coinPool = new Queue<GameObject>();

    public void DropCoinToNumbers(int[] numbers)
    {
        Vector3 targetPos = (numbers.Length == 1)
            ? raycaster.GetCellCenter(numbers[0])
            : raycaster.GetCellsCenter(numbers);

        SpawnAndJump(targetPos);
    }

    public void DropCoinToPosition(Vector3 targetPos)
    {
        SpawnAndJump(targetPos);
    }

    private void SpawnAndJump(Vector3 targetPos)
    {
        GameObject coin = GetCoin();
        coin.transform.position = spawnPoint.position;
        coin.SetActive(true);
        activeCoins.Add(coin);

        MyTween.JumpTo(this, coin.transform, targetPos, 2.5f, 0.65f, null, EaseType.OutCubic);
    }
    
    private GameObject GetCoin()
    {
        if (coinPool.Count > 0)
        {
            return coinPool.Dequeue();
        }
        else
        {
            return Instantiate(coinPrefab);
        }
    }
    
    public void DestroyAllCoins()
    {
        foreach (var coin in activeCoins)
        {
            if (coin != null)
            {
                coin.SetActive(false);
                coinPool.Enqueue(coin);
            }
        }
        activeCoins.Clear();
    }
}