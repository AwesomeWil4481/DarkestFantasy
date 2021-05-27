using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackroundManager : MonoBehaviour
{
    void Start()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Floor Layer"))
        {
            var thing = g.GetComponentsInChildren<MeshRenderer>();
            foreach(MeshRenderer s in thing)
            {
                s.sortingLayerName = "Backround"; 
            }
        }
    }

    void Update()
    {
        
    }
}
