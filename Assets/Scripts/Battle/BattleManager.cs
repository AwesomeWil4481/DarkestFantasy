using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance = null;

    public Queue<Entity> fightQueue;

    int startListAdd = 0;

    public GameObject TargetObject;

    bool neverDone;
    List<Entity> entityList;
    void Start()
    {
        neverDone = true;
        entityList = new List<Entity>();
        fightQueue = new Queue<Entity>();
    }
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
    void Update()
    {

        if (neverDone == true)
        {
            entityList = entityList
           .OrderBy(w => w.speed)
           .ToList();
            foreach (Entity i in entityList)
            {
                fightQueue.Enqueue(entityList[startListAdd]);
                startListAdd += 1;
            }
            neverDone = false;
        }
        if (fightQueue.Peek() == null)
        {
            fightQueue.Dequeue();
        }
    }
    public void RegisterEnemies(EnemyStats enemy)
    {
        entityList.Add(enemy);
        enemy.speedMin -= (enemy.speedMin * 2);
        enemy.speedMax -= enemy.speedMax * 2;
        enemy.speed = Random.Range(enemy.speedMin, enemy.speedMax);
        enemy.level = Random.Range(enemy.lvlMin, enemy.lvlMax);

    }
    public void RegisterCharacters(characterStats Char)
    {
        entityList.Add(Char);

    }
}
