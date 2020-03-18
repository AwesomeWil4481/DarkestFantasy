using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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

    public string description;
    public string name;
    public int ID;
    public int count;
    public bool equipable;
}