using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLoad : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("First Load") == 0)
        {
            string s = "";
            JsonUtility.ToJson(s);
        }
    }
}
