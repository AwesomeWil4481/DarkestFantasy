using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionTwoContainer : characterStats
{
    private static PositionTwoContainer instance;

    public static PositionTwoContainer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PositionTwoContainer();
            }
            return instance;
        }
    }


    RaycastHit2D hit;

    Vector2[] touches = new Vector2[5];

    public Animator animator;
    bool KOed;

    float _delay;
    float _delayMax = .2f;
    float _startDelay;

    bool _targetSelected;

    GameObject bar;

    GameObject _textObject;

    Text _posTwoText;

    public GameObject charTwoContainer;
    public PositionTwoContainer charTwo;

    public GameObject hitAll;
    void Awake()
    {
        BattleManager.instance.RegisterCharacters(this);
        _textObject = GameObject.FindGameObjectWithTag("postwotext");
        _posTwoText = _textObject.GetComponent<Text>();
        bar = GameObject.FindGameObjectWithTag("postwobar");
        bar.SetActive(false);
    }

    public void UpdateCharacter(PositionTwoContainer Prefab, string position)
    {
        charTwoContainer = gameObject;
        charTwo = charTwoContainer.GetComponent<PositionTwoContainer>();
        //GameObject PrefabToLoad = (GameObject)Resources.Load("Objects/Prefabs/Players/Battle/"+ "Position "+position);
        //var LoadedPrefab = PrefabToLoad.GetComponent<PositionTwoContainer>();
        var LoadedPrefab = charTwo;
        LoadedPrefab.HP = Prefab.HP;
        LoadedPrefab.MP = Prefab.MP;
        LoadedPrefab.defense = Prefab.defense;
        LoadedPrefab.level = Prefab.level;
        LoadedPrefab.battlePower = Prefab.battlePower;
        LoadedPrefab.strength = Prefab.strength;
        LoadedPrefab.speed = Prefab.speed;
    }

    private void Start()
    {
        charTwoContainer = GameObject.Find(gameObject.name);
        Name = "Terra";
        hitAll = GameObject.Find("Hit All Button");
        hitAll.SetActive(false);
        _delay = _delayMax;
        _targetSelected = false;
        _maxHP = 100;
        _minHp = _maxHP / 10 + 1;
        speed -= speed * 2;
        _startDelay = 1f;
    }

    private void Update()
    {
        _posTwoText.text = "HP     " + HP + " / " + _maxHP;
        if (HP <= 0)
        {
            print("You Died");
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
            _startDelay -= Time.deltaTime;
            if (_startDelay <= 0)
            {
                bar.SetActive(true);
            }
            if (!KOed)
            {
                if (_action == Action.attack)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        StartCoroutine(SceneTransition.instance.EndScene(ActiveScene.Instance().Scene));
                    }
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
                                _target = target.GetComponent<EnemyStats>();

                                _enemyStats.pointer.SetActive(true);
                                print("hello world");
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
                    }
                    foreach (Touch touch in Input.touches)
                    {
                        if (touch.phase == TouchPhase.Began)
                        {
                            touches[touch.fingerId] = Camera.main.ScreenToWorldPoint(Input.GetTouch(touch.fingerId).position);

                            hit = Physics2D.Raycast(touches[touch.fingerId], Vector2.zero);

                            if (hit.collider.gameObject.tag == "enemy")
                            {
                                print("enemy selected");
                                if (target != hit)
                                {
                                    target = hit.collider.gameObject;

                                    _enemyStats = target.GetComponent<EnemyStats>();
                                    _target = target.GetComponent<EnemyStats>();

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
        }
        else
        {
            bar.SetActive(false);
        }
    }

    public void AtttackButtonPressed()
    {
        charTwoContainer = BattleManager.instance.fightQueue.Peek().gameObject;
        charTwo = charTwoContainer.GetComponent<PositionTwoContainer>();
        charTwo._action = Action.attack;
        print("Attack Button Pressed");
    }

    public void waitButtonPressed()
    {
        charTwoContainer = GameObject.Find("Position "+BattleManager.instance.fightQueue.Peek()._position + "(Clone)");
        charTwo = charTwoContainer.GetComponent<PositionTwoContainer>();
        charTwo._action = Action.nothing;
        BattleManager.instance.fightQueue.Enqueue(BattleManager.instance.fightQueue.Dequeue());
        print("Wait Button Pressed");
    }
}
