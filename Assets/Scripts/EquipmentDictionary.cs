using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Globalization;

[Serializable]
public class MasterEquipmentContainer
{
    private MasterEquipmentContainer()
    {
    }

    private static MasterEquipmentContainer _instance;
    public static MasterEquipmentContainer Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MasterEquipmentContainer();
            }
            return _instance;
        }
    }

    [SerializeField]
    private List<EquipableItem> equipment = new List<EquipableItem>();

    public List<EquipableItem> Equipment { get { return equipment; } }
}

public class SaveTheBooks
{
    public static SaveTheBooks _instance;

    public static SaveTheBooks Instance()
    {
        if (_instance == null)
        {
            _instance = new SaveTheBooks();
        }
        return _instance;
    }

    public List<characterStats> DisplayStats(int saveNum)
    {
        if (File.ReadAllText(Application.persistentDataPath + "/Save " + saveNum + "/SavedCharacters.json") != null)
        {
            var fileData = File.ReadAllText(Application.persistentDataPath + "/Save " + saveNum + "/SavedCharacters.json");
            SavedCharacters deserializedData = JsonUtility.FromJson<SavedCharacters>(fileData);

            List<characterStats> stats = new List<characterStats>();

            foreach (Stats s in deserializedData.currentStats)
            {
                characterStats newStats = new characterStats();

                newStats.Name = s.characterName;
                newStats._position = s._position;

                stats.Add(newStats);
            }

            return stats;
        }
        return null;
    }
    public static void SaveGame(string SaveSelected)
    {
        InventoryManager.instance.SaveGame(SaveSelected);

        characterPosition.instance.CharacterPositionSave(MenuManager.Instance.savePointPos, SaveSelected);

        CharacterStatisticsSerializer.SaveGame(SaveSelected);

        WorldStateManager.Instance().StateSave(SaveSelected);

        string character = JsonUtility.ToJson(MasterEquipmentContainer.Instance);
        File.WriteAllText(Application.persistentDataPath + "/Save " + SaveSelected + "/EquipmentDictionary.json", character);

        character = JsonUtility.ToJson(ActiveScene.Instance());
        File.WriteAllText(Application.persistentDataPath + "/Save " + SaveSelected + "/SavedScene.json", character);

        character = JsonUtility.ToJson(WorldStateList.Instance());
        File.WriteAllText(Application.persistentDataPath + "/Save " + SaveSelected + ActiveScene.Instance().Scene, character);
    }

    public static void NewGame()
    {
        string currentScene;
        { // Loading the scene
            var fileData = File.ReadAllText(Application.streamingAssetsPath + "/Json/Prefab Save" + "/SavedScene.json");
            ActiveScene deserializedData = JsonUtility.FromJson<ActiveScene>(fileData);
            ActiveScene.Instance().Scene = deserializedData.Scene;
            currentScene = deserializedData.Scene;

            SceneManager.LoadScene(deserializedData.Scene);
        }

        { // Loading the World State
            var fileData = File.ReadAllText(Application.streamingAssetsPath + "/Json/Prefab Save/" + currentScene + ".json");
            SaveWorldState deserializedData = JsonUtility.FromJson<SaveWorldState>(fileData);

            if (deserializedData.list.Count != 0)
            {
                WorldStateList.Instance().Edits = new List<WorldObject>();
                foreach (WorldStateSerilization s in deserializedData.list)
                {
                    WorldStateList.Instance().Edits.Add(new WorldObject { ID = s.ID, interacted = s.interacted });
                }
            }
        }

        { // Loading the Items
            var fileData = File.ReadAllText(Application.streamingAssetsPath + "/Json/Prefab Save" + "/Inventory.json");
            ItemList deserializedData = JsonUtility.FromJson<ItemList>(fileData);

            ItemList.Instance().Items.Clear();
            ItemList.Instance().Items.AddRange(deserializedData.Items);

            Debug.Log(ItemList.Instance().Items[1].Name + " of " + ItemList.Instance().Items[1].Count);

            ItemList.Instance().GP = deserializedData.GP;
            Debug.Log("Current GP = " + ItemList.Instance().GP.ToString());
        }

        { // Loading the start position
            var fileData = File.ReadAllText(Application.streamingAssetsPath + "/Json/Prefab Save" + "/savedPosition.json");
            VPos deserializedData = JsonUtility.FromJson<VPos>(fileData);

            VPos.savedPlayerPos = deserializedData.playerPos;
            var value = VPos.savedPlayerPos;
        }

        { // Loading Character Stats
            var fileData = File.ReadAllText(Application.streamingAssetsPath + "/Json/Prefab Save" + "/SavedCharacters.json");
            SavedCharacters deserializedData = JsonUtility.FromJson<SavedCharacters>(fileData);

            SavedCharacters.Instance().currentStats.Clear();
            SavedCharacters.Instance().currentStats.AddRange(deserializedData.currentStats);

            foreach (Stats i in SavedCharacters.Instance().currentStats)
            {
                SavedCharacters.Instance().DcurrentStats[i._position] = i;
                Debug.Log(SavedCharacters.Instance().DcurrentStats[i._position]._position + " is the loaded position of " + SavedCharacters.Instance().DcurrentStats[i._position].characterName);
                Debug.Log(SavedCharacters.Instance().DcurrentStats[i._position].strength + " is the loaded strength of " + SavedCharacters.Instance().DcurrentStats[i._position].characterName);
            }
        }

        { // Loading the Equiment Dictionary
            var fileData = File.ReadAllText(Application.streamingAssetsPath+ "/Json" + "/EquipmentDictionary.json");
            MasterEquipmentContainer deserializedData = JsonUtility.FromJson<MasterEquipmentContainer>(fileData);

            MasterEquipmentContainer.Instance.Equipment.Clear();
            MasterEquipmentContainer.Instance.Equipment.AddRange(deserializedData.Equipment);

            Debug.Log(MasterEquipmentContainer.Instance.Equipment[0].STRPercentBonus.ToString());
        }
    }


