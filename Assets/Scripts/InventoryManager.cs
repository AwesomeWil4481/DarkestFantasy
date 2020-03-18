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
        print(ItemList.SavedItems[0]._itemType);

        //var varu = new ItemList() { items = ItemList.SavedItems };
        //string character = JsonUtility.ToJson(varu);
        //File.WriteAllText(Application.persistentDataPath + "/Inventory.json", character);
    }

    public void SaveGame()
    {
        ItemList.SavedItems = SceneItemList.savedItems;

        var varu = new ItemList() { items = ItemList.SavedItems };
        string character = JsonUtility.ToJson(varu);
        File.WriteAllText(Application.persistentDataPath + "/Inventory.json", character);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            LoadGame();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ItemList.SavedItems.Add(new Item {count = 1, description = "",name = "Potion", _itemType = Item.itemType.Consumable });
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
