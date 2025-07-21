using System.Collections.Generic;

[System.Serializable]
public class GameSaveData
{
    public int currentMoney;
    public List<int> winningNumbers = new List<int>();
    public List<KeyValue> activeChips = new List<KeyValue>();
}
[System.Serializable]
public class KeyValue
{
    public string key;
    public int value;
}
