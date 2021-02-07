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
    [Space]
    public EquipmentType EquipmentType;
    [Space]
    public string Description;

    public List<string> canEquip = new List<string>();
}
