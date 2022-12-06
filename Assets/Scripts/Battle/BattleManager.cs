using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance = null;

    public Queue<Entity> ActionQueue;
    public List<PositionTwoContainer> playerQueue = new List<PositionTwoContainer>();

    int startListAdd {get;set;}

    public GameObject TargetObject;
    public GameObject battleMenu;

    public bool battleWait = false;

    public int enemiesLeft;
    int oldEnemies;

    public GameObject bar;


    public List<Entity> entityList = new List<Entity>();

    void Start()
    {
        startListAdd = 0;
        entityList = new List<Entity>();
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
        if (playerQueue.Count == 0)
        {

        }
        else
        {
            
        }
        
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
    }
    public void RegisterEnemies(EnemyStats enemy)
    {
        entityList.Add(enemy);
        enemy.level = Random.Range(enemy.lvlMin, enemy.lvlMax);
        enemiesLeft += 1;
        oldEnemies += 1;
    }
    public void RegisterCharacters(PositionTwoContainer Char)
    {
        var charPos = Char._position;
    }
}
