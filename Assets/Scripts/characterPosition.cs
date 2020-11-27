using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
public class characterPosition: MonoBehaviour
{
    public static characterPosition instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CharacterPositionSave(Vector3 playerPos, string SaveSelected)
    {
        var varu = new VPos { playerPos = playerPos };
        var saveableData = JsonUtility.ToJson(varu);
        File.WriteAllText(Application.persistentDataPath + "/Save "+SaveSelected+"/savedPosition.json", saveableData);
    }

    public static Vector3 CharacterPositionLoad()
    {

        if (!File.Exists(Application.persistentDataPath + "/savedPosition.json"))
        {
            print("File Created");

            var varu = new VPos {playerPos = new Vector3(4,0,0)};
            var saveableData = JsonUtility.ToJson(varu);
            File.WriteAllText(Application.persistentDataPath + "/savedPosition.json", saveableData);
        }
        var fileData = File.ReadAllText(Application.persistentDataPath + "/savedPosition.json");
        VPos deserializedData = JsonUtility.FromJson<VPos>(fileData);


        VPos.savedPlayerPos = deserializedData.playerPos;
        var value = VPos.savedPlayerPos;

        return value;
    }
}
[System.Serializable]
public class VPos
{
    public static VPos instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public Vector3 playerPos;
    public static Vector3 savedPlayerPos;
}
