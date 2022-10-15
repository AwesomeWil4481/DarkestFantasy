using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance = null;

    public Queue<Entity> fightQueue;

    int startListAdd {get;set;}

    public GameObject TargetObject;

    public int enemiesLeft;
    int oldEnemies;

    public GameObject bar;

    bool neverDone;

    public List<Entity> entityList = new List<Entity>();
    public List<characterStats> activeCharacterList;

    void Start()
    {
        startListAdd = 0;
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
        if (oldEnemies != enemiesLeft)
        {
            oldEnemies = enemiesLeft;
            print(enemiesLeft);
        }
        if (oldEnemies == 0)
        {
            print("you win!");
            ActiveScene.Instance().Scene = GameObject.Find("Game Management").GetComponent<LoadCharacterStats>().TargetScene;
            StartCoroutine(SceneTransition.instance.EndScene(ActiveScene.Instance().Scene));
            CharacterStatisticsSerializer.Instance.SaveToPrefab();
        }
        if (neverDone == true)//&& startDelay <= 0)
        {
            entityList = entityList
           .OrderBy(w => -w.speed)
           .ToList();


            foreach (Entity i in entityList)
            {
                print(i.Name + "'s speed is " + i.speed);
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
    }
}
