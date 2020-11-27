using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentBarScript : MonoBehaviour
{
    public int Count;
    public int Armor;
    public int HPBonus;
    public int STBonus;
    public int HPPercentBonus;
    public int STPercentBonus;
    [Space]
    public EquipmentType EquipmentType;
    [Space]
    public string Description;

    public List<string> canEquip = new List<string>();
}
