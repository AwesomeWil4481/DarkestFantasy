using System;

public enum EquipmentType
{
    Head,
    Chest,
    Boots,
    Gloves,
    Hand
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
}
