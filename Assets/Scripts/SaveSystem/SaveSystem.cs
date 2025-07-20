using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static void Save<T>(T data, string key)
    {
        File.WriteAllText(GetPath(key), JsonUtility.ToJson(data, true));
    }
    
    public static T Load<T>(string key)
    {
        string path = GetPath(key);
        if (!File.Exists(path))
            return default;
        return JsonUtility.FromJson<T>(File.ReadAllText(path));
    }
    
    public static void Delete(string key)
    {
        string path = GetPath(key);
        if (File.Exists(path))
            File.Delete(path);
    }
    private static string GetPath(string key)
    {
        return Path.Combine(Application.persistentDataPath, key + ".json");
    }
}