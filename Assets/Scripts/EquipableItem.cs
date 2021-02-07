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
    public double HPPercentBonus;
    public double MPPercentBonus;
    public double BTLPWRPercentBonus;
    public double STRPercentBonus;
    public double SPDPercentBonus;
    public double DEFPercentBonus;
    public double STMNAPercentBonus;
    public double MAGPercentBonus;
    public double ATKPercentBonus;
    public double EVSINPercentBonus;
    public double MagEVSINPercentBonus;
    public double MagDefPercentBonus;
    public EquipmentType EquipmentType;

    public List<string> canEquip = new List<string>();
}
