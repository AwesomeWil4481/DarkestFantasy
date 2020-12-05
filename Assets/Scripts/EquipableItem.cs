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
    public int HPBonus;
    public int MPBonus;
    public int BTLPWRBonus;
    public int STRBonus;
    public int SPDBonus;
    public int DEFBonus;
    public int STMNABonus;
    public int MAGBonus;
    public int ATKBonus;
    public int EVSINBonus;
    public int MagEVSINBonus;
    public int MagDefBonus;
    public int HPPercentBonus;
    public int MPPercentBonus;
    public int BTLPWRPercentBonus;
    public int STRPercentBonus;
    public int SPDPercentBonus;
    public int DEFPercentBonus;
    public int STMNAPercentBonus;
    public int MAGPercentBonus;
    public int ATKPercentBonus;
    public int EVSINPercentBonus;
    public int MagEVSINPercentBonus;
    public int MagDefPercentBonus;
    public EquipmentType EquipmentType;

    public List<string> canEquip = new List<string>();
}
