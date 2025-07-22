public interface ISaveService
{
    void Save(GameSaveData data, string key);
    GameSaveData Load(string key);
}
