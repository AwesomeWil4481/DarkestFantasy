using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

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

    public static void SaveGame(string SaveSelected)
    {
        InventoryManager.instance.SaveGame(SaveSelected);

        characterPosition.instance.CharacterPositionSave(MenuManager.Instance.savePointPos, SaveSelected);

        CharacterStatisticsSerializer.SaveGame(SaveSelected);

        string character = JsonUtility.ToJson(MasterEquipmentContainer.Instance);
        File.WriteAllText(Application.persistentDataPath + "/Save " + SaveSelected + "/EquipmentDictionary.json", character);

        character = JsonUtility.ToJson(ActiveScene.Instance());
        File.WriteAllText(Application.persistentDataPath + "/Save " + SaveSelected + "/SavedScene.json", character);
    }

    public static void LoadGame(string SaveSelected)
    {
        { // Loading the scene
            var fileData = File.ReadAllText(Application.persistentDataPath + "/Save " + SaveSelected + "/SavedScene.json");
            ActiveScene deserializedData = JsonUtility.FromJson<ActiveScene>(fileData);
            ActiveScene.Instance().Scene = deserializedData.Scene;
            SceneManager.LoadScene(deserializedData.Scene);
        }

        { // Loading the Items
            var fileData = File.ReadAllText(Application.persistentDataPath + "/Save " + SaveSelected + "/Inventory.json");
            ItemList deserializedData = JsonUtility.FromJson<ItemList>(fileData);

            ItemList.Instance().Items.Clear();
            ItemList.Instance().Items.AddRange(deserializedData.Items);

            Debug.Log(ItemList.Instance().Items[1].Name+" of "+ItemList.Instance().Items[1].Count);
        }

        { // Loading the start position
            characterPosition.CharacterPositionLoad();
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
            }
        }

        { // Loading the Equiment Dictionary
            var fileData = File.ReadAllText(Application.persistentDataPath + "/EquipmentDictionary.json");
            MasterEquipmentContainer deserializedData = JsonUtility.FromJson<MasterEquipmentContainer>(fileData);

            MasterEquipmentContainer.Instance.Equipment.Clear();
            MasterEquipmentContainer.Instance.Equipment.AddRange(deserializedData.Equipment);
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

    public string Scene = SceneManager.GetActiveScene().name;
}
