using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public Vector3 savePointPos;

    public GameObject statBlocks;
    public GameObject[] _statBlocks;
    [Space]
    public GameObject menuPanel;
    public GameObject mainMenuButton;
    public GameObject mainMenuScreen;
    public GameObject saveButton;
    public GameObject equipButton;
    public GameObject exitConfirm;
    [Space]
    public GameObject inventoryScreen;
    public GameObject descriptionBar;
    [Space]
    public GameObject equipScreen;
    public GameObject equipmentSelectionScreen;
    public GameObject[] equipButtons;
    public Sprite[] characterPortraits;
    public GameObject equipmentDescriptionBar;
    public GameObject SelectedEquipment;
    public GameObject BodyUnequip;
    public GameObject HeadUnequip;
    public GameObject Relic1Unequip;
    public GameObject Relic2Unequip;
    public GameObject LeftHandUnequip;
    public GameObject RightHandUnequip;
    [Space]
    public GameObject selectionScreen;
    [Space]
    public GameObject SaveSelectionScreen;

    int posX = 320;
    int posY = 915;
    int rotationNumber = 1;

    bool savedThisCycle;
    bool wantToAdd;
    [Space]
    public GameObject EquipmentBar;
    public GameObject ItemBar;
    public GameObject Joystick;
    public bool canSave = true;
    public bool cantSave;
    [Space]
    public GameObject selectedItem;
    public Text[] TextArray;
    public GameObject[] itemBars;
    public List<string> itemNames = new List<string>();
    public List<GameObject> equipableItems;

    private PositionTwoContainer selectedCharToEquip { get; set; }

    bool LeftHandSelected { get; set; }
    bool RightHandSelected { get; set; }
    bool LeftRelicSelected { get; set; }
    bool RightRelicSelected { get; set; }

    int selectedPos;
    void Start()
    {
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Destroy(gameObject); };
    }

    // Entering the main menu
    public void OnMenuPress()
    {
        savedThisCycle = false;
        mainMenuButton.SetActive(false);
        mainMenuScreen.SetActive(true);
        menuPanel.SetActive(true);
        Joystick = GameObject.FindGameObjectWithTag("Joystick");
        Joystick.SetActive(false);

        if (canSave)
        {
            var _button = saveButton.GetComponent<Button>();

            _button.interactable = true;
        }

        if (!canSave)
        {
            var _button = saveButton.GetComponent<Button>();

            _button.interactable = false;
        }

        CharacterStatisticsSerializer.Instance.LoadCharacter();
    }

    // Exiting the main menu
    public void BackToScene()
    {
        menuPanel.SetActive(false);
        mainMenuButton.SetActive(true);
        mainMenuScreen.SetActive(false);
        Joystick.SetActive(true);

        savedThisCycle = false;
    }

    // Exiting to the title screen.
    public void ReturnToMainMenu()
    {
        if (savedThisCycle)
        {
            StartCoroutine(SceneTransition.instance.EndScene("MainMenu"));
        }
        else
        {
            exitConfirm.SetActive(true);
        }
    }

    // Open the confirm exit menu
    public void ConfirmReturnToMainMenu()
    {
        StartCoroutine(SceneTransition.instance.EndScene("MainMenu"));
    }

    // Closing the confirm exit menu
    public void CancelReturnToMainMenu()
    {
        exitConfirm.SetActive(false);
    }

    // Entering the item screen
    public void ItemScreenEnter()
    {
        print("Number of Items: " + ItemList.Instance().Items.Count);
        if (ItemList.Instance().Items.Any())
        {
            Debug.Log(ItemList.Instance().Items[0].Name);
        }
        else
        {
            Debug.LogWarning("no items found");
        }

        inventoryScreen.SetActive(true);
        mainMenuScreen.SetActive(false);
        foreach (Item i in ItemList.Instance().Items)
        {
            //Canvas cheat sheet x - 960, y - 540.
            GameObject go = Instantiate(ItemBar, new Vector3(posX, posY, 0), Quaternion.identity) as GameObject;
            go.transform.parent = GameObject.Find("Item Screen").transform;
            TextArray = go.GetComponentsInChildren<Text>();

            statBlocks.SetActive(false);

            TextArray[0].text = i.Name;
            TextArray[1].GetComponent<Text>().text = " s"+ i.Count.ToString();
            print(TextArray[1].name);
            go.gameObject.name = i.Name;
            go.GetComponent<ScreenItem>().count = i.Count;
            print(i.Name);
            print("There are this many " + i.Name + ": " + i.Count);

            var ahhh = go.GetComponent<ScreenItem>();

            ahhh.description = i.Description;

            if (i.ItemType == ItemType.Consumable)
            {
                ahhh._itemType = ScreenItem.itemType.Consumable;
            }

            wantToAdd = true;
            if (itemNames != null)
            {
                foreach (string w in itemNames)
                {
                    if (w == i.Name)
                    {
                        var currentObject = GameObject.Find(i.Name);
                        if (currentObject == null)
                        {
                            Debug.LogError($"failed to find GameObject {i.Name}");
                            continue;
                        }

                        var otherThing = currentObject.GetComponent<ScreenItem>();

                        otherThing.count += 1;
                        Destroy(go);
                        wantToAdd = false;
                        break;
                    }
                    else
                    {

                    }
                }
            }

            if (wantToAdd)
            {
                itemNames.Add(i.Name);
            }

            if (rotationNumber % 3 == 0)
            {
                posY += -110;
                posX = 320;
            }

            else
            {
                posX += 640;
            }
            rotationNumber += 1;
        }

        itemNames.Clear();
        posX = 320;
        posY = 915;
        rotationNumber = 1;
    }

    // When you click on an item in the menu
    public void ItemSelection(GameObject _gameObject)
    {
        descriptionBar = GameObject.Find("Description Bar");
        selectedItem = _gameObject;
        var selection = selectedItem.GetComponent<ScreenItem>();
        var textObject = descriptionBar.GetComponentInChildren<Text>();
        print(selection.description);
        print(selectedItem.name);
        print(descriptionBar.name);
        if (selection.description != null)
        {
            textObject.text = selection.description;
        }
    }

    //Exiting the item screen
    public void ItemScreenExit()
    {
        statBlocks.SetActive(true);
        itemBars = GameObject.FindGameObjectsWithTag("item bar");
        foreach (GameObject i in itemBars)
        {
            Destroy(i);
        }
        print(ItemList.Instance().Items.Count);
        mainMenuScreen.SetActive(true);
        inventoryScreen.SetActive(false);
    }

    EquipableItem DerriveEquipmentType(EquipmentType equipment)
    {
        var go = Instance.selectedCharToEquip;

        switch (equipment)
        {
            case EquipmentType.Chest:
                return (go.BodySlot);

            case EquipmentType.Head:
                return (go.HeadSlot);

            case EquipmentType.Relic:
                if (Instance.LeftRelicSelected == true)
                {
                    return (go.LeftRelicSlot);
                }
                else if (Instance.RightRelicSelected == true)
                {
                    return (go.RightRelicSlot);
                }
                break;

            case EquipmentType.Hand:
                if (Instance.RightHandSelected == true)
                {
                    return (go.RightHandSlot);
                }
                else if (Instance.LeftHandSelected == true)
                {
                    return (go.LeftHandSlot);
                }
                break;
        }

        return null;
    }

    void equipItem(EquipableItem item, GameObject button)
    {
        var go = Instance.selectedCharToEquip;

        switch (item.EquipmentType)
        {
            case EquipmentType.Chest:
                go.BodySlot = item;
                GameObject.Find("Body").GetComponentInChildren<Text>().text = item.Name;
                break;

            case EquipmentType.Head:
                go.HeadSlot = item;
                GameObject.Find("Helmet").GetComponentInChildren<Text>().text = item.Name;
                break;

            case EquipmentType.Relic:
                if (Instance.LeftRelicSelected == true)
                {
                    go.LeftRelicSlot = item;
                    GameObject.Find("Left Relic").GetComponentInChildren<Text>().text = item.Name;
                }
                else if (Instance.RightRelicSelected == true)
                {
                    go.RightRelicSlot = item;
                    GameObject.Find("Right Relic").GetComponentInChildren<Text>().text = item.Name;
                }
                break;

            case EquipmentType.Hand:
                if (Instance.LeftHandSelected == true)
                {
                    go.LeftHandSlot = item;
                    GameObject.Find("Left Hand").GetComponentInChildren<Text>().text = item.Name;

                    return;
                }
                else if (Instance.RightHandSelected == true)
                {
                    go.RightHandSlot = item;
                    GameObject.Find("Right Hand").GetComponentInChildren<Text>().text = item.Name;
                }
                break;
        }
    }

    // When you click on a piece of equipment while in the equipment screen
    public void EquipmentClick(GameObject thisButton)
    {
        GameObject.Find("Equipment Description Bar").GetComponentsInChildren<Text>()[0].text = thisButton.GetComponent<EquipmentBarScript>().Description;

        var selectedCharacter = GameObject.Find("Character Statistics").GetComponentsInChildren<Text>();
        var goo = thisButton.GetComponent<EquipmentBarScript>();
        var go = Instance.selectedCharToEquip;

        EquipableItem currentItem = DerriveEquipmentType(thisButton.GetComponent<EquipmentBarScript>().EquipmentType);

        if (Instance.SelectedEquipment == thisButton)
        {
            if (currentItem.Name != "Empty")
            {
                go.strength -= Convert.ToInt32(currentItem.STRBonus);
                go.speed -= Convert.ToInt32(currentItem.SPDBonus);
                go.defense -= Convert.ToInt32(currentItem.DEFBonus);
                go.stamina -= Convert.ToInt32(currentItem.STMNABonus);
                go.magic -= Convert.ToInt32(currentItem.MAGBonus);
                go.attack -= Convert.ToInt32(currentItem.ATKBonus);
                go.evasion -= Convert.ToInt32(currentItem.EVSINBonus);
                go.magicEvasion -= Convert.ToInt32(currentItem.MagEVSINBonus);
                go.magicDefense -= Convert.ToInt32(currentItem.MagDefBonus);
            }
            foreach (Item i in ItemList.Instance().Items)
            {
                if (i.Name == goo.name)
                {
                    foreach (EquipableItem e in MasterEquipmentContainer.Instance.Equipment)
                    {
                        if (e.Name == i.Name)
                        {
                            i.Count -= 1;
                            equipItem(e, thisButton);
                            break;
                        }
                    }
                    break;
                }
            }
            go.strength = Convert.ToInt32(Math.Floor((go.strength + goo.STRBonus) * goo.STRPercentBonus));
            go.speed = Convert.ToInt32(Math.Floor((go.speed + goo.SPDBonus) * goo.SPDPercentBonus));
            go.stamina = Convert.ToInt32(Math.Floor((go.stamina + goo.STMNABonus) * goo.STMNAPercentBonus));
            go.magic = Convert.ToInt32(Math.Floor((go.magic + goo.MAGBonus) * goo.MAGPercentBonus));
            go.defense = Convert.ToInt32(Math.Floor((go.defense + goo.DEFBonus) * goo.DEFPercentBonus));
            go.attack = Convert.ToInt32(Math.Floor((go.attack + goo.ATKBonus) * goo.ATKPercentBonus));
            go.evasion = Convert.ToInt32(Math.Floor((go.evasion + goo.EVSINBonus) * goo.EVSINPercentBonus));
            go.magicEvasion = Convert.ToInt32(Math.Floor((go.magicEvasion + goo.MagEVSINBonus) * goo.MagEVSINPercentBonus));
            go.magicDefense = Convert.ToInt32(Math.Floor((go.magicDefense + goo.MagDefBonus) * goo.MagDefPercentBonus));

            selectedCharacter[9].text = go.strength.ToString();
            selectedCharacter[10].text = go.speed.ToString();
            selectedCharacter[11].text = go.stamina.ToString();
            selectedCharacter[12].text = go.magic.ToString();
            selectedCharacter[13].text = go.attack.ToString();
            selectedCharacter[14].text = go.defense.ToString();
            selectedCharacter[15].text = go.evasion.ToString();
            selectedCharacter[16].text = go.magicDefense.ToString();
            selectedCharacter[17].text = go.magicEvasion.ToString();

            goo.Count -= 1;
            if (goo.Count <= 0)
            {
                Destroy(thisButton);
            }
            else
            {
                thisButton.GetComponentsInChildren<Text>()[1].text = goo.Count.ToString();
            }
            if (currentItem.Name != "Empty")
            {
                if (currentItem.Name != goo.name)
                {
                    Instance.UpdateEquipmentList(currentItem.EquipmentType.ToString());

                    bool Found = false;

                    foreach (Item i in ItemList.Instance().Items)
                    {
                        if (i.Name == currentItem.Name)
                        {
                            i.Count += 1;
                            Found = true;
                            break;
                        }
                    }

                    if (Found == false)
                    {
                        ItemList.Instance().Items.Add(currentItem);
                    }
                }
                else
                {
                    foreach (Item i in ItemList.Instance().Items)
                    {
                        if (i.Name == currentItem.Name)
                        {
                            i.Count += 1;
                        }
                    }
                }
            }

            Instance.UpdateEquipmentList(currentItem.EquipmentType.ToString());
        }

        else
        {
            if (currentItem.Name == "Empty")
            {
                selectedCharacter[18].text = (Instance.AddedValue(go.strength, goo.STRBonus) * goo.STRPercentBonus).ToString();
                selectedCharacter[19].text = (Instance.AddedValue(go.speed, goo.SPDBonus) * goo.SPDPercentBonus).ToString();
                selectedCharacter[20].text = (Instance.AddedValue(go.stamina, goo.STMNABonus) * goo.STMNAPercentBonus).ToString();
                selectedCharacter[21].text = (Instance.AddedValue(go.magic, goo.MAGBonus) * goo.MAGPercentBonus).ToString();
                selectedCharacter[22].text = (Instance.AddedValue(go.attack, goo.ATKBonus) * goo.ATKPercentBonus).ToString();
                selectedCharacter[23].text = (Instance.AddedValue(go.defense, goo.DEFBonus) * goo.DEFPercentBonus).ToString();
                selectedCharacter[24].text = (Instance.AddedValue(go.evasion, goo.EVSINBonus) * goo.EVSINPercentBonus).ToString();
                selectedCharacter[25].text = (Instance.AddedValue(go.magicDefense, goo.MagDefBonus) * goo.MagDefPercentBonus).ToString();
                selectedCharacter[26].text = (Instance.AddedValue(go.magicEvasion, goo.MagEVSINBonus) * goo.MagEVSINPercentBonus).ToString();
            }
            else
            {
                selectedCharacter[18].text = (Instance.AddTriValue(go.strength, goo.STRBonus, -currentItem.STRBonus) * (-currentItem.STRPercentBonus)).ToString();
                selectedCharacter[18].text = (Instance.AddTriValue(go.speed, goo.SPDBonus, -currentItem.SPDBonus) * (-currentItem.SPDPercentBonus)).ToString();
                selectedCharacter[18].text = (Instance.AddTriValue(go.stamina, goo.STMNABonus, -currentItem.STMNABonus) * (-currentItem.STMNAPercentBonus)).ToString();
                selectedCharacter[18].text = (Instance.AddTriValue(go.magic, goo.MAGBonus, -currentItem.MAGBonus) * (-currentItem.MAGPercentBonus)).ToString();
                selectedCharacter[18].text = (Instance.AddTriValue(go.attack, goo.ATKBonus, -currentItem.ATKBonus) * (-currentItem.ATKPercentBonus)).ToString();
                selectedCharacter[18].text = (Instance.AddTriValue(go.defense, goo.DEFBonus, -currentItem.DEFBonus) * (-currentItem.DEFPercentBonus)).ToString();
                selectedCharacter[18].text = (Instance.AddTriValue(go.evasion, goo.EVSINBonus, -currentItem.EVSINBonus) * (-currentItem.EVSINPercentBonus)).ToString();
                selectedCharacter[18].text = (Instance.AddTriValue(go.magicDefense, goo.MagDefBonus, -currentItem.MagDefBonus) * (-currentItem.MagDefPercentBonus)).ToString();
                selectedCharacter[18].text = (Instance.AddTriValue(go.magicEvasion, goo.MagEVSINBonus, -currentItem.MagEVSINBonus) * (-currentItem.MagEVSINPercentBonus)).ToString();
            }
            Instance.SelectedEquipment = thisButton;
        }
    }

    // When you select the left hand button to equip
    public void LeftHandSelectionPress()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("item bar"))
        {
            Destroy(o);
        }

        equipmentSelectionScreen.SetActive(true);
        statBlocks.SetActive(false);
        equipScreen.SetActive(false);

        int VectorX = 1140;
        int VectorY = 582;
        string Place = "Hand";

        Instance.BodyUnequip.SetActive(false);
        Instance.HeadUnequip.SetActive(false);
        Instance.LeftHandUnequip.SetActive(true);
        Instance.RightHandUnequip.SetActive(false);
        Instance.Relic1Unequip.SetActive(false);
        Instance.Relic2Unequip.SetActive(false);

        Instance.LeftHandSelected = true;
        Instance.RightHandSelected = false;

        var t = ItemList.Instance().Items
            .Where(x => x.ItemType == ItemType.EquipableItem)
            .Select(x => MasterEquipmentContainer.Instance.Equipment.First(e => e.Name == x.Name))
            .Select(x =>
            {
                GameObject go = Instantiate(EquipmentBar, new Vector3(VectorX, VectorY, 0), Quaternion.identity) as GameObject;
                go.transform.SetParent(GameObject.Find("Equipment Selection").transform);

                bool delete = true;
                foreach (string s in x.canEquip)
                {
                    if (s == Instance.selectedCharToEquip.Name && Place == x.EquipmentType.ToString())
                    {
                        TextArray = go.GetComponentsInChildren<Text>();

                        var goo = go.GetComponent<EquipmentBarScript>();

                        goo.HPBonus = x.HPBonus;
                        goo.MPBonus = x.MPBonus;
                        goo.BTLPWRBonus = x.BTLPWRBonus;
                        goo.STRBonus = x.STRBonus;
                        goo.SPDBonus = x.SPDBonus;
                        goo.DEFBonus = x.DEFBonus;
                        goo.STMNABonus = x.STMNABonus;
                        goo.MAGBonus = x.MAGBonus;
                        goo.ATKBonus = x.ATKBonus;
                        goo.EVSINBonus = x.EVSINBonus;
                        goo.MagEVSINBonus = x.MagEVSINBonus;
                        goo.MagDefBonus = x.MagDefBonus;
                        goo.HPPercentBonus = x.HPPercentBonus;
                        goo.MPPercentBonus = x.MPPercentBonus;
                        goo.BTLPWRPercentBonus = x.BTLPWRPercentBonus;
                        goo.STRPercentBonus = x.STRPercentBonus;
                        goo.SPDPercentBonus = x.SPDPercentBonus;
                        goo.DEFPercentBonus = x.DEFPercentBonus;
                        goo.STMNAPercentBonus = x.STMNAPercentBonus;
                        goo.MAGPercentBonus = x.MAGPercentBonus;
                        goo.ATKPercentBonus = x.EVSINPercentBonus;
                        goo.MagEVSINPercentBonus = x.MagEVSINPercentBonus;
                        goo.MagDefPercentBonus = x.MagDefPercentBonus;

                        goo.canEquip.Clear();
                        goo.canEquip.AddRange(x.canEquip);

                        goo.EquipmentType = x.EquipmentType;
                        delete = false;
                        foreach (Item i in ItemList.Instance().Items)
                        {
                            if (i.Name == x.Name)
                            {
                                goo.Count = i.Count;
                                if (goo.Count <= 0)
                                {
                                    delete = true;
                                }
                                break;
                            }
                        }
                        goo.Description = x.Description;
                        go.name = x.Name;
                        go.GetComponentsInChildren<Text>()[0].text = x.Name;
                        go.GetComponentsInChildren<Text>()[1].text = goo.Count.ToString();


                        VectorX += 300;
                    }
                }

                if (delete)
                {
                    Destroy(go);
                }

                return go;
            })
            .ToList();
        equipableItems = t;
    }

    public void UpdateEquipmentList(string Place)
    {
        int VectorX = 1140;
        int VectorY = 582;

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("item bar"))
        {
            Destroy(g);
        }

        var t = ItemList.Instance().Items
            .Where(x => x.ItemType == ItemType.EquipableItem)
            .Select(x => MasterEquipmentContainer.Instance.Equipment.First(e => e.Name == x.Name))
            .Select(x =>
            {
                GameObject go = Instantiate(Instance.EquipmentBar, new Vector3(VectorX, VectorY, 0), Quaternion.identity) as GameObject;
                go.transform.SetParent(GameObject.Find("Equipment Selection").transform);

                bool delete = true;
                foreach (string s in x.canEquip)
                {
                    if (s == Instance.selectedCharToEquip.Name && Place == x.EquipmentType.ToString())
                    {
                        TextArray = go.GetComponentsInChildren<Text>();

                        var goo = go.GetComponent<EquipmentBarScript>();

                        goo.HPBonus = x.HPBonus;
                        goo.MPBonus = x.MPBonus;
                        goo.BTLPWRBonus = x.BTLPWRBonus;
                        goo.STRBonus = x.STRBonus;
                        goo.SPDBonus = x.SPDBonus;
                        goo.DEFBonus = x.DEFBonus;
                        goo.STMNABonus = x.STMNABonus;
                        goo.MAGBonus = x.MAGBonus;
                        goo.ATKBonus = x.ATKBonus;
                        goo.EVSINBonus = x.EVSINBonus;
                        goo.MagEVSINBonus = x.MagEVSINBonus;
                        goo.MagDefBonus = x.MagDefBonus;
                        goo.HPPercentBonus = x.HPPercentBonus;
                        goo.MPPercentBonus = x.MPPercentBonus;
                        goo.BTLPWRPercentBonus = x.BTLPWRPercentBonus;
                        goo.STRPercentBonus = x.STRPercentBonus;
                        goo.SPDPercentBonus = x.SPDPercentBonus;
                        goo.DEFPercentBonus = x.DEFPercentBonus;
                        goo.STMNAPercentBonus = x.STMNAPercentBonus;
                        goo.MAGPercentBonus = x.MAGPercentBonus;
                        goo.ATKPercentBonus = x.EVSINPercentBonus;
                        goo.MagEVSINPercentBonus = x.MagEVSINPercentBonus;
                        goo.MagDefPercentBonus = x.MagDefPercentBonus;

                        goo.canEquip.Clear();
                        goo.canEquip.AddRange(x.canEquip);

                        goo.EquipmentType = x.EquipmentType;
                        delete = false;
                        foreach (Item i in ItemList.Instance().Items)
                        {
                            if (i.Name == x.Name)
                            {
                                goo.Count = i.Count;
                                if (goo.Count <= 0)
                                {
                                    delete = true;
                                }
                                break;
                            }
                        }
                        goo.Description = x.Description;
                        go.name = x.Name;
                        go.GetComponentsInChildren<Text>()[0].text = x.Name;
                        go.GetComponentsInChildren<Text>()[1].text = goo.Count.ToString();
                    }
                }

                if (delete)
                {
                    Destroy(go);
                }
                else
                {
                    VectorX += 300;
                }

                return go;
            })
            .ToList();
    }

    public void ButtonLeftHandUnequip()
    {
        var gob = Instance.selectedCharToEquip.GetComponent<PositionTwoContainer>();
        var gb = gob.LeftHandSlot;
        var selectedCharacter = GameObject.Find("Character Statistics").GetComponentsInChildren<Text>();

        if (Instance.SelectedEquipment == Instance.LeftHandUnequip)
        {
            GameObject.Find("Left Hand").GetComponentsInChildren<Text>()[0].text = "Empty";

            gob.strength -= gb.STRBonus;
            gob.speed -= gb.SPDBonus;
            gob.defense -= gb.DEFBonus;
            gob.stamina -= gb.STMNABonus;
            gob.magic -= gb.MAGBonus;
            gob.attack -= gb.ATKBonus;
            gob.evasion -= gb.EVSINBonus;
            gob.magicEvasion -= gb.MagEVSINBonus;
            gob.magicDefense -= gb.MagDefBonus;

            selectedCharacter[9].text = gob.strength.ToString();
            selectedCharacter[10].text = gob.speed.ToString();
            selectedCharacter[11].text = gob.stamina.ToString();
            selectedCharacter[12].text = gob.magic.ToString();
            selectedCharacter[13].text = gob.attack.ToString();
            selectedCharacter[14].text = gob.defense.ToString();
            selectedCharacter[15].text = gob.evasion.ToString();
            selectedCharacter[16].text = gob.magicDefense.ToString();
            selectedCharacter[17].text = gob.magicEvasion.ToString();

            if (gb.Name != "Empty")
            {
                EquipableItem thing = null;
                foreach (EquipableItem e in MasterEquipmentContainer.Instance.Equipment)
                {
                    if (gb.Name == e.Name)
                    {
                        thing = e;
                        break;
                    }
                }

                List<Item> MarkedForDeletion = new List<Item>();
                foreach (Item i in ItemList.Instance().Items)
                {
                    if (i.Count <= 0)
                    {
                        MarkedForDeletion.Add(i);
                        continue;
                    }
                }

                foreach (Item i in MarkedForDeletion)
                {
                    ItemList.Instance().Items.Remove(i);
                }

                bool FoundItem = false;
                foreach (Item i in ItemList.Instance().Items)
                {
                    if (i.Name == thing.Name)
                    {
                        FoundItem = true;
                        i.Count += 1;
                        break;
                    }
                }

                if (!FoundItem)
                {
                    thing.Count += 1;
                    ItemList.Instance().Items.Add(thing);

                    foreach (GameObject o in GameObject.FindGameObjectsWithTag("item bar"))
                    {
                        Destroy(o);
                    }

                    Instance.equipmentSelectionScreen.SetActive(true);
                    Instance.statBlocks.SetActive(false);
                    Instance.equipScreen.SetActive(false);

                    string Place = "Hand";

                    Instance.BodyUnequip.SetActive(false);
                    Instance.HeadUnequip.SetActive(false);
                    Instance.LeftHandUnequip.SetActive(true);
                    Instance.RightHandUnequip.SetActive(false);
                    Instance.Relic1Unequip.SetActive(false);
                    Instance.Relic2Unequip.SetActive(false);

                    Instance.LeftHandSelected = true;
                    Instance.RightHandSelected = false;

                    Instance.UpdateEquipmentList(Place);
                }
                else
                {
                    foreach (GameObject o in GameObject.FindGameObjectsWithTag("item bar"))
                    {
                        if (o.name == gb.Name)
                        {
                            o.GetComponentsInChildren<Text>()[1].text = (AddedValue(o.GetComponent<EquipmentBarScript>().Count, 1)).ToString();
                            break;
                        }
                    }
                }
            }

            Instance.selectedCharToEquip.LeftHandSlot = new EquipableItem { Name = "Empty", Count = 1 };
            print(gb.Name);
            Instance.SelectedEquipment = null;
            return;
        }
        else
        {
            selectedCharacter[18].text = (Instance.AddedValue(gob.strength, -gb.STRBonus) / (gb.STRPercentBonus)).ToString();
            selectedCharacter[19].text = (Instance.AddedValue(gob.speed, -gb.SPDBonus) / (gb.SPDPercentBonus)).ToString();
            selectedCharacter[20].text = (Instance.AddedValue(gob.stamina, -gb.STMNABonus) / (gb.STMNAPercentBonus)).ToString();
            selectedCharacter[21].text = (Instance.AddedValue(gob.magic, -gb.MAGBonus) / (gb.MAGPercentBonus)).ToString();
            selectedCharacter[22].text = (Instance.AddedValue(gob.attack, -gb.ATKBonus) / (gb.ATKPercentBonus)).ToString();
            selectedCharacter[23].text = (Instance.AddedValue(gob.defense, -gb.DEFBonus) / (gb.DEFPercentBonus)).ToString();
            selectedCharacter[24].text = (Instance.AddedValue(gob.evasion, -gb.EVSINBonus) / (gb.EVSINPercentBonus)).ToString();
            selectedCharacter[25].text = (Instance.AddedValue(gob.magicDefense, -gb.MagDefBonus) / (gb.MagDefPercentBonus)).ToString();
            selectedCharacter[26].text = (Instance.AddedValue(gob.magicEvasion, -gb.MagEVSINBonus) / (gb.MagEVSINPercentBonus)).ToString();

            Instance.SelectedEquipment = Instance.LeftHandUnequip;
        }
    }

    // When you press the body button
    public void ChestSelectionPress()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("item bar"))
        {
            Destroy(o);
        }

        equipmentSelectionScreen.SetActive(true);
        statBlocks.SetActive(false);
        equipScreen.SetActive(false);

        Instance.BodyUnequip.SetActive(true);
        Instance.HeadUnequip.SetActive(false);
        Instance.LeftHandUnequip.SetActive(false);
        Instance.RightHandUnequip.SetActive(false);
        Instance.Relic1Unequip.SetActive(false);
        Instance.Relic2Unequip.SetActive(false);

        Instance.LeftHandSelected = false;
        Instance.RightHandSelected = false;

        UpdateEquipmentList("Chest");
    }

    // When you press the unequip button while viewing chest equipment
    public void ButtonChestUnequip()
    {
        var gob = Instance.selectedCharToEquip;
        var gb = gob.BodySlot;
        var selectedCharacter = GameObject.Find("Character Statistics").GetComponentsInChildren<Text>();


        selectedCharacter[9].text = gob.strength.ToString();
        selectedCharacter[10].text = gob.speed.ToString();
        selectedCharacter[11].text = gob.stamina.ToString();
        selectedCharacter[12].text = gob.magic.ToString();
        selectedCharacter[13].text = gob.attack.ToString();
        selectedCharacter[14].text = gob.defense.ToString();
        selectedCharacter[15].text = gob.evasion.ToString();
        selectedCharacter[16].text = gob.magicDefense.ToString();
        selectedCharacter[17].text = gob.magicEvasion.ToString();
        selectedCharacter[18].text = gob.strength.ToString();
        selectedCharacter[19].text = gob.speed.ToString();
        selectedCharacter[20].text = gob.stamina.ToString();
        selectedCharacter[21].text = gob.magic.ToString();
        selectedCharacter[22].text = gob.attack.ToString();
        selectedCharacter[23].text = gob.defense.ToString();
        selectedCharacter[24].text = gob.evasion.ToString();
        selectedCharacter[25].text = gob.magicDefense.ToString();
        selectedCharacter[26].text = gob.magicEvasion.ToString();

        if (Instance.SelectedEquipment == Instance.BodyUnequip)
        {
            GameObject.Find("Body").GetComponentsInChildren<Text>()[0].text = "Empty";

            if (gb.Name != "Empty")
            {
                gob.strength = Convert.ToInt32(Math.Floor((gob.strength - gb.STRBonus) / gb.STRPercentBonus));
                gob.speed = Convert.ToInt32(Math.Floor((gob.speed - gb.SPDBonus) / gb.SPDPercentBonus));
                gob.defense = Convert.ToInt32(Math.Floor((gob.defense - gb.DEFBonus) / gb.DEFPercentBonus));
                gob.stamina = Convert.ToInt32(Math.Floor((gob.stamina - gb.STMNABonus) / gb.STMNAPercentBonus));
                gob.magic = Convert.ToInt32(Math.Floor((gob.magic - gb.MAGBonus) / gb.MAGPercentBonus));
                gob.attack = Convert.ToInt32(Math.Floor((gob.attack - gb.ATKBonus) / gb.ATKPercentBonus));
                gob.evasion = Convert.ToInt32(Math.Floor((gob.evasion - gb.EVSINBonus) / gb.EVSINPercentBonus));
                gob.magicEvasion = Convert.ToInt32(Math.Floor((gob.magicEvasion - gb.MagEVSINBonus) / gb.MagEVSINPercentBonus));
                gob.magicDefense = Convert.ToInt32(Math.Floor((gob.magicDefense - gb.MagDefBonus) / gb.MagDefPercentBonus));

                EquipableItem thing = null;
                foreach (EquipableItem e in MasterEquipmentContainer.Instance.Equipment)
                {
                    if (gb.Name == e.Name)
                    {
                        thing = e;
                        break;
                    }
                }

                int num = 1;
                foreach (Item i in ItemList.Instance().Items)
                {
                    if (i.Name == thing.Name)
                    {
                        i.Count += 1;
                        break;
                    }

                    if (num == ItemList.Instance().Items.Count)
                    {
                        thing.Count += 1;
                        ItemList.Instance().Items.Add(thing);
                        break;
                    }
                    num += 1;
                }

                foreach (GameObject o in GameObject.FindGameObjectsWithTag("item bar"))
                {
                    if (o.name == gb.Name)
                    {

                        o.GetComponentsInChildren<Text>()[1].text = (AddedValue(o.GetComponent<EquipmentBarScript>().Count, 1)).ToString();
                        break;
                    }
                }
            }

            gob.BodySlot = new EquipableItem { Name = "Empty", Count = 1 };
            Instance.SelectedEquipment = null;
            UpdateEquipmentList("Chest");

            return;
        }
        else
        {
            selectedCharacter[18].text = (Instance.AddedValue(gob.strength, -gb.STRBonus) / (gb.STRPercentBonus)).ToString();
            selectedCharacter[19].text = (Instance.AddedValue(gob.speed, -gb.SPDBonus) / (gb.SPDPercentBonus)).ToString();
            selectedCharacter[20].text = (Instance.AddedValue(gob.stamina, -gb.STMNABonus) / (gb.STMNAPercentBonus)).ToString();
            selectedCharacter[21].text = (Instance.AddedValue(gob.magic, -gb.MAGBonus) / (gb.MAGPercentBonus)).ToString();
            selectedCharacter[22].text = (Instance.AddedValue(gob.attack, -gb.ATKBonus) / (gb.ATKPercentBonus)).ToString();
            selectedCharacter[23].text = (Instance.AddedValue(gob.defense, -gb.DEFBonus) / (gb.DEFPercentBonus)).ToString();
            selectedCharacter[24].text = (Instance.AddedValue(gob.evasion, -gb.EVSINBonus) / (gb.EVSINPercentBonus)).ToString();
            selectedCharacter[25].text = (Instance.AddedValue(gob.magicDefense, -gb.MagDefBonus) / (gb.MagDefPercentBonus)).ToString();
            selectedCharacter[26].text = (Instance.AddedValue(gob.magicEvasion, -gb.MagEVSINBonus) / (gb.MagEVSINPercentBonus)).ToString();

            Instance.SelectedEquipment = Instance.BodyUnequip;
        }
    }
    // When you press the helmet button
    public void HeadSelectionPress()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("item bar"))
        {
            Destroy(o);
        }

        equipmentSelectionScreen.SetActive(true);
        statBlocks.SetActive(false);
        equipScreen.SetActive(false);

        Instance.BodyUnequip.SetActive(false);
        Instance.HeadUnequip.SetActive(true);
        Instance.LeftHandUnequip.SetActive(false);
        Instance.RightHandUnequip.SetActive(false);
        Instance.Relic1Unequip.SetActive(false);
        Instance.Relic2Unequip.SetActive(false);

        Instance.LeftHandSelected = false;
        Instance.RightHandSelected = false;

        UpdateEquipmentList("Head");
    }

    // When you press the unequip button while viewing head equipment
    public void ButtonHeadUnequip()
    {
        var gob = Instance.selectedCharToEquip;
        var gb = gob.HeadSlot;
        var selectedCharacter = GameObject.Find("Character Statistics").GetComponentsInChildren<Text>();

        if (Instance.SelectedEquipment == Instance.HeadUnequip)
        {
            GameObject.Find("Helmet").GetComponentsInChildren<Text>()[0].text = "Empty";

            if (gb.Name != "Empty")
            {
                gob.strength = Convert.ToInt32(Math.Floor((gob.strength - gb.STRBonus) / gb.STRPercentBonus));
                gob.speed = Convert.ToInt32(Math.Floor((gob.speed - gb.SPDBonus) / gb.SPDPercentBonus));
                gob.defense = Convert.ToInt32(Math.Floor((gob.defense - gb.DEFBonus) / gb.DEFPercentBonus));
                gob.stamina = Convert.ToInt32(Math.Floor((gob.stamina - gb.STMNABonus) / gb.STMNAPercentBonus));
                gob.magic = Convert.ToInt32(Math.Floor((gob.magic - gb.MAGBonus) / gb.MAGPercentBonus));
                gob.attack = Convert.ToInt32(Math.Floor((gob.attack - gb.ATKBonus) / gb.ATKPercentBonus));
                gob.evasion = Convert.ToInt32(Math.Floor((gob.evasion - gb.EVSINBonus) / gb.EVSINPercentBonus));
                gob.magicEvasion = Convert.ToInt32(Math.Floor((gob.magicEvasion - gb.MagEVSINBonus) / gb.MagEVSINPercentBonus));
                gob.magicDefense = Convert.ToInt32(Math.Floor((gob.magicDefense - gb.MagDefBonus) / gb.MagDefPercentBonus));

                selectedCharacter[9].text = gob.strength.ToString();
                selectedCharacter[10].text = gob.speed.ToString();
                selectedCharacter[11].text = gob.stamina.ToString();
                selectedCharacter[12].text = gob.magic.ToString();
                selectedCharacter[13].text = gob.attack.ToString();
                selectedCharacter[14].text = gob.defense.ToString();
                selectedCharacter[15].text = gob.evasion.ToString();
                selectedCharacter[16].text = gob.magicDefense.ToString();
                selectedCharacter[17].text = gob.magicEvasion.ToString();
                selectedCharacter[18].text = gob.strength.ToString();
                selectedCharacter[19].text = gob.speed.ToString();
                selectedCharacter[20].text = gob.stamina.ToString();
                selectedCharacter[21].text = gob.magic.ToString();
                selectedCharacter[22].text = gob.attack.ToString();
                selectedCharacter[23].text = gob.defense.ToString();
                selectedCharacter[24].text = gob.evasion.ToString();
                selectedCharacter[25].text = gob.magicDefense.ToString();
                selectedCharacter[26].text = gob.magicEvasion.ToString();

                EquipableItem thing = null;
                foreach (EquipableItem e in MasterEquipmentContainer.Instance.Equipment)
                {
                    if (gb.Name == e.Name)
                    {
                        thing = e;
                        break;
                    }
                }

                int num = 1;
                foreach (Item i in ItemList.Instance().Items)
                {
                    if (i.Name == thing.Name)
                    {
                        i.Count += 1;
                        break;
                    }

                    if (num == ItemList.Instance().Items.Count)
                    {
                        thing.Count += 1;
                        ItemList.Instance().Items.Add(thing);
                    }
                    num += 1;
                }

                foreach (GameObject o in GameObject.FindGameObjectsWithTag("item bar"))
                {
                    if (o.name == gb.Name)
                    {
                        o.GetComponentsInChildren<Text>()[1].text = (AddedValue(o.GetComponent<EquipmentBarScript>().Count, 1)).ToString();
                        break;
                    }
                }
            }

            gob.HeadSlot = new EquipableItem { Name = "Empty", Count = 1 };
            Instance.SelectedEquipment = null;
            UpdateEquipmentList("Head");

            return;
        }
        else
        {
            selectedCharacter[18].text = (Instance.AddedValue(gob.strength, -gb.STRBonus) / (gb.STRPercentBonus)).ToString();
            selectedCharacter[19].text = (Instance.AddedValue(gob.speed, -gb.SPDBonus) / (gb.SPDPercentBonus)).ToString();
            selectedCharacter[20].text = (Instance.AddedValue(gob.stamina, -gb.STMNABonus) / (gb.STMNAPercentBonus)).ToString();
            selectedCharacter[21].text = (Instance.AddedValue(gob.magic, -gb.MAGBonus) / (gb.MAGPercentBonus)).ToString();
            selectedCharacter[22].text = (Instance.AddedValue(gob.attack, -gb.ATKBonus) / (gb.ATKPercentBonus)).ToString();
            selectedCharacter[23].text = (Instance.AddedValue(gob.defense, -gb.DEFBonus) / (gb.DEFPercentBonus)).ToString();
            selectedCharacter[24].text = (Instance.AddedValue(gob.evasion, -gb.EVSINBonus) / (gb.EVSINPercentBonus)).ToString();
            selectedCharacter[25].text = (Instance.AddedValue(gob.magicDefense, -gb.MagDefBonus) / (gb.MagDefPercentBonus)).ToString();
            selectedCharacter[26].text = (Instance.AddedValue(gob.magicEvasion, -gb.MagEVSINBonus) / (gb.MagEVSINPercentBonus)).ToString();

            Instance.SelectedEquipment = Instance.HeadUnequip;
        }
    }

    //when you press the right hand button
    public void RightHandSelectionPress()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("item bar"))
        {
            Destroy(o);
        }

        equipmentSelectionScreen.SetActive(true);
        statBlocks.SetActive(false);
        equipScreen.SetActive(false);

        Instance.BodyUnequip.SetActive(false);
        Instance.HeadUnequip.SetActive(false);
        Instance.LeftHandUnequip.SetActive(false);
        Instance.RightHandUnequip.SetActive(true);
        Instance.Relic1Unequip.SetActive(false);
        Instance.Relic2Unequip.SetActive(false);

        Instance.LeftHandSelected = false;
        Instance.RightHandSelected = true;

        UpdateEquipmentList("Hand");
    }

    // When you press unequip while viewing right hand equipment
    public void ButtonRightHandUnequip()
    {
        var gob = Instance.selectedCharToEquip;
        var gb = gob.RightHandSlot;
        var selectedCharacter = GameObject.Find("Character Statistics").GetComponentsInChildren<Text>();

        if (Instance.SelectedEquipment == Instance.RightHandUnequip)
        {
            GameObject.Find("Right Hand").GetComponentsInChildren<Text>()[0].text = "Empty";

            if (gb.Name != "Empty")
            {
                gob.strength = Convert.ToInt32(Math.Floor((gob.strength - gb.STRBonus) / gb.STRPercentBonus));
                gob.speed = Convert.ToInt32(Math.Floor((gob.speed - gb.SPDBonus) / gb.SPDPercentBonus));
                gob.defense = Convert.ToInt32(Math.Floor((gob.defense - gb.DEFBonus) / gb.DEFPercentBonus));
                gob.stamina = Convert.ToInt32(Math.Floor((gob.stamina - gb.STMNABonus) / gb.STMNAPercentBonus));
                gob.magic = Convert.ToInt32(Math.Floor((gob.magic - gb.MAGBonus) / gb.MAGPercentBonus));
                gob.attack = Convert.ToInt32(Math.Floor((gob.attack - gb.ATKBonus) / gb.ATKPercentBonus));
                gob.evasion = Convert.ToInt32(Math.Floor((gob.evasion - gb.EVSINBonus) / gb.EVSINPercentBonus));
                gob.magicEvasion = Convert.ToInt32(Math.Floor((gob.magicEvasion - gb.MagEVSINBonus) / gb.MagEVSINPercentBonus));
                gob.magicDefense = Convert.ToInt32(Math.Floor((gob.magicDefense - gb.MagDefBonus) / gb.MagDefPercentBonus));

                selectedCharacter[9].text = gob.strength.ToString();
                selectedCharacter[10].text = gob.speed.ToString();
                selectedCharacter[11].text = gob.stamina.ToString();
                selectedCharacter[12].text = gob.magic.ToString();
                selectedCharacter[13].text = gob.attack.ToString();
                selectedCharacter[14].text = gob.defense.ToString();
                selectedCharacter[15].text = gob.evasion.ToString();
                selectedCharacter[16].text = gob.magicDefense.ToString();
                selectedCharacter[17].text = gob.magicEvasion.ToString();
                selectedCharacter[18].text = gob.strength.ToString();
                selectedCharacter[19].text = gob.speed.ToString();
                selectedCharacter[20].text = gob.stamina.ToString();
                selectedCharacter[21].text = gob.magic.ToString();
                selectedCharacter[22].text = gob.attack.ToString();
                selectedCharacter[23].text = gob.defense.ToString();
                selectedCharacter[24].text = gob.evasion.ToString();
                selectedCharacter[25].text = gob.magicDefense.ToString();
                selectedCharacter[26].text = gob.magicEvasion.ToString();

                EquipableItem thing = null;
                foreach (EquipableItem e in MasterEquipmentContainer.Instance.Equipment)
                {
                    if (gb.Name == e.Name)
                    {
                        thing = e;
                        break;
                    }
                }

                List<Item> MarkedForDeletion = new List<Item>();
                foreach (Item i in ItemList.Instance().Items)
                {
                    if (i.Count <= 0)
                    {
                        MarkedForDeletion.Add(i);
                        continue;
                    }
                }

                foreach (Item i in MarkedForDeletion)
                {
                    ItemList.Instance().Items.Remove(i);
                }

                bool FoundItem = false;
                foreach (Item i in ItemList.Instance().Items)
                {
                    if (i.Name == thing.Name)
                    {
                        FoundItem = true;
                        i.Count += 1;
                        break;
                    }
                }

                if (!FoundItem)
                {
                    thing.Count += 1;
                    ItemList.Instance().Items.Add(thing);

                    foreach (GameObject o in GameObject.FindGameObjectsWithTag("item bar"))
                    {
                        Destroy(o);
                    }

                    Instance.equipmentSelectionScreen.SetActive(true);
                    Instance.statBlocks.SetActive(false);
                    Instance.equipScreen.SetActive(false);

                    Instance.BodyUnequip.SetActive(false);
                    Instance.HeadUnequip.SetActive(false);
                    Instance.LeftHandUnequip.SetActive(false);
                    Instance.RightHandUnequip.SetActive(true);
                    Instance.Relic1Unequip.SetActive(false);
                    Instance.Relic2Unequip.SetActive(false);

                    Instance.LeftHandSelected = false;
                    Instance.RightHandSelected = true;

                    Instance.UpdateEquipmentList("Hand");
                }
                else
                {
                    foreach (GameObject o in GameObject.FindGameObjectsWithTag("item bar"))
                    {
                        if (o.name == gb.Name)
                        {
                            o.GetComponentsInChildren<Text>()[1].text = (AddedValue(o.GetComponent<EquipmentBarScript>().Count, 1)).ToString();
                            break;
                        }
                    }
                }
            }

            Instance.selectedCharToEquip.RightHandSlot = new EquipableItem { Name = "Empty", Count = 1 };
            Instance.SelectedEquipment = null;
            return;
        }
        else
        {
            selectedCharacter[18].text = (Instance.AddedValue(gob.strength, -gb.STRBonus) / (gb.STRPercentBonus)).ToString();
            selectedCharacter[19].text = (Instance.AddedValue(gob.speed, -gb.SPDBonus) / (gb.SPDPercentBonus)).ToString();
            selectedCharacter[20].text = (Instance.AddedValue(gob.stamina, -gb.STMNABonus) / (gb.STMNAPercentBonus)).ToString();
            selectedCharacter[21].text = (Instance.AddedValue(gob.magic, -gb.MAGBonus) / (gb.MAGPercentBonus)).ToString();
            selectedCharacter[22].text = (Instance.AddedValue(gob.attack, -gb.ATKBonus) / (gb.ATKPercentBonus)).ToString();
            selectedCharacter[23].text = (Instance.AddedValue(gob.defense, -gb.DEFBonus) / (gb.DEFPercentBonus)).ToString();
            selectedCharacter[24].text = (Instance.AddedValue(gob.evasion, -gb.EVSINBonus) / (gb.EVSINPercentBonus)).ToString();
            selectedCharacter[25].text = (Instance.AddedValue(gob.magicDefense, -gb.MagDefBonus) / (gb.MagDefPercentBonus)).ToString();
            selectedCharacter[26].text = (Instance.AddedValue(gob.magicEvasion, -gb.MagEVSINBonus) / (gb.MagEVSINPercentBonus)).ToString();

            Instance.SelectedEquipment = Instance.RightHandUnequip;
        }
    }

    // When you press the left relic button
    public void LeftRelicSelectionPress()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("item bar"))
        {
            Destroy(o);
        }

        equipmentSelectionScreen.SetActive(true);
        statBlocks.SetActive(false);
        equipScreen.SetActive(false);

        Instance.BodyUnequip.SetActive(false);
        Instance.HeadUnequip.SetActive(false);
        Instance.LeftHandUnequip.SetActive(false);
        Instance.RightHandUnequip.SetActive(false);
        Instance.Relic1Unequip.SetActive(true);
        Instance.Relic2Unequip.SetActive(false);

        Instance.LeftRelicSelected = true;
        Instance.RightRelicSelected = false;

        UpdateEquipmentList("Relic");
    }

    // When you press the unequip button while looking at the left relic
    public void ButtonLeftRelicUnequip()
    {
        var gob = Instance.selectedCharToEquip;
        var gb = gob.LeftRelicSlot;
        var selectedCharacter = GameObject.Find("Character Statistics").GetComponentsInChildren<Text>();

        if (Instance.SelectedEquipment == Instance.Relic1Unequip)
        {
            GameObject.Find("Left Relic").GetComponentsInChildren<Text>()[0].text = "Empty";
            
            if (gb.Name != "Empty")
            {
                gob.strength = Convert.ToInt32(Math.Floor((gob.strength - gb.STRBonus) / gb.STRPercentBonus));
                gob.speed = Convert.ToInt32(Math.Floor((gob.speed - gb.SPDBonus) / gb.SPDPercentBonus));
                gob.defense = Convert.ToInt32(Math.Floor((gob.defense - gb.DEFBonus) / gb.DEFPercentBonus));
                gob.stamina = Convert.ToInt32(Math.Floor((gob.stamina - gb.STMNABonus) / gb.STMNAPercentBonus));
                gob.magic = Convert.ToInt32(Math.Floor((gob.magic - gb.MAGBonus) / gb.MAGPercentBonus));
                gob.attack = Convert.ToInt32(Math.Floor((gob.attack - gb.ATKBonus) / gb.ATKPercentBonus));
                gob.evasion = Convert.ToInt32(Math.Floor((gob.evasion - gb.EVSINBonus) / gb.EVSINPercentBonus));
                gob.magicEvasion = Convert.ToInt32(Math.Floor((gob.magicEvasion - gb.MagEVSINBonus) / gb.MagEVSINPercentBonus));
                gob.magicDefense = Convert.ToInt32(Math.Floor((gob.magicDefense - gb.MagDefBonus) / gb.MagDefPercentBonus));

                selectedCharacter[9].text = gob.strength.ToString();
                selectedCharacter[10].text = gob.speed.ToString();
                selectedCharacter[11].text = gob.stamina.ToString();
                selectedCharacter[12].text = gob.magic.ToString();
                selectedCharacter[13].text = gob.attack.ToString();
                selectedCharacter[14].text = gob.defense.ToString();
                selectedCharacter[15].text = gob.evasion.ToString();
                selectedCharacter[16].text = gob.magicDefense.ToString();
                selectedCharacter[17].text = gob.magicEvasion.ToString();
                selectedCharacter[18].text = gob.strength.ToString();
                selectedCharacter[19].text = gob.speed.ToString();
                selectedCharacter[20].text = gob.stamina.ToString();
                selectedCharacter[21].text = gob.magic.ToString();
                selectedCharacter[22].text = gob.attack.ToString();
                selectedCharacter[23].text = gob.defense.ToString();
                selectedCharacter[24].text = gob.evasion.ToString();
                selectedCharacter[25].text = gob.magicDefense.ToString();
                selectedCharacter[26].text = gob.magicEvasion.ToString();

                EquipableItem thing = null;
                foreach (EquipableItem e in MasterEquipmentContainer.Instance.Equipment)
                {
                    if (gb.Name == e.Name)
                    {
                        thing = e;
                        break;
                    }
                }

                List<Item> MarkedForDeletion = new List<Item>();
                foreach (Item i in ItemList.Instance().Items)
                {
                    if (i.Count <= 0)
                    {
                        MarkedForDeletion.Add(i);
                        continue;
                    }
                }

                foreach (Item i in MarkedForDeletion)
                {
                    ItemList.Instance().Items.Remove(i);
                }

                bool FoundItem = false;
                foreach (Item i in ItemList.Instance().Items)
                {
                    if (i.Name == thing.Name)
                    {
                        FoundItem = true;
                        i.Count += 1;
                        break;
                    }
                }

                if (!FoundItem)
                {
                    thing.Count += 1;
                    ItemList.Instance().Items.Add(thing);

                    foreach (GameObject o in GameObject.FindGameObjectsWithTag("item bar"))
                    {
                        Destroy(o);
                    }

                    Instance.UpdateEquipmentList("Relic");
                }
                else
                {
                    foreach (GameObject o in GameObject.FindGameObjectsWithTag("item bar"))
                    {
                        if (o.name == gb.Name)
                        {
                            o.GetComponentsInChildren<Text>()[1].text = (AddedValue(o.GetComponent<EquipmentBarScript>().Count, 1)).ToString();
                            break;
                        }
                    }
                }
            }

            Instance.selectedCharToEquip.LeftRelicSlot = new EquipableItem { Name = "Empty", Count = 1 };
            Instance.SelectedEquipment = null;
            return;
        }
        else
        {
            selectedCharacter[18].text = (Instance.AddedValue(gob.strength, -gb.STRBonus) / (gb.STRPercentBonus)).ToString();
            selectedCharacter[19].text = (Instance.AddedValue(gob.speed, -gb.SPDBonus) / (gb.SPDPercentBonus)).ToString();
            selectedCharacter[20].text = (Instance.AddedValue(gob.stamina, -gb.STMNABonus) / (gb.STMNAPercentBonus)).ToString();
            selectedCharacter[21].text = (Instance.AddedValue(gob.magic, -gb.MAGBonus) / (gb.MAGPercentBonus)).ToString();
            selectedCharacter[22].text = (Instance.AddedValue(gob.attack, -gb.ATKBonus) / (gb.ATKPercentBonus)).ToString();
            selectedCharacter[23].text = (Instance.AddedValue(gob.defense, -gb.DEFBonus) / (gb.DEFPercentBonus)).ToString();
            selectedCharacter[24].text = (Instance.AddedValue(gob.evasion, -gb.EVSINBonus) / (gb.EVSINPercentBonus)).ToString();
            selectedCharacter[25].text = (Instance.AddedValue(gob.magicDefense, -gb.MagDefBonus) / (gb.MagDefPercentBonus)).ToString();
            selectedCharacter[26].text = (Instance.AddedValue(gob.magicEvasion, -gb.MagEVSINBonus) / (gb.MagEVSINPercentBonus)).ToString();

            Instance.SelectedEquipment = Instance.Relic1Unequip;
        }
    }

    // When you press the right relic button
    public void RightRelicSelectionPress()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("item bar"))
        {
            Destroy(o);
        }

        equipmentSelectionScreen.SetActive(true);
        statBlocks.SetActive(false);
        equipScreen.SetActive(false);

        Instance.BodyUnequip.SetActive(false);
        Instance.HeadUnequip.SetActive(false);
        Instance.LeftHandUnequip.SetActive(false);
        Instance.RightHandUnequip.SetActive(false);
        Instance.Relic1Unequip.SetActive(false);
        Instance.Relic2Unequip.SetActive(true);

        Instance.LeftRelicSelected = false;
        Instance.RightRelicSelected = true;

        UpdateEquipmentList("Relic");
    }

    // When you press the unequip button while looking at the right relic
    public void ButtonRightRelicUnequip()
    {
        var gob = Instance.selectedCharToEquip;
        var gb = gob.RightRelicSlot;
        var selectedCharacter = GameObject.Find("Character Statistics").GetComponentsInChildren<Text>();

        if (Instance.SelectedEquipment == Instance.Relic2Unequip)
        {
            GameObject.Find("Right Relic").GetComponentsInChildren<Text>()[0].text = "Empty";

            if (gb.Name != "Empty")
            {
                gob.strength = Convert.ToInt32(Math.Floor((gob.strength - gb.STRBonus) / gb.STRPercentBonus));
                gob.speed = Convert.ToInt32(Math.Floor((gob.speed - gb.SPDBonus) / gb.SPDPercentBonus));
                gob.defense = Convert.ToInt32(Math.Floor((gob.defense - gb.DEFBonus) / gb.DEFPercentBonus));
                gob.stamina = Convert.ToInt32(Math.Floor((gob.stamina - gb.STMNABonus) / gb.STMNAPercentBonus));
                gob.magic = Convert.ToInt32(Math.Floor((gob.magic - gb.MAGBonus) / gb.MAGPercentBonus));
                gob.attack = Convert.ToInt32(Math.Floor((gob.attack - gb.ATKBonus) / gb.ATKPercentBonus));
                gob.evasion = Convert.ToInt32(Math.Floor((gob.evasion - gb.EVSINBonus) / gb.EVSINPercentBonus));
                gob.magicEvasion = Convert.ToInt32(Math.Floor((gob.magicEvasion - gb.MagEVSINBonus) / gb.MagEVSINPercentBonus));
                gob.magicDefense = Convert.ToInt32(Math.Floor((gob.magicDefense - gb.MagDefBonus) / gb.MagDefPercentBonus));

                selectedCharacter[9].text = gob.strength.ToString();
                selectedCharacter[10].text = gob.speed.ToString();
                selectedCharacter[11].text = gob.stamina.ToString();
                selectedCharacter[12].text = gob.magic.ToString();
                selectedCharacter[13].text = gob.attack.ToString();
                selectedCharacter[14].text = gob.defense.ToString();
                selectedCharacter[15].text = gob.evasion.ToString();
                selectedCharacter[16].text = gob.magicDefense.ToString();
                selectedCharacter[17].text = gob.magicEvasion.ToString();
                selectedCharacter[18].text = gob.strength.ToString();
                selectedCharacter[19].text = gob.speed.ToString();
                selectedCharacter[20].text = gob.stamina.ToString();
                selectedCharacter[21].text = gob.magic.ToString();
                selectedCharacter[22].text = gob.attack.ToString();
                selectedCharacter[23].text = gob.defense.ToString();
                selectedCharacter[24].text = gob.evasion.ToString();
                selectedCharacter[25].text = gob.magicDefense.ToString();
                selectedCharacter[26].text = gob.magicEvasion.ToString();

                EquipableItem thing = null;
                foreach (EquipableItem e in MasterEquipmentContainer.Instance.Equipment)
                {
                    if (gb.Name == e.Name)
                    {
                        thing = e;
                        break;
                    }
                }

                List<Item> MarkedForDeletion = new List<Item>();
                foreach (Item i in ItemList.Instance().Items)
                {
                    if (i.Count <= 0)
                    {
                        MarkedForDeletion.Add(i);
                        continue;
                    }
                }

                foreach (Item i in MarkedForDeletion)
                {
                    ItemList.Instance().Items.Remove(i);
                }

                bool FoundItem = false;
                foreach (Item i in ItemList.Instance().Items)
                {
                    if (i.Name == thing.Name)
                    {
                        FoundItem = true;
                        i.Count += 1;
                        break;
                    }
                }

                if (!FoundItem)
                {
                    thing.Count += 1;
                    ItemList.Instance().Items.Add(thing);

                    foreach (GameObject o in GameObject.FindGameObjectsWithTag("item bar"))
                    {
                        Destroy(o);
                    }

                    Instance.UpdateEquipmentList("Relic");
                }
                else
                {
                    foreach (GameObject o in GameObject.FindGameObjectsWithTag("item bar"))
                    {
                        if (o.name == gb.Name)
                        {
                            o.GetComponentsInChildren<Text>()[1].text = (AddedValue(o.GetComponent<EquipmentBarScript>().Count, 1)).ToString();
                            break;
                        }
                    }
                }
            }

            gob.RightRelicSlot = new EquipableItem { Name = "Empty", Count = 1 };
            Instance.SelectedEquipment = null;
            return;
        }
        else
        {
            selectedCharacter[18].text = (Instance.AddedValue(gob.strength, -gb.STRBonus) / (gb.STRPercentBonus)).ToString();
            selectedCharacter[19].text = (Instance.AddedValue(gob.speed, -gb.SPDBonus) / (gb.SPDPercentBonus)).ToString();
            selectedCharacter[20].text = (Instance.AddedValue(gob.stamina, -gb.STMNABonus) / (gb.STMNAPercentBonus)).ToString();
            selectedCharacter[21].text = (Instance.AddedValue(gob.magic, -gb.MAGBonus) / (gb.MAGPercentBonus)).ToString();
            selectedCharacter[22].text = (Instance.AddedValue(gob.attack, -gb.ATKBonus) / (gb.ATKPercentBonus)).ToString();
            selectedCharacter[23].text = (Instance.AddedValue(gob.defense, -gb.DEFBonus) / (gb.DEFPercentBonus)).ToString();
            selectedCharacter[24].text = (Instance.AddedValue(gob.evasion, -gb.EVSINBonus) / (gb.EVSINPercentBonus)).ToString();
            selectedCharacter[25].text = (Instance.AddedValue(gob.magicDefense, -gb.MagDefBonus) / (gb.MagDefPercentBonus)).ToString();
            selectedCharacter[26].text = (Instance.AddedValue(gob.magicEvasion, -gb.MagEVSINBonus) / (gb.MagEVSINPercentBonus)).ToString();

            Instance.SelectedEquipment = Instance.Relic2Unequip;
        }
    }

    // When you press the left facing arrow on the equipment selection screen
    public void LeftArrowButton()
    {
        int NewSelectedPos = selectedPos - 1;
        int Num = 1;
        bool Done = false;
        while (!Done)
        {
            if (SavedCharacters.Instance().DcurrentStats.ContainsKey(NewSelectedPos))
            {
                OnMemberEquipSelect(NewSelectedPos);
                Done = true;
            }
            else
            {
                if (Num >= 4)
                {
                    OnMemberEquipSelect(selectedPos);
                    Done = true;
                }
                if (NewSelectedPos == selectedPos)
                {
                    OnMemberEquipSelect(selectedPos);
                    Done = true;
                }
                if (NewSelectedPos <= 1)
                {
                    NewSelectedPos = 4;
                }
                else
                {
                    NewSelectedPos -= 1;
                }
                Num += 1;
            }
        }
    }
    // When you press the right facing arrow on equipment selection screen
    public void RightArrowButton()
    {
        int NewSelectedPos = selectedPos + 1;
        int Num = 1;
        bool Done = false;
        while (!Done)
        {
            if (SavedCharacters.Instance().DcurrentStats.ContainsKey(NewSelectedPos))
            {
                OnMemberEquipSelect(NewSelectedPos);
                Done = true;
            }
            else
            {
                if (Num >= 4)
                {
                    OnMemberEquipSelect(selectedPos);
                    Done = true;
                }
                if (NewSelectedPos == selectedPos)
                {
                    OnMemberEquipSelect(selectedPos);
                    Done = true;
                }
                if (NewSelectedPos >= 4)
                {
                    NewSelectedPos = 1;
                }
                else
                {
                    NewSelectedPos += 1;
                }
                Num += 1;
            }
        }
    }

    // Simply adds two ints
    public int AddedValue(decimal Int1, decimal Int2)
    {
        return decimal.ToInt32(Int1 + Int2);
    }

    // Simply adds three ints
    public int AddTriValue(decimal Int1, decimal Int2, decimal Int3)
    {
        return decimal.ToInt32(Int1 + Int2 + Int3);
    }

    // When you exit the character to equip screen
    public void EquipmentSelectionScreenExit()
    {
        equipmentSelectionScreen.SetActive(false);
        selectionScreen.SetActive(false);
        statBlocks.SetActive(true);
        mainMenuScreen.SetActive(true);
    }

    // When you press the save button
    public void SavePressed()
    {
        statBlocks.SetActive(false);
        mainMenuScreen.SetActive(false);
        SaveSelectionScreen.SetActive(true);
    }

    // When you press the back button on the save screen
    public void SaveBack()
    {
        statBlocks.SetActive(true);
        mainMenuScreen.SetActive(true);
        SaveSelectionScreen.SetActive(false);
    }

    // When you select a save file to save to
    public void SaveSelected(string SaveSelected)
    {
        savePointPos = GameObject.Find("Player").transform.position;
        print(GameObject.Find("Player").transform.position);
        SaveTheBooks.SaveGame(SaveSelected);
        savedThisCycle = true;
    }

    // This is called when you select a statblock to equip
    public void OnMemberEquipSelect(int Position)
    {
        mainMenuScreen.SetActive(false);

        int charPos = 0;
        PositionTwoContainer currentChar = null;

        foreach (GameObject c in CharacterStatisticsSerializer.Instance.activePartyMembers)
        {
            if (c.GetComponent<PositionTwoContainer>()._position == Position)
            {
                charPos = c.GetComponent<PositionTwoContainer>()._position - 1;
                currentChar = c.GetComponent<PositionTwoContainer>();

                break;
            }
            else
            {
                Debug.Log("Repeating");
            }
        }
        Instance.selectedCharToEquip = currentChar;

        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("item bar"))
        {
            Destroy(gameObject);
        }

        BodyUnequip.SetActive(false);
        HeadUnequip.SetActive(false);
        Relic1Unequip.SetActive(false);
        Relic2Unequip.SetActive(false);
        LeftHandUnequip.SetActive(false);
        RightHandUnequip.SetActive(false);

        equipmentDescriptionBar.GetComponentsInChildren<Text>()[0].text = "";

        selectedPos = Position;

        selectionScreen.SetActive(true);
        equipScreen.SetActive(false);
        statBlocks.SetActive(false);

        var selectedCharacter = GameObject.Find("Character Statistics").GetComponentsInChildren<Text>();

        selectedCharacter[9].text = currentChar.strength.ToString();
        selectedCharacter[10].text = currentChar.speed.ToString();
        selectedCharacter[11].text = currentChar.stamina.ToString();
        selectedCharacter[12].text = currentChar.magic.ToString();
        selectedCharacter[13].text = currentChar.attack.ToString();
        selectedCharacter[14].text = currentChar.defense.ToString();
        selectedCharacter[15].text = currentChar.evasion.ToString();
        selectedCharacter[16].text = currentChar.magicDefense.ToString();
        selectedCharacter[17].text = currentChar.magicEvasion.ToString();

        GameObject.Find("Helmet").GetComponentsInChildren<Text>()[0].text = currentChar.HeadSlot.Name;
        GameObject.Find("Body").GetComponentsInChildren<Text>()[0].text = currentChar.BodySlot.Name;
        GameObject.Find("Left Relic").GetComponentsInChildren<Text>()[0].text = currentChar.LeftRelicSlot.Name;
        GameObject.Find("Right Relic").GetComponentsInChildren<Text>()[0].text = currentChar.RightRelicSlot.Name;
        GameObject.Find("Left Hand").GetComponentsInChildren<Text>()[0].text = currentChar.LeftHandSlot.Name;
        GameObject.Find("Right Hand").GetComponentsInChildren<Text>()[0].text = currentChar.RightHandSlot.Name;

        int currentSprite = 0;
        foreach (Sprite o in CharacterStatisticsSerializer.Instance.characterPortraits)
        {
            if (CharacterStatisticsSerializer.Instance.characterPortraits[currentSprite].name == currentChar.Name)
            {
                var portrait = GameObject.Find("Character Portrait");
                if (portrait == null)
                {
                    Debug.LogError("portrait was null");
                }

                var portraitSprite = portrait.GetComponent<Image>();
                if (portraitSprite == null)
                {
                    Debug.LogError("component was null");
                }
                portraitSprite.sprite = o;
                break;
            }
            else
            {
                currentSprite += 1;
            }
        }

        selectedCharacter[18].text = 0.ToString();
        selectedCharacter[19].text = 0.ToString();
        selectedCharacter[20].text = 0.ToString();
        selectedCharacter[21].text = 0.ToString();
        selectedCharacter[22].text = 0.ToString();
        selectedCharacter[23].text = 0.ToString();
        selectedCharacter[24].text = 0.ToString();
        selectedCharacter[25].text = 0.ToString();
        selectedCharacter[26].text = 0.ToString();
        //print(thisObject.GetComponent<LoadCharacterStats>().myTiedObject.Name);
        //namePlate.GetComponentInChildren<Text>().text = thisObject.GetComponent<LoadCharacterStats>().myTiedObject.Name;
    }

    // When you exit the actual equipment view screen
    public void OnMemberEquipExit()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("item bar"))
        {
            Destroy(o);
        }
        selectionScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
        statBlocks.SetActive(true);

        HeadUnequip.SetActive(false);
        LeftHandUnequip.SetActive(false);
        RightHandUnequip.SetActive(false);
        BodyUnequip.SetActive(false);
        Relic1Unequip.SetActive(false);
        Relic2Unequip.SetActive(false);
    }

    // This one activates when you press the equip button
    public void OnEquipPress()
    {
        equipScreen.SetActive(true);
        mainMenuScreen.SetActive(false);

        foreach (GameObject s in CharacterStatisticsSerializer.Instance.activePartyMembers)
        {
            equipButtons[s.GetComponent<PositionTwoContainer>()._position - 1].SetActive(true);
        }
        //foreach (Stats s in SavedCharacters.Instance().currentStats)
        //{
        //equipButtons[s._position].SetActive(true);
        //equipButtons[i._position].GetComponent<LoadCharacterStats>().myTiedObject = i;
        //}
    }

    // i think this is when you havent selected a character to equip and go back
    public void OnEquipExit()
    {
        equipScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }
}
