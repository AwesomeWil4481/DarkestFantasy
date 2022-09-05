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
    public int stamina;
    public int attack;
    public int magic;
    public int evasion;
    public int magicEvasion;
    public int magicDefense;
    public EquipableItem BodySlot = new EquipableItem { Name = "Empty" };
    public EquipableItem HeadSlot = new EquipableItem { Name = "Empty" };
    public EquipableItem LeftRelicSlot = new EquipableItem { Name = "Empty" };
    public EquipableItem RightRelicSlot = new EquipableItem { Name = "Empty" };
    public EquipableItem LeftHandSlot = new EquipableItem { Name = "Empty" };
    public EquipableItem RightHandSlot = new EquipableItem { Name = "Empty" };
}

public abstract class Abilities : Entity 
{
    public Entity _target;
    public abstract void Attack();
}
