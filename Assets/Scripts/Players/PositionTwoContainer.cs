using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionTwoContainer : characterStats
{
    public static PositionTwoContainer instance = null;
    public Animator animator;
    bool KOed;

    float _delay;
    float _delayMax = .2f;

    bool _targetSelected;

    GameObject _textObject;
    Text _posTwoText;

    public GameObject charTwoContainer;
    public PositionTwoContainer charTwo;

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
    private void Start()
    {
        BattleManager.instance.RegisterCharacters(this);
        _textObject = GameObject.FindGameObjectWithTag("postwotext");
        _posTwoText = _textObject.GetComponent<Text>();
        _delay = _delayMax;
        _targetSelected = false;
        _maxHP = 100;
        _minHp = _maxHP / 10 + 1;
        speed -= speed * 2;
    }

    private void Update()
    {
        _posTwoText.text = "HP     "+ HP +" / " +_maxHP;
        if (HP <= 1)
        {
            print("silly saurid, ya be dead again");
            HP = 0;
            KOed = true;
            animator.SetBool("is dead", true);
        }

        if (HP <= _minHp + 1)
        {
            animator.SetBool("Wounded", true);
        }

        if (BattleManager.instance.fightQueue.Peek() == this)
        {
            if (_action == Action.attack)
            {
                if (_targetSelected == true)
                {
                    _delay -= Time.deltaTime;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                    if (hit.collider.gameObject.tag == "enemy")
                    {
                        print("enemy selected");
                        if (target != hit)
                        {
                            target = hit.collider.gameObject;

                            _enemyStats = target.GetComponent<EnemyStats>();
                            Target = target.GetComponent<EnemyStats>();

                            _enemyStats.pointer.SetActive(true);
                            print("Hello World");
                            _targetSelected = true;
                        }
                        if (target == hit && _delay <= 0)
                        {
                            Attack();
                            _enemyStats = target.GetComponent<EnemyStats>();
                            _enemyStats.pointer.SetActive(false);
                            _action = Action.nothing;
                            _targetSelected = false;
                            _delay = _delayMax;
                            target = null;
                            BattleManager.instance.fightQueue.Enqueue(BattleManager.instance.fightQueue.Dequeue());
                        }
                    }
                    else
                    {
                        print("Didn't hit touch an enemy");
                    }
                }
            }
        }
    }

    public void AtttackButtonPressed()
    {
        charTwoContainer = GameObject.FindGameObjectWithTag("position2");
        charTwo = charTwoContainer.GetComponent<PositionTwoContainer>();
        charTwo._action = Action.attack;
        print("Attack Button Pressed");
    }
}
