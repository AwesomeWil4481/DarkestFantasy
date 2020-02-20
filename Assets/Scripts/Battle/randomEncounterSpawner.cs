using System.Collections;
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

    GameObject objectToInstantiate;

    public GameObject characterOne;
    public GameObject characterTwo;
    public GameObject characterThree;
    public GameObject characterFour;

    Vector3 positionOne;
    Vector3 positionTwo;
    Vector3 positionThree;
    Vector3 positionFour;

    Vector3 characterPosOne;
    Vector3 characterPosTwo = new Vector3(7.75f,-0.25f,0);
    Vector3 characterPosThree;
    Vector3 characterPosFour;

    int numberOfEnemies;
    void Start()
    {
        Instantiate(characterTwo, characterPosTwo, Quaternion.identity);
        numberOfEnemies = Random.Range(3, 4);
        if (numberOfEnemies == 3 || numberOfEnemies == 4)
        {
            positionOne = FourOne.transform.position;
            positionTwo = FourTwo.transform.position;
            positionThree = FourThree.transform.position;
            positionFour = FourFour.transform.position;
        }

        currentLocation = locationOne;
        if (numberOfEnemies == 3 || numberOfEnemies == 4)
        {
            objectToInstantiate = currentLocation[Random.Range(0, currentLocation.Length)];
            Instantiate(objectToInstantiate, positionOne, Quaternion.identity);

            objectToInstantiate = currentLocation[Random.Range(0, currentLocation.Length)];
            Instantiate(objectToInstantiate, positionTwo, Quaternion.identity);

            objectToInstantiate = currentLocation[Random.Range(0, currentLocation.Length)];
            Instantiate(objectToInstantiate, positionThree, Quaternion.identity);

            objectToInstantiate = currentLocation[Random.Range(0, currentLocation.Length)];
            Instantiate(objectToInstantiate, positionFour, Quaternion.identity);
        }
    }

    void Update()
    {
        
    }
}
