using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class characterStats : Abilities
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
    public override void Attack()
    {
        var strengthCalc = strength >= 128 ? 255 : strength * 2;
        var attackPower = battlePower + strengthCalc;

        var damage = battlePower + ((level * level * attackPower) / 256) * 3 / 2;
        damage = (damage * Random.Range(224, 255) / 256) + 1;

        print($"character damage: {damage}");

        _target.HP -= damage;
        BattleManager.instance.RegisterHit(damage, _target);
        _target = null;
    }
}