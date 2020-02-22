using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : Entity
{
    GameObject[] potentialTargets;

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

    public Animator animator;

    float delay;
    float delayMax;

    bool _hasAttacked;

    public GameObject pointer;
    protected characterStats _targetStats;
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
    GameObject PositionTwo;
    public GameObject PositionThree;
    public GameObject PositionFour;
    [HideInInspector]
    public int action;
    void Start()
    {
        delayMax = 1f;
        delay = delayMax;
        BattleManager.instance.RegisterEnemies(this);
        PositionTwo = GameObject.FindGameObjectWithTag("position2");
        int level = Random.Range(1, 3);
    }

    IEnumerator Delay()
    {
        animator.SetTrigger("Attack");
        Target = PositionTwo.GetComponent<Entity>();
        yield return new WaitForSeconds(0);
        animator.SetTrigger("Attack");
        isTurn = true;

        if (!_hasAttacked)
        {
            print("blah");
            Attack();
            _hasAttacked = true;
        }
    }

    void Update()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
        if (BattleManager.instance.fightQueue.Peek() == this)
        {
            positionToAttack = Random.Range(2, 2);
            if (positionToAttack == 2 && !isTurn)
            {
                StartCoroutine(Delay());
            }
            delay -= Time.deltaTime;
            if (delay <= 0)
            {
                delay = delayMax;
                isTurn = false;
                BattleManager.instance.fightQueue.Enqueue(BattleManager.instance.fightQueue.Dequeue());
            }
        }
    }
}
