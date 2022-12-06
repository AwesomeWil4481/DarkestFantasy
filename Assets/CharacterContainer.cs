using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterContainer : MonoBehaviour
{
    public PositionTwoContainer thisCharacter;

    public GameObject thisImage;
    public GameObject thisPanel;
    public GameObject battleMenu;

    private void Start()
    {
        if(thisCharacter == null)
        {
            thisImage.SetActive(false);
            thisImage.SetActive(false);
        }
        else
        {
            DeactivateUI();
        }
    }

    private void Update()
    {
        if (thisCharacter != null)
        {
            if (thisCharacter.Active)
            {
                ActivateUI();
            }

            thisImage.SetActive(true);
            thisPanel.SetActive(true);
        }
    }

    void DeactivateUI()
    {
        thisImage.GetComponent<Image>().color = new Color(80, 80, 80);
    }

    void ActivateUI()
    {
        thisImage.GetComponent<Image>().color = new Color(255, 255, 255);

        battleMenu.GetComponentInChildren<BattleMenu>().RegisterActor(thisCharacter);
    }
}
