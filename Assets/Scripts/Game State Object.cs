using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public class Quest
{
    public enum QuestState { inactive, active, objectiveComplete, complete}
    public ItemList rewards;
    public string Name;
    public string Description;

    public int ID; // Start with the number 1. (1-----)
}
public class WorldObject
{
    public bool interacted;
    public List<Item> reward;

    public int ID; // Start with the number 2. (2-----)
}
public class NPC
{
    public bool isActive;
    public bool isMerchant;
    public bool isHostile;
    public int[] availableQuests;
    
    public int ID; // Start with the number 3. (3-----)
}

public class questDictionary
{
    public static questDictionary instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static Dictionary<questDictionary, int> Entry = new Dictionary<questDictionary, int>();
}

public class WorldStateList
{
    private static WorldStateList _instance;
    public static WorldStateList Instance()
    {
        if (_instance == null)
        {
            _instance = new WorldStateList();
        }
        return _instance;
    }

    public List<WorldObject> Edits = new List<WorldObject>();
    public static List<WorldObject> Entry = new List<WorldObject>();
}
