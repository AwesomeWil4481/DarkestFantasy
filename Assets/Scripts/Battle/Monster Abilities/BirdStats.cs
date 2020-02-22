using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdStats : EnemyStats
{
    private void Start()
    {
        BattleManager.instance.RegisterEnemies(this);
        speedMin = speedMin -= speedMin;
        speedMax = speedMax -= speedMax;
    }
    private void Update()
    {
        if (isTurn == true)
        {
            Action();
        }
    }
    void Action()
    {
        action = (Random.Range(1, allAbilities));
        if (positionToAttack == 1)
        {
            attackedCharacter = PositionOne;
        }
        if (positionToAttack == 3)
        {
            attackedCharacter = PositionThree;
        }
        if (positionToAttack == 4)
        {
            attackedCharacter = PositionFour;
        }

        print("its my turn" + gameObject.name);
        if (action == 1)
        {
            dmgDvivder = Random.Range(240, 270);
            dmgMultiplier = Random.Range(240, 270);
            var dmgCalc = (attack * (dmgMultiplier - PositionTwoContainer.instance.defense) / dmgDvivder) + 1;
            PositionTwoContainer.instance.HP -= dmgCalc;
            print(gameObject.name + " Hit for " + dmgCalc);
        }
        if (action == 2)
        {

        }
    }
}
