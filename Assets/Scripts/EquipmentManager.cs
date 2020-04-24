using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EquipmentManager : MonoBehaviour
{
    public GameObject positionOneButton;
    public GameObject positionTwoButton;
    public GameObject positionThreeButton;
    public GameObject positionFourButton;

    void Start()
    {
        
    }

    //public void NamingCharacterButtons()
    //{
    //    if (!File.Exists(Application.persistentDataPath+ "/PartyStats.json"))
    //    {
    //        //var varu = new SavedCharacters { currentStats =  };
    //        var saveableData = JsonUtility.ToJson(varu);
    //        File.WriteAllText(Application.persistentDataPath + "/PartyStats.json", saveableData);
    //    }
    //    positionOneButton.name = "";
    //}
}
