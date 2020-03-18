using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System;
using System.IO;
using UnityEngine.SceneManagement;


public class CharacterStatisticsSerializer : MonoBehaviour
{
    public GameObject characterObject;
    public GameObject characterPrefab;
    public GameObject[] characters;

    int characterNumber;
    int otherNumber;
    int nameNumber;

    string sceneName;

    bool neverDone;

    void Start()
    {

    }

    void Update()
    {
        if (!neverDone)
        {
            LoadCharacter();
            neverDone = true;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            print("Q pressed");
            battleWon();
        }
    }

    public void LoadCharacter()
    {
        var openScene = SceneManager.GetActiveScene();
        sceneName = openScene.name;
        var nameData = File.ReadAllText(Application.persistentDataPath + "/SavedCharacters.json");
        psc deserilizedNames = JsonUtility.FromJson<psc>(nameData);



        otherNumber = characterNumber + 1;
        print("character Starting");
        print(deserilizedNames.pps[0].characterName);
        print(nameNumber);
        print("Position " + otherNumber + "(Clone)");
        print(deserilizedNames.pps[nameNumber].characterName);

        var sdsds = characterPrefab.GetComponent<PositionTwoContainer>();
        var sdaw = sdsds.Name;
        foreach (ActiveCharacters i in deserilizedNames.pps)
        {
            if (deserilizedNames.pps[nameNumber].characterName == sdaw)
            {
                var fileData = File.ReadAllText(Application.persistentDataPath + "/" + deserilizedNames.pps[nameNumber].characterName + ".json");
                SavedCharacters deserilizedData = JsonUtility.FromJson<SavedCharacters>(fileData);

                var characterPrefabStats = characterPrefab.GetComponent<PositionTwoContainer>();
                characterPrefabStats.level = deserilizedData.currentStats[characterNumber].level;
                characterPrefabStats.HP = deserilizedData.currentStats[characterNumber].HP;
                characterPrefabStats.MP = deserilizedData.currentStats[characterNumber].MP;
                characterPrefabStats.defense = deserilizedData.currentStats[characterNumber].defense;
                characterPrefabStats.speed = deserilizedData.currentStats[characterNumber].speed;
                characterPrefabStats.strength = deserilizedData.currentStats[characterNumber].strength;
                characterPrefabStats.battlePower = deserilizedData.currentStats[characterNumber].battlePower;

                SavedCharacters.savedStats = deserilizedData.currentStats;

                sdsds.HP = deserilizedData.currentStats[characterNumber].HP;
                print("character loaded " + characterNumber);
                var characterStats = characters[characterNumber].GetComponent<characterStats>();

            }
            else
            {
                nameNumber += 1;
            }
        }
    }

    public void battleWon()
    {
        characterNumber = 0;

        otherNumber = characterNumber + 1;
        foreach (GameObject o in characters)
        {
            print("Position " + otherNumber + "(Clone)");
            characterObject = GameObject.Find("Position " + otherNumber + "(Clone)");
            var characterScriptObject = characterObject.GetComponent<PositionTwoContainer>();

            var fileData = File.ReadAllText(Application.persistentDataPath + "/" + characterScriptObject.Name + ".json");
            SavedCharacters deserilizedData = JsonUtility.FromJson<SavedCharacters>(fileData);
            string currentCharacter = deserilizedData.currentStats[0].characterName;

            SavedCharacters.savedStats = deserilizedData.currentStats;

            SavedCharacters.savedStats[0].HP = characterScriptObject.HP;

            print(SavedCharacters.savedStats[0].characterName);
            print(characterNumber);

            var varu = new SavedCharacters() { currentStats = SavedCharacters.savedStats };

            string character = JsonUtility.ToJson(varu);
            File.WriteAllText(Application.persistentDataPath + "/" + currentCharacter + ".json", character);
            otherNumber += 1;
        }
    }
}

[Serializable]
public class SavedCharacters
{
    public static SavedCharacters instance;

    void Awake()
    {
        instance = this;
    }

    public List<Stats> currentStats = new List<Stats>();
    public static List<Stats> savedStats = new List<Stats>();
}
[Serializable]
public class psc
{
    public static psc instance;

    void Awake()
    {
        instance = this;
    }

    public List<ActiveCharacters> pps = new List<ActiveCharacters>();
    public static List<ActiveCharacters> Spps = new List<ActiveCharacters>();
}
[Serializable]
public class ActiveCharacters
{
    public string characterName;
}

[Serializable]
public class Stats
{
    public static Stats instance;
    void Awake()
    {
        instance = this;
    }
    public string characterName;
    public static string CcharacterName;
    public int HP;
    public static int CHP;
    public int MP;
    public int battlePower;
    public int strength;
    public int speed;
    public int defense;
    public int level;
}
