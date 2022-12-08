using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionTwoContainer : characterStats
{
    RaycastHit2D hit;

    Vector2[] touches = new Vector2[5];

    bool KOed;

    float _delay;
    float _delayMax = .2f;

    GameObject _textObject;

    Text _posTwoText;

    public GameObject charTwoContainer;
    public PositionTwoContainer charTwo;

    public GameObject hitAll;
    void Awake()
    {
        BattleManager.instance.RegisterCharacters(this);
        GameObject.Find("Select " + _position).GetComponent<CharacterContainer>().thisCharacter = this;
    }

    public void UpdateCharacter(PositionTwoContainer Prefab, string position)
    {
        charTwoContainer = gameObject;
        charTwo = charTwoContainer.GetComponent<PositionTwoContainer>();
        //GameObject PrefabToLoad = (GameObject)Resources.Load("Objects/Prefabs/Players/Battle/"+ "Position "+position);
        //var LoadedPrefab = PrefabToLoad.GetComponent<PositionTwoContainer>();
        var LoadedPrefab = charTwo;
        LoadedPrefab.HP = Prefab.HP;
        LoadedPrefab.MP = Prefab.MP;
        LoadedPrefab.defense = Prefab.defense;
        LoadedPrefab.level = Prefab.level;
        LoadedPrefab.battlePower = Prefab.battlePower;
        LoadedPrefab.strength = Prefab.strength;
        LoadedPrefab.speed = Prefab.speed;
    }

    private void Start()
    {
        charTwoContainer = GameObject.Find(gameObject.name);
        Name = "Terra";
        _delay = _delayMax;
        _maxHP = 100;
        _minHp = _maxHP / 10 + 1;
    }

    private void Update()
    {
        if (HP <= 0)
        {
            print("You Died");
            HP = 0;
            KOed = true;
            animator.SetBool("is dead", true);
        }

        if (HP <= _minHp + 1)
        {
            animator.SetBool("Wounded", true);
        }

        if (!BattleManager.instance.battleWait)
        {
            if (!Active && !storedAction)
            {
                timeProgress = (timeProgress + Time.deltaTime) + ((float) speed) / 5000;
            }

            if (timeProgress >= 10)
            {
                Active = true;
                BattleManager.instance.RegisterCharacters(this);
            }
        }
    }
}
