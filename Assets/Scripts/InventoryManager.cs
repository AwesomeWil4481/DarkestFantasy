using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class InventoryManager : MonoBehaviour
{

    void Start()
    {
        LoadGame();
    }

    public void LoadGame()
    {
        var fileData = File.ReadAllText(Application.persistentDataPath + "/Inventory.json");
        ItemList deserializedData = JsonUtility.FromJson<ItemList>(fileData);

        ItemList.SavedItems = deserializedData.items;
        ItemList.instance.items = deserializedData.items;

        print(ItemList.SavedItems[0]._itemType);

        //var varu = new ItemList() { items = ItemList.SavedItems };
        //string character = JsonUtility.ToJson(varu);
        //File.WriteAllText(Application.persistentDataPath + "/Inventory.json", character);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            LoadGame();
        }
    }
}
[Serializable]
public class ItemList
{
    public static ItemList instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public List<Item> items = new List<Item>();
    public static List<Item> SavedItems = new List<Item>();
}
[Serializable]
public class Item
{
    public enum itemType
    {
        Key,
        Weapon,
        Consumable,
        Armor,
        Relic
    }
    public itemType _itemType;

    public string name;
    public int ID;
    public int count;
    public bool equipable;
}