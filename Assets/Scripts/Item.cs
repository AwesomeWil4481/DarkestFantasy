using System;

public enum ItemType
{
    Key,
    Consumable,
    EquipableItem
}

[Serializable]
public class Item 
{

    public ItemType ItemType;
    public string Description;
    public int ID;
    public int Count;
    public string Name;
}