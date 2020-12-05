using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

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
    [Space]
    public GameObject selectionScreen;
    [Space]
    public GameObject SaveSelectionScreen;

    int otherNumber;
    int number;
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
    public Text[] thing;
    public GameObject[] itemBars;
    public List<string> itemNames = new List<string>();

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
            thing = go.GetComponentsInChildren<Text>();

            statBlocks.SetActive(false);

            thing[0].text = i.Name;
            thing[1].GetComponent<Text>().text = i.Count.ToString();
            print(thing[1].name);
            go.gameObject.name = i.Name;
            print(i.Name);
            print("There are this many "+i.Name+": "+i.Count);

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

    // This is when you select a place to put the equipment
    public void EquipmentSelectionScreenPress(string Place)
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("item bar"))
        {
            Destroy(o);
        }

        equipmentSelectionScreen.SetActive(true);
        statBlocks.SetActive(false);
        equipScreen.SetActive(false);

        int Num = 0;

        int VectorX = 840;
        int VectorY = 582;

        var t = ItemList.Instance().Items
            .Where(x => x.ItemType == ItemType.EquipableItem)
            .Select(x => MasterEquipmentContainer.Instance.Equipment.First(e => e.Name == x.Name))
            .Select(x =>
            {
                GameObject go = Instantiate(EquipmentBar, new Vector3(VectorX, VectorY, 0), Quaternion.identity) as GameObject;
                go.transform.parent = GameObject.Find("Equipment Selection").transform;

                bool delete = true;
                foreach (string s in x.canEquip)
                {
                    print(s);
                    if (s == SavedCharacters.Instance().DcurrentStats[selectedPos].characterName && Place == x.EquipmentType.ToString())
                    {
                        thing = go.GetComponentsInChildren<Text>();

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
                        goo.MagEVSINPercentBonus= x.MagEVSINPercentBonus;
                        goo.MagDefPercentBonus = x.MagDefPercentBonus;

                        goo.canEquip.Clear();
                        goo.canEquip.AddRange(x.canEquip);
                        goo.EquipmentType = x.EquipmentType;
                        foreach(Item i in ItemList.Instance().Items)
                        
                        {
                            if(i.Name == x.Name)
                            {
                                goo.Count = i.Count;
                            }
                        }
                        goo.Description = x.Description;
                        go.name = x.Name;
                        go.GetComponentsInChildren<Text>()[0].text = x.Name;
                        go.GetComponentsInChildren<Text>()[1].text = goo.Count.ToString();
                        delete = false;

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

    }

    // When you click on a piece of equipment while in the equipment screen
    public void EquipmentClick(GameObject thisButton)
    {
        GameObject.Find("Equipment Description Bar").GetComponentsInChildren<Text>()[0].text = thisButton.GetComponent<EquipmentBarScript>().Description;

        var goo = thisButton.GetComponent<EquipmentBarScript>();

        if (Instance.SelectedEquipment == thisButton)
        {
            SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].defense += goo.DEFBonus;
        }
        else
        {
            var selectedCharacter = GameObject.Find("Character Statistics").GetComponentsInChildren<Text>();

            selectedCharacter[23].text = Instance.AddedValue(SavedCharacters.Instance().DcurrentStats[Instance.selectedPos].defense, goo.DEFBonus).ToString();
        }
        Instance.SelectedEquipment = thisButton;
    }

    public int AddedValue(int Int1, int Int2)
    {
        return (Int1 + Int2);
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
    public void onMemberEquipSelect(int Position)
    {
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
        //print(thisObject.GetComponent<LoadCharacterStats>().myTiedObject.Name);
        //namePlate.GetComponentInChildren<Text>().text = thisObject.GetComponent<LoadCharacterStats>().myTiedObject.Name;
    }

    // When you exit the actual equipment view screen
    public void onMemberEquipExit()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("item bar"))
        {
            Destroy(o);
        }
        selectionScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
        statBlocks.SetActive(true);
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

    //public void onHandClick()
    //{
    //    foreach (GameObject o in GameObject.FindGameObjectsWithTag("item bar"))
    //    {
    //        Destroy(o);
    //    }
    //    int _posX = 1060;
    //    int _posY = 540;
    //    int d3 = 0;

    //    List<Item> toBeDestroyed = new List<Item>();
    //    List<string> itemNames = new List<string>();

    //    int[] numbersToAdd;
    //    List<Item> modifiedItems;
    //    foreach(Item i in ItemList.SavedItems)
    //    {
    //        bool wantToAdd = true;

    //        if (i._itemType == Item.itemType.Hand)
    //        {
    //            if (itemNames == null)
    //            {
    //                print("item names empty");
    //            }
    //            else
    //            {
    //                foreach (string s in itemNames)
    //                {
    //                    if (s == i.name)
    //                    {
    //                        GameObject.Find(i.name).GetComponent<ScreenItem>().count += 1;
    //                        toBeDestroyed.Add(i);
    //                        wantToAdd = false;
    //                        break;
    //                    }
    //                    else
    //                    {

    //                        print("New Item!");
    //                    }
    //                }
    //            }

    //            if (wantToAdd)
    //            {
    //                print(i.name);
    //                GameObject go = Instantiate(itemEquipBar, new Vector3(_posX, _posY, 0), Quaternion.identity) as GameObject;
    //                go.GetComponentsInChildren<Text>()[0].text = i.name;
    //                go.GetComponent<ScreenItem>().count = i.count;

    //                go.transform.parent = GameObject.Find("Equipment Selection").transform;
    //                go.name = i.name;


    //                itemNames.Add(i.name);

    //                _posX += 175;
    //                d3 += 1;

    //                if (d3 % 3 == 0)
    //                {
    //                    _posY += -40;
    //                    _posX = 1060;
    //                }
    //            }
    //        }
    //    }

    //    foreach(Item t in toBeDestroyed)
    //    {
    //        ItemList.SavedItems.Remove(t);
    //    }
    //}

    // i think this is when you havent selected a character to equip and go back
    public void OnEquipExit()
    {
        equipScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }
}
