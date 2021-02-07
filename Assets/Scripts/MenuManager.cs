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

    bool wantToAdd;
    [Space]
    public GameObject EquipmentBar;
    public GameObject ItemBar;
    public GameObject Joystick;
    [Space]
    public bool canSave;
    public GameObject selectedItem;
    public Text[] TextArray;
    public GameObject[] itemBars;
    public List<string> itemNames = new List<string>();
    public List<GameObject> equipableItems;
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
        mainMenuButton.SetActive(false);
        mainMenuScreen.SetActive(true);
        menuPanel.SetActive(true);
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
            TextArray[1].GetComponent<Text>().text = i.Count.ToString();
            print(TextArray[1].name);
            go.gameObject.name = i.Name;
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

    // When you click on a piece of equipment while in the equipment screen
    public void EquipmentClick(GameObject thisButton)
    {
        GameObject.Find("Equipment Description Bar").GetComponentsInChildren<Text>()[0].text = thisButton.GetComponent<EquipmentBarScript>().Description;

        var selectedCharacter = GameObject.Find("Character Statistics").GetComponentsInChildren<Text>();
        var goo = thisButton.GetComponent<EquipmentBarScript>();
        var go = SavedCharacters.Instance().DcurrentStats[Instance.selectedPos];
        var gb = SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].BodySlot;
        var glh = SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].LeftHandSlot;
        var grh = SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].RightHandSlot;
        var grr = SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].Relic1Slot;
        var glr = SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].Relic2Slot;
        var gh = SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].HeadSlot;

        if (Instance.SelectedEquipment == thisButton)
        {
            // This is called if the equipment slot is chest
            if (goo.EquipmentType == EquipmentType.Chest)
            {
                if (go.BodySlot.Name != "Empty")
                {
                    go.strength -= Convert.ToInt32(gb.STRBonus);
                    go.speed -= Convert.ToInt32(gb.SPDBonus);
                    go.defense -= Convert.ToInt32(gb.DEFBonus);
                    go.stamina -= Convert.ToInt32(gb.STMNABonus);
                    go.magic -= Convert.ToInt32(gb.MAGBonus);
                    go.attack -= Convert.ToInt32(gb.ATKBonus);
                    go.evasion -= Convert.ToInt32(gb.EVSINBonus);
                    go.magicEvasion -= Convert.ToInt32(gb.MagEVSINBonus);
                    go.magicDefense -= Convert.ToInt32(gb.MagDefBonus);

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
                                go.BodySlot = e;
                                break;
                            }
                        }
                        break;
                    }
                }
                GameObject.Find("Body").GetComponentInChildren<Text>().text = goo.name;

                go.strength = Convert.ToInt32(Math.Floor((go.strength + goo.STRBonus) * goo.STRPercentBonus));
                go.speed = Convert.ToInt32(Math.Floor((go.speed+ goo.SPDBonus) * goo.SPDPercentBonus));
                go.stamina = Convert.ToInt32(Math.Floor((go.stamina + goo.STMNABonus) * goo.STMNAPercentBonus));
                go.magic = Convert.ToInt32(Math.Floor((go.magic+ goo.MAGBonus) * goo.MAGPercentBonus));
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
                if (gb.Name != "Empty")
                {
                    if (gb.Name != goo.name)
                    {
                        Instance.UpdateEquipmentList("Chest");

                        bool Found = false;

                        foreach (Item i in ItemList.Instance().Items)
                        {
                            if (i.Name == gb.Name)
                            {
                                i.Count += 1;
                                Found = true;
                                break;
                            }
                        }

                        if (Found == false)
                        {
                            ItemList.Instance().Items.Add(gb);
                        }
                    }
                    else
                    {
                        foreach (Item i in ItemList.Instance().Items)
                        {
                            if (i.Name == gb.Name)
                            {
                                i.Count += 1;
                            }
                        }
                    }
                }

                Instance.UpdateEquipmentList("Chest");
            }

            // this is called if the equipment slot is Hand
            if (goo.EquipmentType == EquipmentType.Hand)
            {
                if (Instance.LeftHandSelected == true)
                {
                    if (go.LeftHandSlot.Name != "Empty")
                    {
                        go.strength -= glh.STRBonus;
                        go.speed -= glh.SPDBonus;
                        go.defense -= glh.DEFBonus;
                        go.stamina -= glh.STMNABonus;
                        go.magic -= glh.MAGBonus;
                        go.attack -= glh.ATKBonus;
                        go.evasion -= glh.EVSINBonus;
                        go.magicEvasion -= glh.MagEVSINBonus;
                        go.magicDefense -= glh.MagDefBonus;

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
                                    go.LeftHandSlot = e;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    GameObject.Find("Left Hand").GetComponentInChildren<Text>().text = goo.name;

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
                    if (glh.Name != "Empty")
                    {
                        if (glh.Name != goo.name)
                        {
                            Instance.UpdateEquipmentList("Hand");

                            bool Found = false;

                            foreach (Item i in ItemList.Instance().Items)
                            {
                                if (i.Name == glh.Name)
                                {
                                    i.Count += 1;
                                    Found = true;
                                    break;
                                }
                            }

                            if (Found == false)
                            {
                                ItemList.Instance().Items.Add(glh);
                            }
                        }
                        else
                        {
                            foreach (Item i in ItemList.Instance().Items)
                            {
                                if (i.Name == glh.Name)
                                {
                                    i.Count += 1;
                                }
                            }
                        }
                    }

                    Instance.UpdateEquipmentList("Hand");
                }

                if (Instance.RightHandSelected == true)
                {
                    if (go.RightHandSlot.Name != "Empty")
                    {
                        go.strength -= grh.STRBonus;
                        go.speed -= grh.SPDBonus;
                        go.defense -= grh.DEFBonus;
                        go.stamina -= grh.STMNABonus;
                        go.magic -= grh.MAGBonus;
                        go.attack -= grh.ATKBonus;
                        go.evasion -= grh.EVSINBonus;
                        go.magicEvasion -= grh.MagEVSINBonus;
                        go.magicDefense -= grh.MagDefBonus;

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
                                    go.RightHandSlot = e;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    GameObject.Find("Right Hand").GetComponentInChildren<Text>().text = goo.name;

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
                    if (grh.Name != "Empty")
                    {
                        if (grh.Name != goo.name)
                        {
                            Instance.UpdateEquipmentList("Hand");

                            bool Found = false;

                            foreach (Item i in ItemList.Instance().Items)
                            {
                                if (i.Name == grh.Name)
                                {
                                    i.Count += 1;
                                    Found = true;
                                    break;
                                }
                            }

                            if (Found == false)
                            {
                                ItemList.Instance().Items.Add(grh);
                            }
                        }
                        else
                        {
                            foreach (Item i in ItemList.Instance().Items)
                            {
                                if (i.Name == grh.Name)
                                {
                                    i.Count += 1;
                                }
                            }
                        }
                    }

                    Instance.UpdateEquipmentList("Hand");
                }

                Instance.SelectedEquipment = null;
            }

            // This is called if the equipment slot is Relic
            if(goo.EquipmentType == EquipmentType.Relic)
            {
                if (Instance.LeftRelicSelected == true)
                {
                    if (go.Relic1Slot.Name != "Empty")
                    {
                        go.strength -= glr.STRBonus;
                        go.speed -= glr.SPDBonus;
                        go.defense -= glr.DEFBonus;
                        go.stamina -= glr.STMNABonus;
                        go.magic -= glr.MAGBonus;
                        go.attack -= glr.ATKBonus;
                        go.evasion -= glr.EVSINBonus;
                        go.magicEvasion -= glr.MagEVSINBonus;
                        go.magicDefense -= glr.MagDefBonus;

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
                                    go.Relic1Slot = e;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    GameObject.Find("Relic 1").GetComponentInChildren<Text>().text = goo.name;

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
                    if (glr.Name != "Empty")
                    {
                        if (glr.Name != goo.name)
                        {
                            Instance.UpdateEquipmentList("Relic");

                            bool Found = false;

                            foreach (Item i in ItemList.Instance().Items)
                            {
                                if (i.Name == glr.Name)
                                {
                                    i.Count += 1;
                                    Found = true;
                                    break;
                                }
                            }

                            if (Found == false)
                            {
                                ItemList.Instance().Items.Add(glr);
                            }
                        }
                        else
                        {
                            foreach (Item i in ItemList.Instance().Items)
                            {
                                if (i.Name == glr.Name)
                                {
                                    i.Count += 1;
                                }
                            }
                        }
                    }

                    Instance.UpdateEquipmentList("Relic");
                }

                if (Instance.RightRelicSelected == true)
                {
                    if (go.Relic2Slot.Name != "Empty")
                    {
                        go.strength -= grr.STRBonus;
                        go.speed -= grr.SPDBonus;
                        go.defense -= grr.DEFBonus;
                        go.stamina -= grr.STMNABonus;
                        go.magic -= grr.MAGBonus;
                        go.attack -= grr.ATKBonus;
                        go.evasion -= grr.EVSINBonus;
                        go.magicEvasion -= grr.MagEVSINBonus;
                        go.magicDefense -= grr.MagDefBonus;

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
                                    go.Relic2Slot = e;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    GameObject.Find("Relic 2").GetComponentInChildren<Text>().text = goo.name;

                    go.strength += goo.STRBonus;
                    go.speed += goo.SPDBonus;
                    go.defense += goo.DEFBonus;
                    go.stamina += goo.STMNABonus;
                    go.magic += goo.MAGBonus;
                    go.attack += goo.ATKBonus;
                    go.evasion += goo.EVSINBonus;
                    go.magicEvasion += goo.MagEVSINBonus;
                    go.magicDefense += goo.MagDefBonus;

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
                    if (grr.Name != "Empty")
                    {
                        if (grr.Name != goo.name)
                        {
                            Instance.UpdateEquipmentList("Relic");

                            bool Found = false;

                            foreach (Item i in ItemList.Instance().Items)
                            {
                                if (i.Name == grr.Name)
                                {
                                    i.Count += 1;
                                    Found = true;
                                    break;
                                }
                            }

                            if (Found == false)
                            {
                                ItemList.Instance().Items.Add(grr);
                            }
                        }
                        else
                        {
                            foreach (Item i in ItemList.Instance().Items)
                            {
                                if (i.Name == grr.Name)
                                {
                                    i.Count += 1;
                                }
                            }
                        }
                    }

                    Instance.UpdateEquipmentList("Relic");
                }
            }

            //this is called if the equipment slot is Head
            if (goo.EquipmentType == EquipmentType.Head)
            {
                if (go.HeadSlot.Name != "Empty")
                {
                    go.strength -= gh.STRBonus;
                    go.speed -= gh.SPDBonus;
                    go.defense -= gh.DEFBonus;
                    go.stamina -= gh.STMNABonus;
                    go.magic -= gh.MAGBonus;
                    go.attack -= gh.ATKBonus;
                    go.evasion -= gh.EVSINBonus;
                    go.magicEvasion -= gh.MagEVSINBonus;
                    go.magicDefense -= gh.MagDefBonus;

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
                                go.HeadSlot = e;
                                break;
                            }
                        }
                        break;
                    }
                }
                GameObject.Find("Helmet").GetComponentInChildren<Text>().text = goo.name;

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
                if (gh.Name != "Empty")
                {
                    if (gh.Name != goo.name)
                    {
                        Instance.UpdateEquipmentList("Head");

                        bool Found = false;

                        foreach (Item i in ItemList.Instance().Items)
                        {
                            if (i.Name == gh.Name)
                            {
                                i.Count += 1;
                                Found = true;
                                break;
                            }
                        }

                        if (Found == false)
                        {
                            ItemList.Instance().Items.Add(gh);
                        }
                    }
                    else
                    {
                        foreach (Item i in ItemList.Instance().Items)
                        {
                            if (i.Name == gh.Name)
                            {
                                i.Count += 1;
                            }
                        }
                    }
                }

                Instance.UpdateEquipmentList("Head");
            }
        }
        else
        {
            if (goo.EquipmentType == EquipmentType.Chest)
            {
                if (go.BodySlot.Name == "Empty")
                {
                    selectedCharacter[18].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].strength, goo.STRBonus) * goo.STRPercentBonus).ToString();
                    selectedCharacter[19].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].speed, goo.SPDBonus) * goo.SPDPercentBonus).ToString();
                    selectedCharacter[20].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].stamina, goo.STMNABonus) * goo.STMNAPercentBonus).ToString();
                    selectedCharacter[21].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].magic, goo.MAGBonus) * goo.MAGPercentBonus).ToString();
                    selectedCharacter[22].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].attack, goo.ATKBonus) * goo.ATKPercentBonus).ToString();
                    selectedCharacter[23].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].defense, goo.DEFBonus) * goo.DEFPercentBonus).ToString();
                    selectedCharacter[24].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].evasion, goo.EVSINBonus) * goo.EVSINPercentBonus).ToString();
                    selectedCharacter[25].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].magicDefense, goo.MagDefBonus) * goo.MagDefPercentBonus).ToString();
                    selectedCharacter[26].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].magicEvasion, goo.MagEVSINBonus) * goo.MagEVSINPercentBonus).ToString();
                }
                else
                {
                    selectedCharacter[18].text = (Instance.AddTriValue(go.strength, goo.STRBonus, -gb.STRBonus) * (-gb.STRPercentBonus)).ToString();
                    selectedCharacter[18].text = (Instance.AddTriValue(go.speed, goo.SPDBonus, -gb.SPDBonus) * (-gb.SPDPercentBonus)).ToString();
                    selectedCharacter[18].text = (Instance.AddTriValue(go.stamina, goo.STMNABonus, -gb.STMNABonus) * (-gb.STMNAPercentBonus)).ToString();
                    selectedCharacter[18].text = (Instance.AddTriValue(go.magic, goo.MAGBonus, -gb.MAGBonus) * (-gb.MAGPercentBonus)).ToString();
                    selectedCharacter[18].text = (Instance.AddTriValue(go.attack, goo.ATKBonus, -gb.ATKBonus) * (-gb.ATKPercentBonus)).ToString();
                    selectedCharacter[18].text = (Instance.AddTriValue(go.defense, goo.DEFBonus, -gb.DEFBonus) * (-gb.DEFPercentBonus)).ToString();
                    selectedCharacter[18].text = (Instance.AddTriValue(go.evasion, goo.EVSINBonus, -gb.EVSINBonus) * (-gb.EVSINPercentBonus)).ToString();
                    selectedCharacter[18].text = (Instance.AddTriValue(go.magicDefense, goo.MagDefBonus, -gb.MagDefBonus) * (-gb.MagDefPercentBonus)).ToString();
                    selectedCharacter[18].text = (Instance.AddTriValue(go.magicEvasion, goo.MagEVSINBonus, -gb.MagEVSINBonus) * (-gb.MagEVSINPercentBonus)).ToString();
                }
            }

            if (goo.EquipmentType == EquipmentType.Hand)
            {
                if (Instance.LeftHandSelected)
                {
                    if (go.LeftHandSlot.Name == "Empty")
                    {
                        selectedCharacter[18].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].strength, goo.STRBonus) * goo.STRPercentBonus).ToString();
                        selectedCharacter[19].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].speed, goo.SPDBonus) * goo.SPDPercentBonus).ToString();
                        selectedCharacter[20].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].stamina, goo.STMNABonus) * goo.STMNAPercentBonus).ToString();
                        selectedCharacter[21].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].magic, goo.MAGBonus) * goo.MAGPercentBonus).ToString();
                        selectedCharacter[22].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].attack, goo.ATKBonus) * goo.ATKPercentBonus).ToString();
                        selectedCharacter[23].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].defense, goo.DEFBonus) * goo.DEFPercentBonus).ToString();
                        selectedCharacter[24].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].evasion, goo.EVSINBonus) * goo.EVSINPercentBonus).ToString();
                        selectedCharacter[25].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].magicDefense, goo.MagDefBonus) * goo.MagDefPercentBonus).ToString();
                        selectedCharacter[26].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].magicEvasion, goo.MagEVSINBonus) * goo.MagEVSINPercentBonus).ToString();
                    }
                    else
                    {
                        selectedCharacter[18].text = (Instance.AddTriValue(go.strength, goo.STRBonus, -glh.STRBonus) * (-glh.STRPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.speed, goo.SPDBonus, -glh.SPDBonus) * (-glh.SPDPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.stamina, goo.STMNABonus, -glh.STMNABonus) * (-glh.STMNAPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.magic, goo.MAGBonus, -glh.MAGBonus) * (-glh.MAGPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.attack, goo.ATKBonus, -glh.ATKBonus) * (-glh.ATKPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.defense, goo.DEFBonus, -glh.DEFBonus) * (-glh.DEFPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.evasion, goo.EVSINBonus, -glh.EVSINBonus) * (-glh.EVSINPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.magicDefense, goo.MagDefBonus, -glh.MagDefBonus) * (-glh.MagDefPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.magicEvasion, goo.MagEVSINBonus, -glh.MagEVSINBonus) * (-glh.MagEVSINPercentBonus)).ToString();
                    }
                }

                if (Instance.RightHandSelected)
                {
                    if (go.RightHandSlot.Name == "Empty")
                    {
                        selectedCharacter[18].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].strength, goo.STRBonus) * goo.STRPercentBonus).ToString();
                        selectedCharacter[19].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].speed, goo.SPDBonus) * goo.SPDPercentBonus).ToString();
                        selectedCharacter[20].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].stamina, goo.STMNABonus) * goo.STMNAPercentBonus).ToString();
                        selectedCharacter[21].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].magic, goo.MAGBonus) * goo.MAGPercentBonus).ToString();
                        selectedCharacter[22].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].attack, goo.ATKBonus) * goo.ATKPercentBonus).ToString();
                        selectedCharacter[23].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].defense, goo.DEFBonus) * goo.DEFPercentBonus).ToString();
                        selectedCharacter[24].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].evasion, goo.EVSINBonus) * goo.EVSINPercentBonus).ToString();
                        selectedCharacter[25].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].magicDefense, goo.MagDefBonus) * goo.MagDefPercentBonus).ToString();
                        selectedCharacter[26].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].magicEvasion, goo.MagEVSINBonus) * goo.MagEVSINPercentBonus).ToString();
                    }
                    else
                    {
                        selectedCharacter[18].text = (Instance.AddTriValue(go.strength, goo.STRBonus, -grh.STRBonus) * (-grh.STRPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.speed, goo.SPDBonus, -grh.SPDBonus) * (-grh.SPDPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.stamina, goo.STMNABonus, -grh.STMNABonus) * (-grh.STMNAPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.magic, goo.MAGBonus, -grh.MAGBonus) * (-grh.MAGPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.attack, goo.ATKBonus, -grh.ATKBonus) * (-grh.ATKPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.defense, goo.DEFBonus, -grh.DEFBonus) * (-grh.DEFPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.evasion, goo.EVSINBonus, -grh.EVSINBonus) * (-grh.EVSINPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.magicDefense, goo.MagDefBonus, -grh.MagDefBonus) * (-grh.MagDefPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.magicEvasion, goo.MagEVSINBonus, -grh.MagEVSINBonus) * (-grh.MagEVSINPercentBonus)).ToString();
                    }
                }
            }

            if (goo.EquipmentType == EquipmentType.Head)
            {
                if (go.HeadSlot.Name == "Empty")
                {
                    selectedCharacter[18].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].strength, goo.STRBonus) * goo.STRPercentBonus).ToString();
                    selectedCharacter[19].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].speed, goo.SPDBonus) * goo.SPDPercentBonus).ToString();
                    selectedCharacter[20].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].stamina, goo.STMNABonus) * goo.STMNAPercentBonus).ToString();
                    selectedCharacter[21].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].magic, goo.MAGBonus) * goo.MAGPercentBonus).ToString();
                    selectedCharacter[22].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].attack, goo.ATKBonus) * goo.ATKPercentBonus).ToString();
                    selectedCharacter[23].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].defense, goo.DEFBonus) * goo.DEFPercentBonus).ToString();
                    selectedCharacter[24].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].evasion, goo.EVSINBonus) * goo.EVSINPercentBonus).ToString();
                    selectedCharacter[25].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].magicDefense, goo.MagDefBonus) * goo.MagDefPercentBonus).ToString();
                    selectedCharacter[26].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].magicEvasion, goo.MagEVSINBonus) * goo.MagEVSINPercentBonus).ToString();
                }
                else
                {
                    selectedCharacter[18].text = (Instance.AddTriValue(go.strength, goo.STRBonus, -gh.STRBonus) * (-gh.STRPercentBonus)).ToString();
                    selectedCharacter[18].text = (Instance.AddTriValue(go.speed, goo.SPDBonus, -gh.SPDBonus) * (-gh.SPDPercentBonus)).ToString();
                    selectedCharacter[18].text = (Instance.AddTriValue(go.stamina, goo.STMNABonus, -gh.STMNABonus) * (-gh.STMNAPercentBonus)).ToString();
                    selectedCharacter[18].text = (Instance.AddTriValue(go.magic, goo.MAGBonus, -gh.MAGBonus) * (-gh.MAGPercentBonus)).ToString();
                    selectedCharacter[18].text = (Instance.AddTriValue(go.attack, goo.ATKBonus, -gh.ATKBonus) * (-gh.ATKPercentBonus)).ToString();
                    selectedCharacter[18].text = (Instance.AddTriValue(go.defense, goo.DEFBonus, -gh.DEFBonus) * (-gh.DEFPercentBonus)).ToString();
                    selectedCharacter[18].text = (Instance.AddTriValue(go.evasion, goo.EVSINBonus, -gh.EVSINBonus) * (-gh.EVSINPercentBonus)).ToString();
                    selectedCharacter[18].text = (Instance.AddTriValue(go.magicDefense, goo.MagDefBonus, -gh.MagDefBonus) * (-gh.MagDefPercentBonus)).ToString();
                    selectedCharacter[18].text = (Instance.AddTriValue(go.magicEvasion, goo.MagEVSINBonus, -gh.MagEVSINBonus) * (-gh.MagEVSINPercentBonus)).ToString();
                }
            }

            if (goo.EquipmentType == EquipmentType.Relic)
            {
                if (Instance.LeftRelicSelected)
                {
                    if (go.Relic1Slot.Name == "Empty")
                    {
                        selectedCharacter[18].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].strength, goo.STRBonus) * goo.STRPercentBonus).ToString();
                        selectedCharacter[19].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].speed, goo.SPDBonus) * goo.SPDPercentBonus).ToString();
                        selectedCharacter[20].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].stamina, goo.STMNABonus) * goo.STMNAPercentBonus).ToString();
                        selectedCharacter[21].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].magic, goo.MAGBonus) * goo.MAGPercentBonus).ToString();
                        selectedCharacter[22].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].attack, goo.ATKBonus) * goo.ATKPercentBonus).ToString();
                        selectedCharacter[23].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].defense, goo.DEFBonus) * goo.DEFPercentBonus).ToString();
                        selectedCharacter[24].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].evasion, goo.EVSINBonus) * goo.EVSINPercentBonus).ToString();
                        selectedCharacter[25].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].magicDefense, goo.MagDefBonus) * goo.MagDefPercentBonus).ToString();
                        selectedCharacter[26].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].magicEvasion, goo.MagEVSINBonus) * goo.MagEVSINPercentBonus).ToString();
                    }
                    else
                    {
                        selectedCharacter[18].text = (Instance.AddTriValue(go.strength, goo.STRBonus, -glr.STRBonus) * (-glr.STRPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.speed, goo.SPDBonus, -glr.SPDBonus) * (-glr.SPDPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.stamina, goo.STMNABonus, -glr.STMNABonus) * (-glr.STMNAPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.magic, goo.MAGBonus, -glr.MAGBonus) * (-glr.MAGPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.attack, goo.ATKBonus, -glr.ATKBonus) * (-glr.ATKPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.defense, goo.DEFBonus, -glr.DEFBonus) * (-glr.DEFPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.evasion, goo.EVSINBonus, -glr.EVSINBonus) * (-glr.EVSINPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.magicDefense, goo.MagDefBonus, -glr.MagDefBonus) * (-glr.MagDefPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.magicEvasion, goo.MagEVSINBonus, -glr.MagEVSINBonus) * (-glr.MagEVSINPercentBonus)).ToString();
                    }
                }

                if (Instance.RightRelicSelected)
                {
                    if (go.Relic2Slot.Name == "Empty")
                    {
                        selectedCharacter[18].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].strength, goo.STRBonus)*goo.STRPercentBonus).ToString();
                        selectedCharacter[19].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].speed, goo.SPDBonus)*goo.SPDPercentBonus).ToString();
                        selectedCharacter[20].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].stamina, goo.STMNABonus)*goo.STMNAPercentBonus).ToString();
                        selectedCharacter[21].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].magic, goo.MAGBonus)*goo.MAGPercentBonus).ToString();
                        selectedCharacter[22].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].attack, goo.ATKBonus)*goo.ATKPercentBonus).ToString();
                        selectedCharacter[23].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].defense, goo.DEFBonus)*goo.DEFPercentBonus).ToString();
                        selectedCharacter[24].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].evasion, goo.EVSINBonus)*goo.EVSINPercentBonus).ToString();
                        selectedCharacter[25].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].magicDefense, goo.MagDefBonus)*goo.MagDefPercentBonus).ToString();
                        selectedCharacter[26].text = (Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].magicEvasion, goo.MagEVSINBonus)*goo.MagEVSINPercentBonus).ToString();
                    }
                    else
                    {
                        selectedCharacter[18].text = (Instance.AddTriValue(go.strength, goo.STRBonus, -grr.STRBonus) * (-grr.STRPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.speed, goo.SPDBonus, -grr.SPDBonus) * (-grr.SPDPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.stamina, goo.STMNABonus, -grr.STMNABonus) * (-grr.STMNAPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.magic, goo.MAGBonus, -grr.MAGBonus) * (-grr.MAGPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.attack, goo.ATKBonus, -grr.ATKBonus) * (-grr.ATKPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.defense, goo.DEFBonus, -grr.DEFBonus) * (-grr.DEFPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.evasion, goo.EVSINBonus, -grr.EVSINBonus) * (-grr.EVSINPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.magicDefense, goo.MagDefBonus, -grr.MagDefBonus) * (-grr.MagDefPercentBonus)).ToString();
                        selectedCharacter[18].text = (Instance.AddTriValue(go.magicEvasion, goo.MagEVSINBonus, -grr.MagEVSINBonus) * (-grr.MagEVSINPercentBonus)).ToString();
                    }
                }
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
                    if (s == SavedCharacters.Instance().DcurrentStats[selectedPos].characterName && Place == x.EquipmentType.ToString())
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

        foreach(GameObject g in GameObject.FindGameObjectsWithTag("item bar"))
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
                    if (s == SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].characterName && Place == x.EquipmentType.ToString())
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
        var gob = SavedCharacters.Instance().DcurrentStats[Instance.selectedPos];
        var gb = SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].LeftHandSlot;
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

            SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].LeftHandSlot = new EquipableItem { Name = "Empty", Count = 1 };
            Instance.SelectedEquipment = null;
            return;
        }
        else
        {
            selectedCharacter[18].text = Instance.AddedValue(gob.strength, -gb.STRBonus).ToString();
            selectedCharacter[19].text = Instance.AddedValue(gob.speed, -gb.SPDBonus).ToString();
            selectedCharacter[20].text = Instance.AddedValue(gob.stamina, -gb.STMNABonus).ToString();
            selectedCharacter[21].text = Instance.AddedValue(gob.magic, -gb.MAGBonus).ToString();
            selectedCharacter[22].text = Instance.AddedValue(gob.attack, -gb.ATKBonus).ToString();
            selectedCharacter[23].text = Instance.AddedValue(gob.defense, -gb.DEFBonus).ToString();
            selectedCharacter[24].text = Instance.AddedValue(gob.evasion, -gb.EVSINBonus).ToString();
            selectedCharacter[25].text = Instance.AddedValue(gob.magicDefense, -gb.MagDefBonus).ToString();
            selectedCharacter[26].text = Instance.AddedValue(gob.magicEvasion, -gb.MagEVSINBonus).ToString();

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
        var go = SavedCharacters.Instance().DcurrentStats[Instance.selectedPos];
        var gb = SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].BodySlot;
        var selectedCharacter = GameObject.Find("Character Statistics").GetComponentsInChildren<Text>();

        if (Instance.SelectedEquipment == Instance.BodyUnequip)
        {
            GameObject.Find("Body").GetComponentsInChildren<Text>()[0].text = "Empty";

            go.strength -= gb.STRBonus;
            go.speed -= gb.SPDBonus;
            go.defense -= gb.DEFBonus;
            go.stamina -= gb.STMNABonus;
            go.magic -= gb.MAGBonus;
            go.attack -= gb.ATKBonus;
            go.evasion -= gb.EVSINBonus;
            go.magicEvasion -= gb.MagEVSINBonus;
            go.magicDefense -= gb.MagDefBonus;

            selectedCharacter[9].text = go.strength.ToString();
            selectedCharacter[10].text = go.speed.ToString();
            selectedCharacter[11].text = go.stamina.ToString();
            selectedCharacter[12].text = go.magic.ToString();
            selectedCharacter[13].text = go.attack.ToString();
            selectedCharacter[14].text = go.defense.ToString();
            selectedCharacter[15].text = go.evasion.ToString();
            selectedCharacter[16].text = go.magicDefense.ToString();
            selectedCharacter[17].text = go.magicEvasion.ToString();

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

            SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].BodySlot = new EquipableItem { Name = "Empty", Count = 1 };
            Instance.SelectedEquipment = null;
            UpdateEquipmentList("Chest");

            return;
        }
        else
        {
            selectedCharacter[18].text = Instance.AddedValue(go.strength, -gb.STRBonus).ToString();
            selectedCharacter[19].text = Instance.AddedValue(go.speed, -gb.SPDBonus).ToString();
            selectedCharacter[20].text = Instance.AddedValue(go.stamina, -gb.STMNABonus).ToString();
            selectedCharacter[21].text = Instance.AddedValue(go.magic, -gb.MAGBonus).ToString();
            selectedCharacter[22].text = Instance.AddedValue(go.attack, -gb.ATKBonus).ToString();
            selectedCharacter[23].text = Instance.AddedValue(go.defense, -gb.DEFBonus).ToString();
            selectedCharacter[24].text = Instance.AddedValue(go.evasion, -gb.EVSINBonus).ToString();
            selectedCharacter[25].text = Instance.AddedValue(go.magicDefense, -gb.MagDefBonus).ToString();
            selectedCharacter[26].text = Instance.AddedValue(go.magicEvasion, -gb.MagEVSINBonus).ToString();

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
        var gob = SavedCharacters.Instance().DcurrentStats[Instance.selectedPos];
        var gb = SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].HeadSlot;
        var selectedCharacter = GameObject.Find("Character Statistics").GetComponentsInChildren<Text>();

        if (Instance.SelectedEquipment == Instance.HeadUnequip)
        {
            GameObject.Find("Helmet").GetComponentsInChildren<Text>()[0].text = "Empty";

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

            SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].HeadSlot = new EquipableItem { Name = "Empty", Count = 1 };
            Instance.SelectedEquipment = null;
            UpdateEquipmentList("Head");

            return;
        }
        else
        {
            selectedCharacter[18].text = Instance.AddedValue(gob.strength, -gb.STRBonus).ToString();
            selectedCharacter[19].text = Instance.AddedValue(gob.speed, -gb.SPDBonus).ToString();
            selectedCharacter[20].text = Instance.AddedValue(gob.stamina, -gb.STMNABonus).ToString();
            selectedCharacter[21].text = Instance.AddedValue(gob.magic, -gb.MAGBonus).ToString();
            selectedCharacter[22].text = Instance.AddedValue(gob.attack, -gb.ATKBonus).ToString();
            selectedCharacter[23].text = Instance.AddedValue(gob.defense, -gb.DEFBonus).ToString();
            selectedCharacter[24].text = Instance.AddedValue(gob.evasion, -gb.EVSINBonus).ToString();
            selectedCharacter[25].text = Instance.AddedValue(gob.magicDefense, -gb.MagDefBonus).ToString();
            selectedCharacter[26].text = Instance.AddedValue(gob.magicEvasion, -gb.MagEVSINBonus).ToString();

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
        var gob = SavedCharacters.Instance().DcurrentStats[Instance.selectedPos];
        var gb = SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].RightHandSlot;
        var selectedCharacter = GameObject.Find("Character Statistics").GetComponentsInChildren<Text>();

        if (Instance.SelectedEquipment == Instance.RightHandUnequip)
        {
            GameObject.Find("Right Hand").GetComponentsInChildren<Text>()[0].text = "Empty";

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

            SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].RightHandSlot = new EquipableItem { Name = "Empty", Count = 1 };
            Instance.SelectedEquipment = null;
            return;
        }
        else
        {
            selectedCharacter[18].text = Instance.AddedValue(gob.strength, -gb.STRBonus).ToString();
            selectedCharacter[19].text = Instance.AddedValue(gob.speed, -gb.SPDBonus).ToString();
            selectedCharacter[20].text = Instance.AddedValue(gob.stamina, -gb.STMNABonus).ToString();
            selectedCharacter[21].text = Instance.AddedValue(gob.magic, -gb.MAGBonus).ToString();
            selectedCharacter[22].text = Instance.AddedValue(gob.attack, -gb.ATKBonus).ToString();
            selectedCharacter[23].text = Instance.AddedValue(gob.defense, -gb.DEFBonus).ToString();
            selectedCharacter[24].text = Instance.AddedValue(gob.evasion, -gb.EVSINBonus).ToString();
            selectedCharacter[25].text = Instance.AddedValue(gob.magicDefense, -gb.MagDefBonus).ToString();
            selectedCharacter[26].text = Instance.AddedValue(gob.magicEvasion, -gb.MagEVSINBonus).ToString();

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
        var gob = SavedCharacters.Instance().DcurrentStats[Instance.selectedPos];
        var gb = SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].Relic1Slot;
        var selectedCharacter = GameObject.Find("Character Statistics").GetComponentsInChildren<Text>();

        if (Instance.SelectedEquipment == Instance.Relic1Unequip)
        {
            GameObject.Find("Relic 1").GetComponentsInChildren<Text>()[0].text = "Empty";

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

            SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].Relic1Slot = new EquipableItem { Name = "Empty", Count = 1 };
            Instance.SelectedEquipment = null;
            return;
        }
        else
        {
            selectedCharacter[18].text = Instance.AddedValue(gob.strength, -gb.STRBonus).ToString();
            selectedCharacter[19].text = Instance.AddedValue(gob.speed, -gb.SPDBonus).ToString();
            selectedCharacter[20].text = Instance.AddedValue(gob.stamina, -gb.STMNABonus).ToString();
            selectedCharacter[21].text = Instance.AddedValue(gob.magic, -gb.MAGBonus).ToString();
            selectedCharacter[22].text = Instance.AddedValue(gob.attack, -gb.ATKBonus).ToString();
            selectedCharacter[23].text = Instance.AddedValue(gob.defense, -gb.DEFBonus).ToString();
            selectedCharacter[24].text = Instance.AddedValue(gob.evasion, -gb.EVSINBonus).ToString();
            selectedCharacter[25].text = Instance.AddedValue(gob.magicDefense, -gb.MagDefBonus).ToString();
            selectedCharacter[26].text = Instance.AddedValue(gob.magicEvasion, -gb.MagEVSINBonus).ToString();

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
        var gob = SavedCharacters.Instance().DcurrentStats[Instance.selectedPos];
        var gb = SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].Relic2Slot;
        var selectedCharacter = GameObject.Find("Character Statistics").GetComponentsInChildren<Text>();

        if (Instance.SelectedEquipment == Instance.Relic2Unequip)
        {
            GameObject.Find("Relic 2").GetComponentsInChildren<Text>()[0].text = "Empty";

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

            SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].Relic2Slot = new EquipableItem { Name = "Empty", Count = 1 };
            Instance.SelectedEquipment = null;
            return;
        }
        else
        {
            selectedCharacter[18].text = Instance.AddedValue(gob.strength, -gb.STRBonus).ToString();
            selectedCharacter[19].text = Instance.AddedValue(gob.speed, -gb.SPDBonus).ToString();
            selectedCharacter[20].text = Instance.AddedValue(gob.stamina, -gb.STMNABonus).ToString();
            selectedCharacter[21].text = Instance.AddedValue(gob.magic, -gb.MAGBonus).ToString();
            selectedCharacter[22].text = Instance.AddedValue(gob.attack, -gb.ATKBonus).ToString();
            selectedCharacter[23].text = Instance.AddedValue(gob.defense, -gb.DEFBonus).ToString();
            selectedCharacter[24].text = Instance.AddedValue(gob.evasion, -gb.EVSINBonus).ToString();
            selectedCharacter[25].text = Instance.AddedValue(gob.magicDefense, -gb.MagDefBonus).ToString();
            selectedCharacter[26].text = Instance.AddedValue(gob.magicEvasion, -gb.MagEVSINBonus).ToString();

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
                if(Num >= 4)
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
        while (!Done) {
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
        SaveTheBooks.SaveGame(SaveSelected);
    }

    // This is called when you select a statblock to equip
    public void OnMemberEquipSelect(int Position)
    {
        foreach(GameObject gameObject in GameObject.FindGameObjectsWithTag("item bar"))
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

        selectedCharacter[9].text = SavedCharacters.Instance().DcurrentStats[Position].strength.ToString();
        selectedCharacter[10].text = SavedCharacters.Instance().DcurrentStats[Position].speed.ToString();
        selectedCharacter[11].text = SavedCharacters.Instance().DcurrentStats[Position].stamina.ToString();
        selectedCharacter[12].text = SavedCharacters.Instance().DcurrentStats[Position].magic.ToString();
        selectedCharacter[13].text = SavedCharacters.Instance().DcurrentStats[Position].attack.ToString();
        selectedCharacter[14].text = SavedCharacters.Instance().DcurrentStats[Position].defense.ToString();
        selectedCharacter[15].text = SavedCharacters.Instance().DcurrentStats[Position].evasion.ToString();
        selectedCharacter[16].text = SavedCharacters.Instance().DcurrentStats[Position].magicDefense.ToString();
        selectedCharacter[17].text = SavedCharacters.Instance().DcurrentStats[Position].magicEvasion.ToString();

        var sv = SavedCharacters.Instance().DcurrentStats[Position];

        GameObject.Find("Helmet").GetComponentsInChildren<Text>()[0].text = sv.HeadSlot.Name;
        GameObject.Find("Body").GetComponentsInChildren<Text>()[0].text = sv.BodySlot.Name;
        GameObject.Find("Relic 1").GetComponentsInChildren<Text>()[0].text = sv.Relic1Slot.Name;
        GameObject.Find("Relic 2").GetComponentsInChildren<Text>()[0].text = sv.Relic2Slot.Name;
        GameObject.Find("Left Hand").GetComponentsInChildren<Text>()[0].text = sv.LeftHandSlot.Name;
        GameObject.Find("Right Hand").GetComponentsInChildren<Text>()[0].text = sv.RightHandSlot.Name;

        int currentSprite = 0;
        foreach (Sprite o in CharacterStatisticsSerializer.Instance.characterPortraits)
        {
            if (CharacterStatisticsSerializer.Instance.characterPortraits[currentSprite].name == SavedCharacters.Instance().DcurrentStats[Position].characterName)
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
        
        foreach (Stats s in SavedCharacters.Instance().currentStats)
        {
            equipButtons[s._position].SetActive(true);
            //equipButtons[i._position].GetComponent<LoadCharacterStats>().myTiedObject = i;
        }
    }

    // i think this is when you havent selected a character to equip and go back
    public void OnEquipExit()
    {
        equipScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }
}
