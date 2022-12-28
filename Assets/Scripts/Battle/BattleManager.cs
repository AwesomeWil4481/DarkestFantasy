using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class BattleManager : MonoBehaviour
{
    public float actionDelay;

    public static BattleManager instance = null;

    public List<PositionTwoContainer> playerQueue = new List<PositionTwoContainer>();

    public GameObject DMGIndicator;
    int startListAdd {get;set;}

    public GameObject TargetObject;
    public GameObject battleMenu;
    public GameObject TopCanvas;
    public GameObject abilityMenu;

    public bool battleWait = false;

    public int enemiesLeft;
    int oldEnemies;

    public GameObject bar;

    public Queue<Action> actionQueue = new Queue<Action>();
    public List<Entity> entityList = new List<Entity>();

    bool AQactive = false;

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
        if (actionQueue.Count > 0 && !AQactive)
        {
            var t = actionQueue.Peek();
            AQactive = true;
            StartCoroutine( ExcecuteAction(t));
            print("activated action");
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

    public IEnumerator ExcecuteAction(Action action)
    {
        actionQueue.Peek().actor.animator.SetTrigger(actionQueue.Peek().actionName);

        yield return new WaitForSecondsRealtime(action.timer);
        action.actor.Invoke(action.actionName, 0f);
        action.actor.storedAction = false;
        actionQueue.Dequeue();

        StartCoroutine(QueueDelay());
    }

    public IEnumerator QueueDelay()
    {
        yield return new WaitForSecondsRealtime(actionDelay);
        AQactive = false;
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

    public void RegisterHit(int incomingDMG, Entity target)
    {
        var showDMG = Instantiate(instance.DMGIndicator);
        showDMG.transform.SetParent(instance.TopCanvas.transform, false);
        showDMG.transform.position = new Vector3(target.transform.position.x + 1f, target.transform.position.y - 0.5f, target.transform.position.z - 1);
        showDMG.GetComponentInChildren<Text>().text = incomingDMG.ToString();
    }
}

public class Action
{
    public string actionName;
    public string animName;
    public Entity actor;
    public float timer;
}