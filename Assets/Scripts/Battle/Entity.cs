using UnityEngine;

public class Entity : MonoBehaviour
{
    public string Name;
    public int level;
    public int HP;
    public int MP;
    public int defense;
    public int speed;
    public int strength;
    public int battlePower;
    public int _position;
}

public abstract class Abilities : Entity 
{
    public Entity _target;
    public abstract void Attack();
}
