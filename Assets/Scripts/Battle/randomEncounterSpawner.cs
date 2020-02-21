﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomEncounterSpawner : MonoBehaviour
{
    public GameObject[] locationOne;
    public GameObject[] currentLocation;

    public GameObject FourOne;
    public GameObject FourTwo;
    public GameObject FourThree;
    public GameObject FourFour;

    public GameObject OneOne;

    GameObject objectToInstantiate;

    public GameObject characterOne;
    public GameObject characterTwo;
    public GameObject characterThree;
    public GameObject characterFour;

    Vector3 positionFourOne;
    Vector3 positionFourTwo;
    Vector3 positionFourThree;
    Vector3 positionFourFour;

    Vector3 characterPosOne;
    Vector3 characterPosTwo = new Vector3(7.75f,-0.25f,0);
    Vector3 characterPosThree;
    Vector3 characterPosFour;

    int numberOfEnemies;
    void Start()
    {
        Instantiate(characterTwo, characterPosTwo, Quaternion.identity);
        numberOfEnemies = Random.Range(1, 4);
        currentLocation = locationOne;

        if (numberOfEnemies == 1 || numberOfEnemies == 2)
        {
            positionFourOne = OneOne.transform.position;

            objectToInstantiate = currentLocation[Random.Range(0, currentLocation.Length)];
            Instantiate(objectToInstantiate, positionFourOne, Quaternion.identity);
        }

        
        if (numberOfEnemies == 3 || numberOfEnemies == 4)
        {
            positionFourOne = FourOne.transform.position;
            positionFourTwo = FourTwo.transform.position;
            positionFourThree = FourThree.transform.position;
            positionFourFour = FourFour.transform.position;

            objectToInstantiate = currentLocation[Random.Range(0, currentLocation.Length)];
            Instantiate(objectToInstantiate, positionFourOne, Quaternion.identity);

            objectToInstantiate = currentLocation[Random.Range(0, currentLocation.Length)];
            Instantiate(objectToInstantiate, positionFourTwo, Quaternion.identity);

            objectToInstantiate = currentLocation[Random.Range(0, currentLocation.Length)];
            Instantiate(objectToInstantiate, positionFourThree, Quaternion.identity);

            objectToInstantiate = currentLocation[Random.Range(0, currentLocation.Length)];
            Instantiate(objectToInstantiate, positionFourFour, Quaternion.identity);
        }
    }

    void Update()
    {
        
    }
}