    public static void LoadGame(string SaveSelected)
    {
        string currentScene;

        { // Loading the scene
            var fileData = File.ReadAllText(Application.persistentDataPath + "/Save " + SaveSelected + "/SavedScene.json");
            ActiveScene deserializedData = JsonUtility.FromJson<ActiveScene>(fileData);
            ActiveScene.Instance().Scene = deserializedData.Scene;
            currentScene = deserializedData.Scene;

            SceneManager.LoadScene(deserializedData.Scene);
        }

        { // Loading the World State
            var fileData = File.ReadAllText(Application.persistentDataPath + "/Save " + SaveSelected +"/"+ currentScene + ".json");
            SaveWorldState deserializedData = JsonUtility.FromJson<SaveWorldState>(fileData);

            if (deserializedData.list.Count != 0)
            {
                WorldStateList.Instance().Edits = new List<WorldObject>();
                foreach (WorldStateSerilization s in deserializedData.list)
                {
                    WorldStateList.Instance().Edits.Add(new WorldObject { ID = s.ID, interacted = s.interacted });
                }
            }
        }

        { // Loading the Items
            var fileData = File.ReadAllText(Application.persistentDataPath + "/Save " + SaveSelected + "/Inventory.json");
            ItemList deserializedData = JsonUtility.FromJson<ItemList>(fileData);

            ItemList.Instance().Items.Clear();
            ItemList.Instance().Items.AddRange(deserializedData.Items);

            Debug.Log(ItemList.Instance().Items[1].Name+" of "+ItemList.Instance().Items[1].Count);

            ItemList.Instance().GP = deserializedData.GP;
            Debug.Log("Current GP = " + ItemList.Instance().GP.ToString());
        }

        { // Loading the start position
            characterPosition.CharacterPositionLoad(int.Parse(SaveSelected));
        }

        { // Loading Character Stats
            var fileData = File.ReadAllText(Application.persistentDataPath + "/Save " + SaveSelected + "/SavedCharacters.json");
            SavedCharacters deserializedData = JsonUtility.FromJson<SavedCharacters>(fileData);

            SavedCharacters.Instance().currentStats.Clear();
            SavedCharacters.Instance().currentStats.AddRange(deserializedData.currentStats);

            foreach(Stats i in SavedCharacters.Instance().currentStats)
            {
                SavedCharacters.Instance().DcurrentStats[i._position] = i;
                Debug.Log(SavedCharacters.Instance().DcurrentStats[i._position]._position+" is the loaded position of "+SavedCharacters.Instance().DcurrentStats[i._position].characterName);
                Debug.Log(SavedCharacters.Instance().DcurrentStats[i._position].strength+" is the loaded strength of "+SavedCharacters.Instance().DcurrentStats[i._position].characterName);
            }
        }

        { // Loading the Equiment Dictionary
            var fileData = File.ReadAllText(Application.streamingAssetsPath + "/Json" + "/EquipmentDictionary.json");
            MasterEquipmentContainer deserializedData = JsonUtility.FromJson<MasterEquipmentContainer>(fileData);

            MasterEquipmentContainer.Instance.Equipment.Clear();
            MasterEquipmentContainer.Instance.Equipment.AddRange(deserializedData.Equipment);

            Debug.Log(MasterEquipmentContainer.Instance.Equipment[0].STRPercentBonus.ToString());
        }
    }
}

public class ActiveScene
{
    private static ActiveScene _instance { get; set; }

    public static ActiveScene Instance()
    {
        if (_instance == null)
        {
            _instance = new ActiveScene();
        }
        return _instance;
    }

    public string Scene;
}
