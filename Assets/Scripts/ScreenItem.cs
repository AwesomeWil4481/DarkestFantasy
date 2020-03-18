using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenItem : MonoBehaviour
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
    public string _name;
    public int ID;
    public int count;
    public bool equipable;


    public GameObject countText;
    private void Start()
    {
        var text = countText.GetComponent<Text>();
        text.text = "" + count;
    }

    public void OnItemClick()
    {
        if (_itemType == itemType.Consumable)
        {

        }
    }
}