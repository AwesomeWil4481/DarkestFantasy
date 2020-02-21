using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterStats : Entity
{
    public bool isTurn;
    public GameObject target;

    public int enemyHP;

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
