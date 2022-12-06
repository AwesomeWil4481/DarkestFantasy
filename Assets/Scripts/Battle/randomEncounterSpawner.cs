using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class randomEncounterSpawner : LocationManager
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

    public GameObject characterPosOne;
    public GameObject characterPosTwo;
    public GameObject characterPosThree;
    public GameObject characterPosFour;

    int numberOfEnemies;
    int _position;
    private void Awake()
    {

    }

    void Start()
    {
        var t =  Instantiate(characterTwo, characterPosTwo.transform.position, Quaternion.identity);
        t.transform.parent = GameObject.Find("Position "+ t.GetComponent<PositionTwoContainer>()._position).transform;

        numberOfEnemies = Random.Range(1, 1);
        currentLocation = locationOne;
        if (numberOfEnemies == 1 || numberOfEnemies == 2)
        {
            positionFourOne = OneOne.transform.position;

            objectToInstantiate = currentLocation[Random.Range(0, currentLocation.Length)];
            var thing = objectToInstantiate.GetComponent<EnemyStats>();
            thing._position = 1;
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
