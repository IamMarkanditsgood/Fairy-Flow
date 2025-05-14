using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static SaveLoadManager;

public class SaveLoadManager : MonoBehaviour
{
    private static string SavePath => Application.persistentDataPath + "/save.json";

    [System.Serializable]
    public class SaveData
    {
        public List<ChickenSaveData> chickens;
        public List<int> unlockedLevels;
    }

    [System.Serializable]
    private class ChickenSaveWrapper
    {
        public List<ChickenSaveData> Chickens;

        public ChickenSaveWrapper(List<ChickenSaveData> chickens)
        {
            Chickens = chickens;
        }
    }

    public static void SaveChickens(List<MergeChicken> chickens)
    {
        Debug.Log(SavePath);
        SaveData saveData = new SaveData
        {
            chickens = chickens.Select(c => new ChickenSaveData(c.Level, c.transform.position)).ToList(),
            unlockedLevels = GameManager.Instance.GetUnlockedLevels().ToList()
        };

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, json);
    }

    public static SaveData LoadSaveData()
    {
        if (!File.Exists(SavePath))
            return null;

        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public static List<ChickenSaveData> LoadChickens()
    {
        var save = LoadSaveData();
        return save?.chickens ?? new List<ChickenSaveData>();
    }

    public static List<int> LoadUnlockedLevels()
    {
        var save = LoadSaveData();
        return save?.unlockedLevels ?? new List<int>();
    }
}
