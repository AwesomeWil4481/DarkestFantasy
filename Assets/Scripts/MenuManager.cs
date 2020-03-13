using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject mainMenuButton;
    public GameObject mainMenuScreen;
    public GameObject inventoryScreen;

    public GameObject ItemBar;

    int posX = 320;
    int posY = 915;
    int rotationNumber = 1;

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
        inventoryScreen.SetActive(true);
        mainMenuScreen.SetActive(false);

        foreach (Item i in ItemList.SavedItems)
        {
            //Canvas cheat sheet x - 960, y - 540.

            GameObject go = Instantiate(ItemBar, new Vector3(posX, posY, 0), Quaternion.identity) as GameObject;
            go.transform.parent = GameObject.Find("Item Screen").transform;
            var thing = go.GetComponentInChildren<Text>();
            thing.text = i.name;

            if (rotationNumber % 3 == 0)
            {
                print("Rotation Number is  "+rotationNumber);
                posY += -110;
                posX = 320;
            }
            else
            {
                posX += 640;
            }
            rotationNumber += 1;
        }
    }

    public void ItemScreenExit()
    {

    }

    public void OnMenuPress()
    {
        mainMenuButton.SetActive(false);
        mainMenuScreen.SetActive(true);
        menuPanel.SetActive(true);
    }
}
