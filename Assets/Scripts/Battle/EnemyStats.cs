using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : Entity
{
    int strength;
    [HideInInspector]
    int moneyOnDeath;
    int expOnDeath;
    int evasion;
    public int magicPower;
    int magicEvasion;
    int magicDefense;

    public bool isTurn;

    public int lvlMin;
    public int lvlMax;

    public int positionToAttack;

    float waitTime = 0.5f;

    public int speedMax = 5;
    public int speedMin = 1;
    public int allAbilities = 2;
    [HideInInspector]
    public int dmgMultiplier;
    [HideInInspector]
    public int dmgDvivder;
    [HideInInspector]
    public GameObject attackedCharacter;
    public GameObject PositionOne;
    public GameObject PositionTwo;
    public GameObject PositionThree;
    public GameObject PositionFour;
    [HideInInspector]
    public int action;
    RangeInt Range;
    void Start()
    {
        Range = new RangeInt(speedMin, speedMax);
        int level = Random.Range(1, 3);
    }

    public IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(waitTime);
    }

    void Update()
    {
        print(gameObject.name + speed);
    }
}
