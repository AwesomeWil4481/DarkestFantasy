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
    public static CharacterStatisticsSerializer Instance;
   
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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Destroy(gameObject); }
    }

    void Update()
    {
        //if (!neverDone)
        //{
        //    if (SceneManager.GetActiveScene().name != "Fighting Scene" || SceneManager.GetActiveScene().name != "MainMenu")
        //    {
        //        LoadCharacter();
        //    }
        //    neverDone = true;
        //}

        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    print("Q pressed");
        //    SaveToPrefab();
        //}
    }

    public void LoadCharacter()
    {
        var openScene = SceneManager.GetActiveScene();
        sceneName = openScene.name;
        int number = 0;
        int num = -1;
        bool done = false;
        while(!done)
        {
            number += 1;
            num += 1;

            if (number >= 4)
            {
                done = true;
            }

            if (SavedCharacters.Instance().DcurrentStats.ContainsKey(number))
            {
                var thing = SavedCharacters.Instance().DcurrentStats[number]._position -= 1 ;

                statBlocks[thing].SetActive(true);

                MenuManager.Instance._statBlocks[thing].GetComponentsInChildren<Text>()[0].text = SavedCharacters.Instance().DcurrentStats[number].characterName;
                //currentMember.HP = SavedCharacters.Instance().currentStats[number].HP;
                MenuManager.Instance._statBlocks[thing].GetComponentsInChildren<Text>()[5].text = SavedCharacters.Instance().DcurrentStats[number].HP.ToString();
                //currentMember.MP = SavedCharacters.Instance().currentStats[number].MP;
                MenuManager.Instance._statBlocks[thing].GetComponentsInChildren<Text>()[6].text = SavedCharacters.Instance().DcurrentStats[number].MP.ToString();
                //currentMember.level = SavedCharacters.Instance().currentStats[number].level;
                MenuManager.Instance._statBlocks[thing].GetComponentsInChildren<Text>()[4].text = SavedCharacters.Instance().DcurrentStats[number].level.ToString();
                //currentMember.defense = SavedCharacters.Instance().currentStats[number].defense;
                //currentMember.speed = SavedCharacters.Instance().currentStats[number].speed;
                //currentMember.strength = SavedCharacters.Instance().currentStats[number].strength;
                //currentMember.battlePower = SavedCharacters.Instance().currentStats[number].battlePower;
                //currentMember._position = SavedCharacters.Instance().currentStats[number]._position;

                int currentSprite = 0;
                foreach (Sprite o in characterPortraits)
                {
                    if (characterPortraits[currentSprite].name == SavedCharacters.Instance().DcurrentStats[number].characterName)
                    {
                        var portrait = MenuManager.Instance._statBlocks[thing];
                        if (portrait == null)
                        {
                            Debug.LogError("portrait was null");
                        }

                        var portraitSprite = portrait.GetComponentsInChildren<Image>();
                        if (portraitSprite == null)
                        {
                            Debug.LogError("component was null");
                        }
                        portraitSprite[1].sprite = o;
                        break;
                    }
                    else
                    {
                        currentSprite += 1;
                    }
                }
            }
            else
            {
            }
            
        }

    }

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
            print("battlecharacternumber = " + battlecharnumber);
            var currentchar = GameObject.Find("Position " + battlecharnumber + "(Clone)").GetComponent<PositionTwoContainer>();
            if (battlecharnumber == 1)
            {
                posOnePrefab.GetComponent<PositionTwoContainer>().UpdateCharacter(currentchar, battlecharnumber.ToString()) ;
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

    private Dictionary<int, Stats> Dstats = new Dictionary<int, Stats>();

    public Dictionary<int, Stats> DcurrentStats { get { return Dstats; } set { } }

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
    public int MP;
    public int battlePower;
    public int strength;
    public int speed;
    public int defense;
    public int level;
    public int stamina;
    public int magic;
    public int attack;
    public int evasion;
    public int magicDefense;
    public int magicEvasion;
    public EquipableItem BodySlot = new EquipableItem {Name = "Empty" };
    public EquipableItem HeadSlot = new EquipableItem { Name = "Empty" };
    public EquipableItem Relic1Slot = new EquipableItem { Name = "Empty" };
    public EquipableItem Relic2Slot = new EquipableItem { Name = "Empty" };
    public EquipableItem LeftHandSlot = new EquipableItem { Name = "Empty" };
    public EquipableItem RightHandSlot = new EquipableItem {Name = "Empty" };
}
