using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BattleMenu : MonoBehaviour
{
    public enum ActionState
    {
        start,
        attack,
        defend,
        item,
        inactive,
        searching
    }
    
    public ActionState state;

    public PositionTwoContainer actor;

    public Text Name;
    public Text HpNumber;
    public Text MpNumber;

    GameObject target;

    BattleMenu Instance;

    public GameObject attackBack;

    public GameObject inactiveBattleMenu;
    public GameObject activeBattleMenu;

    public GameObject[] EnemySelectBoxes;
    public GameObject[] Selections;

    Vector3 activePos;
    Vector3 inactivePos;

    GameObject selectedTarget;

    bool aquireTarget = false;

    public Action storedAction;

    private void Awake()
    {
        Instance = gameObject.GetComponent<BattleMenu>();
    }

    void Start()
    {
        

        activePos = activeBattleMenu.transform.position;
        inactivePos = inactiveBattleMenu.transform.position;

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        gameObject.transform.parent.transform.position = activePos;
        
    }

    private void OnDisable()
    {
        gameObject.transform.parent.transform.position = inactivePos;
    }

    void Update()
    {
        if (aquireTarget)
        {
            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    checkTouch(Input.GetTouch(0).position);
                }
            }
        }

        if (state == ActionState.searching)
        {
            BattleManager.instance.abilityMenu.SetActive(true);
            foreach (GameObject g in EnemySelectBoxes)
            {
                g.SetActive(false);
            }

            ActionState thing()
            {
                foreach (GameObject g in Selections)
                {
                    var c = g.GetComponent<CharacterContainer>().thisCharacter;
                    if (c != null)
                    {
                        if (c.Active)
                        {
                            actor = c;
                            return ActionState.start;
                        }
                    }
                }

                return ActionState.inactive;
            }

            state = thing();
        }

        if (state == ActionState.inactive)
        {
            actor = null;
            gameObject.SetActive(false);
        }
    }

    void checkTouch(Vector3 pos)
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(pos);
        Vector2 touchPos = new Vector2(wp.x, wp.y);
        Collider2D hit = Physics2D.OverlapPoint(touchPos);

        if (hit != null)
        {
            if (hit.tag == "enemy")
            {
                var currentTarget = AquireTarget(hit.gameObject, storedAction);

                if (currentTarget != null)
                {
                    actor._target = currentTarget.GetComponent<Entity>();
                    selectedTarget = null;
                }
            }
        }
    }

    public GameObject AquireTarget(GameObject t, Action a)
    {
        //if (t == selectedTarget)
        {
            print("Same Target");

            BattleManager.instance.actionQueue.Enqueue(a);
            actor.storedAction = true;
            actor.Active = false;
            actor.animator.SetTrigger("Ready-Attack");
            actor.timeProgress = 0;
            state = ActionState.searching;
            return t;
        }
        //else
        //{
        //    print("New Target");
        //    selectedTarget = t;
        //    return null;
        //}
    }

    public void RegisterActor(PositionTwoContainer _actor)
    {
        if (actor == null)
        {
            state = ActionState.start;

            gameObject.SetActive(true);

            actor = _actor;

            Name.text = actor.Name;
            HpNumber.text = actor.HP.ToString();
            MpNumber.text = actor.MP.ToString();
        }
    }

    public void AttackBack()
    {
        foreach (GameObject g in EnemySelectBoxes)
        {
            g.SetActive(false);
        }

        BattleManager.instance.abilityMenu.SetActive(true);
    }

    public void AttackButton()
    {
        int count = 0;

        foreach (Entity e in BattleManager.instance.entityList)
        {
            EnemySelectBoxes[count].SetActive(true);
            EnemySelectBoxes[count].GetComponentInChildren<Text>().text = e.Name;
            EnemySelectBoxes[count].GetComponent<EnemyContainer>().thisEnemy = e;
            BattleManager.instance.abilityMenu.SetActive(false);
            count++;
        }

        if (actor._target != null)
        {
            actor.Attack();
        }
        else
        {
            storedAction = new Action { actionName = "Attack", actor = actor, timer = 1f};
            Instance.state = ActionState.attack;
            Target();
        }
    }

    void Target()
    {
        aquireTarget = true;
    }
}
