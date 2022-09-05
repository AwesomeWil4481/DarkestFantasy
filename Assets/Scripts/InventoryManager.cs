using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    int number;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        LoadGame();
    }

    public void LoadGame()
    {
        //var varu = new ItemList() { items = ItemList.SavedItems };
        //string character = JsonUtility.ToJson(varu);
        //File.WriteAllText(Application.persistentDataPath + "/Inventory.json", character);
    }

    public void SaveGame(string SaveSelected)
    {
        string character = JsonUtility.ToJson(ItemList.Instance());
        File.WriteAllText(Application.persistentDataPath + "/Save "+SaveSelected+"/Inventory.json", character);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            LoadGame();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ItemList.Instance().Items.Add(new Item { Count = 1, Description = "", Name = "Potion", ItemType = ItemType.Consumable });
        }
    }
}

[Serializable]
public class ItemList
{
    private ItemList()
    {
    }

    private static ItemList _instance { get; set; }

    public static ItemList Instance()
    {
        if (_instance == null)
        {
            _instance = new ItemList();
        }
        return _instance;
    }

    [SerializeField]
    private List<Item> items = new List<Item>();
    public List<Item> Items { get { return items; } }

    [SerializeField]
    public int GP;
}

public class EquipmentList
{
    public static EquipmentList instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public List<EquipableItem> equipableItems = new List<EquipableItem>();
}

