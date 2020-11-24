using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SceneItemList : MonoBehaviour
{
    public static SceneItemList instance;

    public List<Item> ts = new List<Item>();
    public static List<Item> savedItems = new List<Item>();

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
    }
}
