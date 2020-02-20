using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTwoContainer : characterStats
{
    public static PositionTwoContainer instance = null;
    public Animator animator;
    [SerializeField]
    public int Defense;
    [SerializeField]
    int Hp;

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
        Defense = defense;
        Hp = HP;
        BattleManager.instance.RegisterCharacters(this);
        defense = 10;
        HP = 100;
    }
    private void Update()
    {
        if (HP <= 1)
        {
            print("silly saurid, ya be dead again");
            HP = 0;
            KOed = true;
            animator.SetBool("is dead", true);
        }
        if(BattleManager.instance.fightQueue.Peek() == this)
        {
            print("Hello World");
        }
    }
}
