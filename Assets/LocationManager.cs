using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    public string location;

    GameArea _gameArea;

    string json = ".json";
    string lojson;

    Dictionary<string, GameArea> _areaMap = new Dictionary<string, GameArea>();

    List<string> fileNames = new List<string>();

    void Start()
    {
        lojson = location + json;

        var dir = new DirectoryInfo(Application.persistentDataPath);
        var info = dir.GetFiles();
        foreach (FileInfo file in info)
        {
            fileNames.Add(file.FullName);
            print(file.FullName);
        }

        print(fileNames.Count);

        var fileData = File.ReadAllText(Application.persistentDataPath + "/" + lojson);

        GameArea deserializedData = JsonUtility.FromJson<GameArea>(fileData);

        GameArea.Name = deserializedData.savedName;

        print(deserializedData.savedName);
        _areaMap[deserializedData.savedName] = deserializedData;
    }

    void Update()
    {
        _gameArea = _areaMap.ContainsKey(location) ? _areaMap[location] : throw new Exception($"unknown location: {location}");
    }
}

public class Enemy
{
    public string name;
    public int HP;
}

public class InventoryItem
{
}

public class StoryEventTrigger
{
}
[System.Serializable]
public class GameArea
{
    public static GameArea instance = null;
    void Awake()
    {
        instance = this;
    }
    public static string Name;
    public string savedName;

    public Entity[] AvailableEnemies { get; set; }

    public InventoryItem[] AvailableItems { get; set; }

    public StoryEventTrigger[] GameEvents { get; set; }


}
