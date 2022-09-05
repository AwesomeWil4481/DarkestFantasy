using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


public class WorldStateManager : MonoBehaviour
{
    public string Key;

    private static WorldStateManager _instance;
    public static WorldStateManager Instance()
    {
        if (_instance == null)
        {
            _instance = new WorldStateManager();
        }
        return _instance;
    }

    public GameObject savedObject;

    public List<string> SaveMark = new List<string>();
    public List<GameObject> markedForSaveObjects = new List<GameObject>();

    public void Awake()
    {
        if(GameObject.Find("Game Management").GetComponent<InitialLoad>().initialLoad == true)
        {
            LoadToPrefab();
        }
        
        var vv = savedObject.GetComponentsInChildren<WorldObjectState>();
        var va = gameObject.GetComponentsInChildren<WorldObjectState>();
        var num = 0;

        foreach (WorldObjectState i in vv)
        {
            foreach (WorldObjectState w in va)
            {
                if (i.ID == w.ID)
                {
                    va[num].interacted = i.interacted;

                    return;
                }
            }
        }
    }

    public void LoadToPrefab()
    {
        foreach (WorldObject worldObject in WorldStateList.Instance().Edits)
        {
            foreach(WorldObjectState worldObjectState in savedObject.GetComponentsInChildren<WorldObjectState>())
            {
                if (worldObject.ID == worldObjectState.ID)
                {
                    worldObjectState.interacted = worldObject.interacted;

                    return;
                }
            }
        }
        GameObject.Find("Game Management").GetComponent<InitialLoad>().initialLoad = false;
    }

    //public void WorldStateLoad(List<WorldObject> funnel)
    //{
    //    var num = 1;

    //    Key = gameObject.GetComponent<WorldStateManager>().Key;

    //    foreach (WorldObject w in funnel)
    //    {
    //        var g = GameObject.Find("2" + Key + num).GetComponent<WorldObjectState>();

    //        print(g.gameObject.name);
    //        g.interacted = w.interacted;
    //    }
    //}

    public void StateSave(string SaveSelected)
    {
        int num = 0;

        foreach (GameObject g in markedForSaveObjects)
        {
            var b = g.GetComponentsInChildren<WorldObjectState>().ToList();


            SaveWorldState.Instance().WorldStateSave(SaveSelected, b, _instance.SaveMark[num]);
        }
    }
}
[Serializable]
public class SaveWorldState
{
    private static SaveWorldState _instance { get; set; }

    [SerializeField]
    private List<WorldStateSerilization> pList = new List<WorldStateSerilization>();

    public List<WorldStateSerilization> list { get { return pList; } }

    public static SaveWorldState Instance()
    {
        if (_instance == null)
        {
            _instance = new SaveWorldState();
        }
        return _instance;
    }

    public void WorldStateSave(string SaveSelected, List<WorldObjectState> gameObjects, string Name)
    {
        foreach(WorldObjectState w in gameObjects)
        {
            list.Add(new WorldStateSerilization { ID = w.ID, interacted = w.interacted });
        }

        string character = JsonUtility.ToJson(Instance());
        Debug.Log("this is the world state "+character);

        File.WriteAllText(Application.persistentDataPath + "/Save " + SaveSelected + "/"+ Name + ".json", character);

        list.Clear();
    }
}
[Serializable]
public class WorldStateSerilization
{
    public int ID;

    [SerializeField]
    private int PID { get { return ID;}}

    public bool interacted;

    [SerializeField]
    private bool pInteracted { get { return interacted; } }
}
