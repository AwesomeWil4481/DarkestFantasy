using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocationManager : MonoBehaviour
{
    GameObject texObject;

    public string location;

    GameArea _gameArea;

    int enemyNumber;
    int enemySpriteNumber = 0;

    bool spriteFound;

    public GameObject[] _enemies;

    SpriteRenderer enemySprite;

    string json = ".json";
    string lojson;

    Dictionary<string, GameArea> _areaMap = new Dictionary<string, GameArea>();

    public SpriteRenderer backround;
    public List<Sprite> Backrounds = new List<Sprite>();
    int backroundNumber;

    public List<Sprite> enemySprites = new List<Sprite>();
    List<string> fileNames = new List<string>();
    
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Fighting Scene") { 
        lojson = location+".json";

        var dir = new DirectoryInfo(Application.persistentDataPath);
        var info = dir.GetFiles();
        foreach (FileInfo file in info)
        {
            fileNames.Add(file.FullName);
        }

            foreach (GameObject i in _enemies)
            {
                var fileData = File.ReadAllText(Application.persistentDataPath + "/" + lojson);
                GameArea deserializedData = JsonUtility.FromJson<GameArea>(fileData);
                var enemyStats = _enemies[enemyNumber].GetComponent<EnemyStats>();

                texObject = GameObject.Find("");

                GameArea.Name = deserializedData.savedName;
                GameArea.availableEnemies = deserializedData.savedEnemies;
                enemyStats.Name = deserializedData.savedEnemies[enemyNumber].enemyName;
                enemyStats.HP = deserializedData.savedEnemies[enemyNumber].HP;
                enemyStats.level = deserializedData.savedEnemies[enemyNumber].level;
                enemyStats.MP = deserializedData.savedEnemies[enemyNumber].MP;
                enemyStats.defense = deserializedData.savedEnemies[enemyNumber].defense;
                enemyStats.speed = deserializedData.savedEnemies[enemyNumber].speed;
                enemyStats.strength = deserializedData.savedEnemies[enemyNumber].strength;
                enemyStats.battlePower = deserializedData.savedEnemies[enemyNumber].battlePower;
                enemyStats.lootTable = deserializedData.savedEnemies[enemyNumber].lootTable;
                print(enemyStats.lootTable[0].Name);

                foreach (Sprite w in Backrounds)
                {
                    if (deserializedData.Backround == Backrounds[backroundNumber].name)
                    {
                        backround.sprite = Backrounds[backroundNumber];
                    }
                    else
                    {
                        backroundNumber += 1;
                    }
                }
                _enemies[enemyNumber].name = deserializedData.savedEnemies[enemyNumber].enemyName;
                enemySpriteNumber = 0; enemySprite = _enemies[enemyNumber].GetComponentInChildren<SpriteRenderer>();

                foreach (Sprite a in enemySprites)
                {

                    if (deserializedData.savedEnemies[enemyNumber].enemyName == enemySprites[enemySpriteNumber].name)
                    {
                        enemySprite.sprite = enemySprites[enemySpriteNumber];

                        break;
                    }

                    else
                    {
                        enemySpriteNumber += 1;
                    }
                }


                _areaMap[deserializedData.savedName] = deserializedData;
                enemyNumber += 1;
                if (enemyNumber >= deserializedData.savedEnemies.Count)
                {

                    break;
                }
            }
        }
    }
    void Update()
    {
        //_gameArea = _areaMap.ContainsKey(LocationContainer.instance._currentLocation) ? _areaMap[location] : throw new Exception($"unknown location: {LocationContainer.instance._currentLocation}");
    }
}
[Serializable]
public class Enemy
{
    public string enemyName;
    public int level;
    public int HP;
    public int MP;
    public int defense;
    public int speed;
    public int strength;
    public int battlePower;
    public Vector3 HPText;
    public List<Item> lootTable = new List<Item>();
}

public class InventoryItem
{
}

public class StoryEventTrigger
{
}
[Serializable]
public class GameArea
{
    public static GameArea instance = null;
    void Awake()
    {
        instance = this;
    }
    public static string Name;
    public string savedName;

    public List<Enemy> savedEnemies = new List<Enemy>();
    public static List<Enemy> availableEnemies = new List<Enemy>();

    public string Backround;

    public InventoryItem[] AvailableItems { get; set; }

    public StoryEventTrigger[] GameEvents { get; set; }


}
