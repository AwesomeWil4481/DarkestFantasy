using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public string enemyName;
    public int level;
    public int HP;
    public int MP;
    public int defense;
    public int speed;
    public int strength;
    public int battlePower;

    public Entity Target;

    public bool isCharacter;

    private void Start()
    {        
    }

    public virtual void Attack()
    {
    }
}
