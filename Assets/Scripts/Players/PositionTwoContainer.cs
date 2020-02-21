using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTwoContainer : characterStats
{
    public static PositionTwoContainer instance = null;
    public Animator animator;
    bool KOed;

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
        speed -= speed * 2;
    }

    EnemyStats _enemyStats;

    private void Update()
    {
        if (HP <= 1)
        {
            print("silly saurid, ya be dead again");
            HP = 0;
            KOed = true;
            animator.SetBool("is dead", true);
        }

        if (BattleManager.instance.fightQueue.Peek() == this)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit.collider.gameObject.tag == "enemy")
                {
                    target = hit.collider.gameObject;

                    _enemyStats = target.GetComponent<EnemyStats>();

                    _enemyStats.pointer.SetActive(true);
                    print("Hello World");
                }
            }

            if (!_enemyStats != null)
            {
                // do something here
            }

            BattleManager.instance.fightQueue.Enqueue(BattleManager.instance.fightQueue.Dequeue());
        }
    }
}
