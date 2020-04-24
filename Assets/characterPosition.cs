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

    public void CharacterPositionSave(Vector3 playerPos)
    {
        var varu = new VPos { playerPos = playerPos };
        var saveableData = JsonUtility.ToJson(varu);
        File.WriteAllText(Application.persistentDataPath + "/savedPosition.json", saveableData);
    }

    public Vector3 CharacterPositionLoad()
    {
        print(Application.persistentDataPath);
        if (!File.Exists(Application.persistentDataPath + "/savedPosition.json"))
        {
            print("File Created");

            var varu = new VPos {playerPos = new Vector3(4,0,0)};
            var saveableData = JsonUtility.ToJson(varu);
            File.WriteAllText(Application.persistentDataPath + "/savedPosition.json", saveableData);
        }
        var fileData = File.ReadAllText(Application.persistentDataPath + "/savedPosition.json");
        VPos deserializedData = JsonUtility.FromJson<VPos>(fileData);

        print(deserializedData.playerPos);
        VPos.savedPlayerPos = deserializedData.playerPos;
        var value = VPos.savedPlayerPos;
        print("Value = " + value);
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
