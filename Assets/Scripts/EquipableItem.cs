using System;
using System.Collections.Generic;

public enum EquipmentType
{
    Head,//00
    Chest,//01
    Boots,//02
    Gloves,//03
    Hand,//04
    Relic,//05
    Legs//06
}

[Serializable]
public class EquipableItem : Item
{
    public int Armor;
    public int HPBonus;
    public int STBonus;
    public int HPPercentBonus;
    public int STPercentBonus;
    public EquipmentType EquipmentType;

    public List<string> canEquip = new List<string>();
}
