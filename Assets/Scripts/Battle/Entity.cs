using UnityEngine;

public class Entity : MonoBehaviour
{
    public string enemyName;
    public int level;
    public int HP;
    public int MP;
    public int defense;
    public int speed;
    public int strength;
    public int battlePower;
}

public abstract class Abilities : Entity 
{
    public Entity _target;
    public abstract void Attack();
}
