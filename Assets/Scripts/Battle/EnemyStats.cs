using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyStats : Abilities
{
    GameObject[] potentialTargets;

    [HideInInspector]
    int moneyOnDeath;
    int expOnDeath;
    int evasion;
    public int magicPower;
    int magicEvasion;
    int magicDefense;

    bool isTurn;

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

    private void Awake()
    {
        BattleManager.instance.RegisterEnemies(this);
    }
    void Start()
    {
        delayMax = 1f;
        delay = delayMax;
        speed -= speed * 2;
        PositionTwo = GameObject.FindGameObjectWithTag("position2");
        level = Random.Range(3, 6);
        strength = Random.Range(56, 63);
    }

    IEnumerator Delay()
    {
        animator.SetTrigger("Attack");
        _target = PositionTwo.GetComponent<Entity>();
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
                _hasAttacked = false;
                delay = delayMax;
                isTurn = false;
                BattleManager.instance.fightQueue.Enqueue(BattleManager.instance.fightQueue.Dequeue());
            }
        }
    }

    public override void Attack()
    {
        var damage = level * level * (battlePower * 4 + strength) / 256;
        damage = (damage * Random.Range(224, 255) / 256) + 1;

        print($"enemy damage: {damage}");

        _target.HP -= damage;
    }
}
