using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationContainer : MonoBehaviour
{
    public static LocationContainer instance = null;

    public string _currentLocation;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
