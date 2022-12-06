using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EnemyStats : Abilities
{
    GameObject[] potentialTargets;
    public List<Item> lootTable = new List<Item>();

    [HideInInspector]
    int moneyOnDeath;
    int expOnDeath;
    public int magicPower;

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

    public Text positionOneText;
    public Text positionTwoText;
    public Text positionThreeText;
    public Text positionFourText;
    public Text positionFiveText;
    public Text positionSixText;
    public Text positionSevenText;
    public GameObject thisTextObject;
    private void Awake()
    {
        BattleManager.instance.RegisterEnemies(this);
    }
    void Start()
    {
        print(_position);
        if (_position == 1)
        {
            thisTextObject = GameObject.Find("positionOneText");
            positionOneText = thisTextObject.GetComponent<Text>();
            positionOneText.text = "hi";

        }
        delayMax = 1f;
        delay = delayMax;
        speed -= speed * 2;
        PositionTwo = GameObject.Find("Position " + 1 + "(Clone)");
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
            BattleManager.instance.enemiesLeft -= 1;
            int random = Random.Range(1, 8);
            int change = lootTable.Count - 1;
            int loot =  Random.Range(0, change);
            if (random != 9)
            {
                SceneItemList.savedItems.Add(lootTable[loot]);
                print(lootTable[loot].Name);
            }
            Destroy(gameObject);
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
