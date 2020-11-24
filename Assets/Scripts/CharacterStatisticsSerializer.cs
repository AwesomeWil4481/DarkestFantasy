using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterStatisticsSerializer : MonoBehaviour
{
    public static CharacterStatisticsSerializer _instance;

    public List<PositionTwoContainer> currentParty { get; set; } = new List<PositionTwoContainer>();

    public GameObject characterObject;
    public GameObject posOnePrefab;
    public GameObject posTwoPrefab;
    public GameObject posThreePrefab;
    public GameObject posFourPrefab;
    public GameObject characterPrefab;
    public GameObject[] activePartyMembers;
    public Sprite[] characterPortraits;
    public GameObject[] statBlocks;

    int characterNumber;
    int battlecharnumber;
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
            if (SceneManager.GetActiveScene().name != "Fighting Scene")
            {
                LoadCharacter();
            }
            neverDone = true;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            print("Q pressed");
            SaveToPrefab();
        }
    }

    public void LoadCharacter()
    {
        var openScene = SceneManager.GetActiveScene();
        sceneName = openScene.name;
        int number = 0;
        foreach (GameObject _object in activePartyMembers)
        {
            var currentMember = _object.GetComponent<PositionTwoContainer>();

            int positionNumber = SavedCharacters.Instance().currentStats[number]._position;
            statBlocks[positionNumber].SetActive(true);

            print("Position Number : " + positionNumber);

            GameObject.Find("Name " + (positionNumber + 1)).GetComponent<Text>().text = SavedCharacters.Instance().currentStats[number].characterName;
            currentMember.HP = SavedCharacters.Instance().currentStats[number].HP;
            GameObject.Find("HP Number " + (positionNumber + 1)).GetComponent<Text>().text = currentMember.HP.ToString();
            currentMember.MP = SavedCharacters.Instance().currentStats[number].MP;
            GameObject.Find("MP Number " + (positionNumber + 1)).GetComponent<Text>().text = currentMember.MP.ToString();
            currentMember.level = SavedCharacters.Instance().currentStats[number].level;
            GameObject.Find("LV Number " + (positionNumber + 1)).GetComponent<Text>().text = currentMember.level.ToString();
            currentMember.defense = SavedCharacters.Instance().currentStats[number].defense;
            currentMember.speed = SavedCharacters.Instance().currentStats[number].speed;
            currentMember.strength = SavedCharacters.Instance().currentStats[number].strength;
            currentMember.battlePower = SavedCharacters.Instance().currentStats[number].battlePower;
            currentMember._position = SavedCharacters.Instance().currentStats[number]._position;

            currentParty.Add(currentMember);

            int currentSprite = 0;
            foreach (Sprite o in characterPortraits)
            {
                if (characterPortraits[currentSprite].name == SavedCharacters.Instance().currentStats[number].characterName)
                {
                    var portraitName = "Portrait " + (positionNumber + 1);


                    var portrait = GameObject.Find(portraitName);
                    if (portrait == null)
                    {
                        Debug.LogError("portrait was null");
                    }

                    var portraitSprite = portrait.GetComponent<Image>();
                    if (portraitSprite == null)
                    {
                        Debug.LogError("component was null");
                    }
                    portraitSprite.sprite = o;
                    break;
                }
                else
                {
                    currentSprite += 1;
                }     
            }
            number += 1;
        }

        GameObject.Find("Main Menu").SetActive(false);
    }

    //    //var nameData = File.ReadAllText(Application.persistentDataPath + "/SavedCharacters.json");
    //    //psc deserilizedNames = JsonUtility.FromJson<psc>(nameData);

    //    //otherNumber = characterNumber + 1;
    //    //print("character Starting");
    //    //print(deserilizedNames.pps[0].characterName);
    //    //print(nameNumber);
    //    //print(deserilizedNames.pps[nameNumber].characterName);

    //    //var sdsds = characterPrefab.GetComponent<PositionTwoContainer>();
    //    //var sdaw = sdsds.Name;
    //    //foreach (Stats i in deserilizedNames.pps)
    //    //{
    //    //    if (deserilizedNames.pps[nameNumber].characterName == sdaw)
    //    //    {
    //    //        var fileData = File.ReadAllText(Application.persistentDataPath + "/" + deserilizedNames.pps[nameNumber].characterName + ".json");
    //    //        SavedCharacters deserilizedData = JsonUtility.FromJson<SavedCharacters>(fileData);

    //    //        var characterPrefabStats = characterPrefab.GetComponent<PositionTwoContainer>();
    //    //        characterPrefabStats.Name = deserilizedData.currentStats[characterNumber].characterName;
    //    //        characterPrefabStats.level = deserilizedData.currentStats[characterNumber].level;
    //    //        characterPrefabStats.HP = deserilizedData.currentStats[characterNumber].HP;
    //    //        characterPrefabStats.MP = deserilizedData.currentStats[characterNumber].MP;
    //    //        characterPrefabStats.defense = deserilizedData.currentStats[characterNumber].defense;
    //    //        characterPrefabStats.speed = deserilizedData.currentStats[characterNumber].speed;
    //    //        characterPrefabStats.strength = deserilizedData.currentStats[characterNumber].strength;
    //    //        characterPrefabStats.battlePower = deserilizedData.currentStats[characterNumber].battlePower;
    //    //        characterPrefabStats._position = deserilizedData.currentStats[characterNumber]._position;

    //    //        SavedCharacters.savedStats = deserilizedData.currentStats;

    //    //        sdsds.HP = deserilizedData.currentStats[characterNumber].HP;
    //    //        print("character loaded " + characterNumber);
    //    //        var characterStats = characters[characterNumber].GetComponent<characterStats>();
    //    //        GameObject.Find("Name "+(characterNumber+1)).GetComponent<Text>().text = characterPrefabStats.Name;
    //    //        GameObject.Find("LV Number "+(characterNumber+1)).GetComponent<Text>().text = characterPrefabStats.level.ToString();
    //    //        GameObject.Find("HP Number "+(characterNumber+1)).GetComponent<Text>().text = characterPrefabStats.HP.ToString();
    //    //        GameObject.Find("MP Number "+(characterNumber+1)).GetComponent<Text>().text = characterPrefabStats.MP.ToString();
    //    //        GameObject.Find("Select "+(characterNumber+1)+" Equip").GetComponentInChildren<Text>().text = characterPrefabStats.Name;

    //    //        int currentSprite = 0;
    //    //        foreach(Sprite o in characterPortraits)
    //    //        {
    //    //            if(characterPortraits[currentSprite].name == characterPrefabStats.Name)
    //    //            {
    //    //                GameObject.Find("Portrait " + (characterNumber + 1)).GetComponent<Image>().sprite = o;
    //    //                print(o);
    //    //                break;
    //    //            }
    //    //            else
    //    //            {
    //    //                currentSprite +=1;
    //    //            }
    //    //        }
    //    //    }   
    //    //    else
    //    //    {
    //    //        nameNumber += 1;
    //    //    }
    //    //}
    //}

    //public Stats LoadStats()
    //{
    //    //var fileData = File.ReadAllText(Application.persistentDataPath + "/" + characterScriptObject.Name + ".json");
    //    SavedCharacters deserilizedData = JsonUtility.FromJson<SavedCharacters>(fileData);
    //    string currentCharacter = deserilizedData.currentStats[0].characterName;
    //}
    public void SaveToPrefab()
    {
        foreach (GameObject s in GameObject.FindGameObjectsWithTag("battlecharacter"))
        {
            battlecharnumber = +1;
            print("battlecharacternumbewr = " + battlecharnumber);
            var currentchar = GameObject.Find("Position " + battlecharnumber + "(Clone)").GetComponent<PositionTwoContainer>();
            if (battlecharnumber == 1)
            {
                posOnePrefab.GetComponent<PositionTwoContainer>().HP = currentchar.HP;
            }
        }
    }

    public static void SaveGame(string SaveSelected)
    {
        string character = JsonUtility.ToJson(SavedCharacters.Instance());
        Debug.Log($"Characters Saved: {character}");
        Debug.Log($"{Application.persistentDataPath}/Save {SaveSelected}/SavedCharacters.json, {character}");
        File.WriteAllText(Application.persistentDataPath + "/Save " + SaveSelected + "/SavedCharacters.json", character);
    }

    //public void SaveToJson()
    //{
    //    characterNumber = 0;
    //    otherNumber = characterNumber + 1;
    //    foreach (GameObject o in characters)
    //    {
    //        print("Position " + otherNumber + "(Clone)");
    //        characterObject = GameObject.Find("Position " + otherNumber + "(Clone)");
    //        var characterScriptObject = characterObject.GetComponent<PositionTwoContainer>();

    //        var fileData = File.ReadAllText(Application.persistentDataPath + "/" + characterScriptObject.Name + ".json");
    //        SavedCharacters deserilizedData = JsonUtility.FromJson<SavedCharacters>(fileData);
    //        string currentCharacter = deserilizedData.currentStats[0].characterName;

    //        SavedCharacters.savedStats = deserilizedData.currentStats;
    //        SavedCharacters.savedStats[0].HP = characterScriptObject.HP;

    //        print(SavedCharacters.savedStats[0].characterName);
    //        print(characterNumber);

    //        var varu = new SavedCharacters() { currentStats = SavedCharacters.savedStats };

    //        string character = JsonUtility.ToJson(varu);
    //        File.WriteAllText(Application.persistentDataPath + "/" + currentCharacter + ".json", character);
    //        otherNumber += 1;
    //    }
    //}
}

[Serializable]
public class SavedCharacters
{
    private static SavedCharacters _instance;

    public static SavedCharacters Instance()
    {
        if (_instance == null)
        {
            _instance = new SavedCharacters();
        }
        return _instance;
    }

    [SerializeField]
    private List<Stats> stats = new List<Stats>();

    public List<Stats> currentStats { get { return stats; } }
}
[Serializable]
public class psc
{
    public static psc instance;

    void Awake()
    {
        instance = this;
    }

    public List<Stats> pps = new List<Stats>();
    public static List<Stats> Spps = new List<Stats>();
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
    public int _position;
    public int HP;
    public static int CHP;
    public int MP;
    public int battlePower;
    public int strength;
    public int speed;
    public int defense;
    public int level;
}
