using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class MenuManager : MonoBehaviour
{


    public static MenuManager instance;

    public Vector3 savePointPos;

    public GameObject menuPanel;
    public GameObject mainMenuButton;
    public GameObject mainMenuScreen;
    public GameObject inventoryScreen;
    public GameObject selectedItem;
    public GameObject saveButton;
    public GameObject equipScreen;
    public GameObject equipButton;
    public GameObject statBlocks;
    public GameObject equipmentSelectionScreen;
    public GameObject[] equipButtons;
    public GameObject[] _statBlocks;
    public GameObject selectionScreen;
    public GameObject namePlate;
    public GameObject SaveSelectionScreen;
    public GameObject EquipmentBar;
    public GameObject Joystick;

    public Sprite[] characterPortraits;

    public bool canSave;

    public List<string> itemNames = new List<string>();

    public GameObject descriptionBar;
    public GameObject ItemBar;
    public GameObject itemEquipBar;
    public GameObject[] itemBars;

    public Text[] thing;

    int otherNumber;
    int number;
    int posX = 320;
    int posY = 915;
    int rotationNumber = 1;


    bool wantToAdd;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void BackToScene()
    {
        menuPanel.SetActive(false);
        mainMenuButton.SetActive(true);
        mainMenuScreen.SetActive(false);
        Joystick.SetActive(true);
    }

    public void AddItemToMasterEquipmentList(EquipableItem equipableItem)
    {
        MasterEquipmentContainer.Instance.Equipment.Add(equipableItem);
    }

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

    public void ItemPress()
    {
        //descriptionBar.text = button.gameObject.GetComponent<ScreenItem>().description;
    }

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

    public void EquipmentSelectionScreenPress()
    {
        equipmentSelectionScreen.SetActive(true);
        statBlocks.SetActive(false);
        equipScreen.SetActive(false);

        List<Foo> f = new List<Foo> { new Bar(), new Baz() };
        var f1 = f.OfType<Bar>();

        print(ItemList.Instance().Items[0].Name);

        int Num = 0;

        foreach (EquipableItem j in MasterEquipmentContainer.Instance.Equipment)
        {
            print(MasterEquipmentContainer.Instance.Equipment[Num]);
            Num += 1;
        }

        int VectorX = 1060;
        int VectorY = 540;

        var t = ItemList.Instance().Items
            .Where(x => x.ItemType == ItemType.EquipableItem)
            .Select(x => MasterEquipmentContainer.Instance.Equipment.First(e => e.Name == x.Name))
            .Select(x =>
            {
                var g = new GameObject(x.Name);
                print(g);
                GameObject go = Instantiate(EquipmentBar, new Vector3(VectorX, VectorY, 0), Quaternion.identity) as GameObject;
                go.transform.parent = GameObject.Find("Equip Screen").transform;
                thing = go.GetComponentsInChildren<Text>();

                        go.GetComponent<EquipmentBarScript>().Armor = x.Armor;
                        go.GetComponent<EquipmentBarScript>().HPBonus = x.HPBonus;
                        go.GetComponent<EquipmentBarScript>().STBonus = x.STBonus;
                        go.GetComponent<EquipmentBarScript>().HPPercentBonus = x.HPPercentBonus;
                        go.GetComponent<EquipmentBarScript>().STPercentBonus = x.STPercentBonus;

                go.GetComponentsInChildren<Text>()[0].text = x.Name;

                return g;
            })
            .ToList();

    }

    abstract class Foo
    {
        public List<string> Blah { get; }
    }

    class Bar : Foo
    {

    }

    class Baz : Foo
    {

    }

    public void EquipmentSelectionScreenExit()
    {
        equipmentSelectionScreen.SetActive(false);
        selectionScreen.SetActive(false);
        statBlocks.SetActive(true);
        mainMenuScreen.SetActive(true);
    }

    public void SavePressed()
    {
        statBlocks.SetActive(false);
        mainMenuScreen.SetActive(false);
        SaveSelectionScreen.SetActive(true);
    }

    public void SaveBack()
    {
        statBlocks.SetActive(true);
        mainMenuScreen.SetActive(true);
        SaveSelectionScreen.SetActive(false);
    }

    public void SaveSelected(string SaveSelected)
    {
        SaveTheBooks.SaveGame(SaveSelected);
    }

    public void OnMenuPress()
    {
        Joystick.SetActive(false);
        mainMenuButton.SetActive(false);
        mainMenuScreen.SetActive(true);
        menuPanel.SetActive(true);
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
    }

    public void onMemberEquipSelect(int Position)
    {
        int position = Position -= 1;

        selectionScreen.SetActive(true);
        equipScreen.SetActive(false);
        statBlocks.SetActive(false);

        var selectedCharacter = GameObject.Find("Character Statistics").GetComponentsInChildren<Text>();

        int currentSprite = 0;
        foreach (Sprite o in characterPortraits)
        {
            print("Times looped " + currentSprite);
            print("Current Sprite Name " + characterPortraits[currentSprite].name);
            print("Name to Search For : " + SavedCharacters.Instance().currentStats[position].characterName);
            print(position);

            selectedCharacter[9].text = SavedCharacters.Instance().currentStats[position].strength.ToString();

            if (characterPortraits[currentSprite].name == SavedCharacters.Instance().currentStats[position].characterName)
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

    public void OnEquipPress()
    {
        equipScreen.SetActive(true);
        mainMenuScreen.SetActive(false);
        
        foreach (PositionTwoContainer i in GameObject.Find("Character Scriptobject").GetComponent<CharacterStatisticsSerializer>().currentParty)
        {
            equipButtons[i._position].SetActive(true);
            //equipButtons[i._position].GetComponent<LoadCharacterStats>().myTiedObject = i;
        }
    }

    public void OnEquipButtonClick()
    {
        foreach (EquipableItem i in EquipmentList.instance.equipableItems)
        {
            if (i.EquipmentType.ToString() == gameObject.name)
            {
                print("It's a match");
            }
            else
            {
                print("No match");
            }
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

    public void OnEquipExit()
    {
        equipScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }
}
