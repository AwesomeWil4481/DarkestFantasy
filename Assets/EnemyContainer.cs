using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContainer : MonoBehaviour
{
    public Entity thisEnemy;
    public BattleMenu battleMenu;

    public void SelectNewTarget()
    {
        var tar = battleMenu.AquireTarget(thisEnemy.gameObject, battleMenu.storedAction);
        battleMenu.actor._target = tar.GetComponent<Entity>();
    }
}
