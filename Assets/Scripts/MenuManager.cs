using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public GameObject menuPanel;
    public GameObject mainMenuButton;
    public GameObject mainMenuScreen;
    public GameObject inventoryScreen;
    public GameObject selectedItem;
    public GameObject saveButton;

    public bool canSave;

    public List<string> itemNames = new List<string>();

    public GameObject descriptionBar;
    public GameObject ItemBar;
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
    }

    public void ItemScreenEnter()
    {
        ItemList.SavedItems = SceneItemList.savedItems;

        print("Number of Items: " + ItemList.SavedItems.Count);
        inventoryScreen.SetActive(true);
        mainMenuScreen.SetActive(false);
        foreach (Item i in ItemList.SavedItems)
        {
            //Canvas cheat sheet x - 960, y - 540.
            GameObject go = Instantiate(ItemBar, new Vector3(posX, posY, 0), Quaternion.identity) as GameObject;
            go.transform.parent = GameObject.Find("Item Screen").transform;
            thing = go.GetComponentsInChildren<Text>();

            thing[0].text = i.name;
            go.gameObject.name = i.name;

            var ahhh = go.GetComponent<ScreenItem>();

            ahhh.description = i.description;
            if (i._itemType == Item.itemType.Consumable)
            {
                ahhh._itemType = ScreenItem.itemType.Consumable;
            }

            wantToAdd = true;
            if (itemNames != null)
            {
                foreach (string w in itemNames)
                {
                    if (w == i.name)
                    {
                        var currentObject = GameObject.Find(i.name);
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
                itemNames.Add(i.name);
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
        itemBars = GameObject.FindGameObjectsWithTag("item bar");
        foreach (GameObject i in itemBars)
        {
            Destroy(i);
        }
        print(ItemList.SavedItems.Count);
        mainMenuScreen.SetActive(true);
        inventoryScreen.SetActive(false);
    }

    public void SavePressed()
    {
        InventoryManager.instance.SaveGame();
    }

    public void OnMenuPress()
    {
        mainMenuButton.SetActive(false);
        mainMenuScreen.SetActive(true);
        menuPanel.SetActive(true);
        if (canSave)
        {
            saveButton.SetActive(true);
        }
    }
}
