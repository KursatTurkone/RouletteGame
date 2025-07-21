public interface ISaveService
{
    void Save(GameSaveData data, string key);
    GameSaveData Load(string key);
}

public class SaveService : ISaveService
{
    public void Save(GameSaveData data, string key) => SaveSystem.Save(data, key);
    public GameSaveData Load(string key) => SaveSystem.Load<GameSaveData>(key) ?? new GameSaveData();
}