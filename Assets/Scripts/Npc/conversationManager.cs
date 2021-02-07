﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class conversationManager : MonoBehaviour
{
    public List<string> npcDialogues = new List<string>();

    public GameObject speechBubble;
    public GameObject playerBody;
    public GameObject joyStick;
    public GameObject textBox;

    public GameObject LeftBox;
    public GameObject RightBox;
    public GameObject TopBox;
    public GameObject BottomBox;

    public Text textBoxText;

    int numberOfLines = -1;
    int numberOfDialogues = 1;

    bool playerNextTo = false;
    bool firstCall = true;
    bool canEncounter = true;
    bool OnOff = false;

    Vector3 SavedPosition;
    Vector3 NewPosition;


    void Start()
    {
        foreach (string i in npcDialogues)
        {
            numberOfLines += 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        speechBubble.SetActive(true);
        playerNextTo = true;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        speechBubble.SetActive(false);
        canEncounter = true;
        playerNextTo = false;
    }

    void Update()
    {
        if (playerNextTo == true && canEncounter == true)
        {
            onPlayerInteraction();
        }
    }

    void onPlayerInteraction()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                SavedPosition = playerBody.transform.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                NewPosition = playerBody.transform.position;
                if (NewPosition == SavedPosition)
                {
                    if (firstCall == true)
                    {
                        joyStick.SetActive(false);
                        textBox.SetActive(true);
                        speechBubble.SetActive(false);

                        textBoxText.text = "" + npcDialogues[0];
                        firstCall = false;
                    }
                    if (firstCall == false && numberOfLines != 0)
                    {
                        textBoxText.text = "" + npcDialogues[numberOfDialogues];
                        numberOfDialogues += 1;
                        numberOfLines -= 1;
                    }
                    else
                    {
                        joyStick.SetActive(true);
                        textBox.SetActive(false);
                        canEncounter = false;
                        firstCall = true;
                        Reset();
                    }
                }
            }
        }
    }
    private void Reset()
    {
        numberOfLines = -1;
        foreach (string i in npcDialogues)
        {
            numberOfLines += 1;
        }
        numberOfDialogues = 1;
    }
}
