using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int level;
    public int HP;
    public int MP;
    public int attack;
    public int defense;
    public int speed;
    public Entity Target;


    public GameObject[] pointers;

    private void Start()
    {
        pointers = GameObject.FindGameObjectsWithTag("pointer");
    }

    public void Attack()
    {
        Debug.Log(this + "attacking");
        Target.HP -= 10;
    }
}
