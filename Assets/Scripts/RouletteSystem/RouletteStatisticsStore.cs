public static class RouletteStatisticsStore
{
    public static GameSaveData Data { get; private set; } = new GameSaveData();

    public static void Update(GameSaveData newData)
    {
        Data = newData;
    }
}