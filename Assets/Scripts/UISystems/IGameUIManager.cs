using UnityEngine;

public interface IGameUIManager
{
    void ShowResultNotification(bool win, int amount, int number, Color color);
    void UpdateChipsText(int chips);
}