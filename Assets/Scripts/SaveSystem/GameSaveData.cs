using System.Collections.Generic;

[System.Serializable]
public class GameSaveData
{
    public int currentMoney;
    public List<int> winningNumbers = new List<int>();
}