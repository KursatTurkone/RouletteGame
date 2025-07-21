
using System.Collections.Generic;
using UnityEngine;

public interface ICoinService
{
    void DropCoinToPosition(Vector3 pos, string key, int amount);
    void DestroyAllCoins();
    void RestoreAllCoins(Dictionary<string, int> activeChips);
}