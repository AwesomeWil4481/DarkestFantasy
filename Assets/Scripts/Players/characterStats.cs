﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterStats : Entity
{
    protected enum Action
    {
        nothing,
        attack,
        magic,
        item,
        defend
    }

    protected Action _action;

    protected GameObject target;

    protected int _minHp;
    protected int _maxHP;
    public int enemyHP;

    protected EnemyStats _enemyStats;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SelectTarget()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            target =hit.collider.gameObject;
            var targetStats = target.GetComponent<EnemyStats>();
            enemyHP = targetStats.HP;
            Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);
        }
    }
}
