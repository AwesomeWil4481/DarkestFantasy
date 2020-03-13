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

    float startDelay;

    public int enemiesLeft;
    int oldEnemies;
    int old = 0;
    int numberOfCharacters;

    public GameObject bar;

    bool neverDone;
    List<Entity> entityList;
    void Start()
    {
        startDelay = 1;
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
        startDelay -= Time.deltaTime;
        if (oldEnemies != enemiesLeft)
        {
            oldEnemies = enemiesLeft;
            print(enemiesLeft);
        }
        if (oldEnemies == 0)
        {
            print("you win!");
            StartCoroutine(SceneTransition.instance.EndScene("SampleScene"));
        }
        if (neverDone == true)//&& startDelay <= 0)
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
        enemy.level = Random.Range(enemy.lvlMin, enemy.lvlMax);
        enemiesLeft += 1;
        oldEnemies += 1;
    }
    public void RegisterCharacters(characterStats Char)
    {
        entityList.Add(Char);
        numberOfCharacters += 1;
    }
}
