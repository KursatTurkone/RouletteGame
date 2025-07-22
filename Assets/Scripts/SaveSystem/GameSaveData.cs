using System.Collections.Generic;

[System.Serializable]
public class GameSaveData
{
    public int currentMoney = 100000;
    public List<int> winningNumbers = new List<int>();
    public List<KeyValue> activeChips = new List<KeyValue>();
    public int totalSpins;
    public int totalWins;
    public int totalLosses;
    public int totalProfit;
    public int totalMoneyLoss; 
}
[System.Serializable]
public class KeyValue
{
    public string key;
    public int value;
}
