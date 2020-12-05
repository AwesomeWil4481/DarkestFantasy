using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentBarScript : MonoBehaviour
{
    public int Count;
    [Space]
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
   [Space]
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
    [Space]
    public EquipmentType EquipmentType;
    [Space]
    public string Description;

    public List<string> canEquip = new List<string>();
}
