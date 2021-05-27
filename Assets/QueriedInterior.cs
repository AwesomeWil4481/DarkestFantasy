using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.Collections;

public class QueriedInterior : MonoBehaviour
{
    public GameObject TextBox;
    public GameObject Joystick;
    public GameObject PlayerBody;
    public GameObject NewJoystick;

    [ReadOnly]
    public Vector3 JoystickPos;

    public List<GameObject> Exterminate = new List<GameObject>();

    public Button YesButton;
    public Button NoButton;

    public string Dialogue;

    Vector3 NewPosition;
    Vector3 SavedPosition;
    public Vector3 DesiredPosition;

    bool FirstCall = true;
    bool playerNextTo;
    bool canEncounter;

    int NumberOfDialogues;
    int CurrentLine;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        playerNextTo = true;
        canEncounter = true;
        //PlayerBody.transform.position = DesiredPosition;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        canEncounter = true;
        playerNextTo = false;
        FirstCall = true;

        NewPosition = Vector3.zero;
        SavedPosition = Vector3.zero;
    }
    public void Update()
    {
        if (playerNextTo && canEncounter)
        {
            TextBox.SetActive(true);
            YesButton.gameObject.SetActive(true);
            NoButton.gameObject.SetActive(true);

            var gameObjects = GameObject.FindGameObjectsWithTag("Joystick");

            for (var i = 0; i < gameObjects.Length; i++)
            {
                Destroy(gameObjects[i]);
            }
            //foreach(GameObject g in GameObject.FindGameObjectsWithTag("Joystick"))
            //{
            //    Exterminate.Add(g);
            //}

            //foreach(GameObject g in Exterminate)
            //{
            //    Destroy(g);
            //}
            PlayerBody.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            TextBox.GetComponentInChildren<Text>().text = Dialogue;

            YesButton.onClick.AddListener(ChangePlayerPosition);
            NoButton.onClick.AddListener(CancelInteraction);
        }
    }

    public void PlayerInteraction() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            SavedPosition = PlayerBody.transform.position;
        }
        if (Input.GetMouseButtonUp(0))
        {
            NewPosition = PlayerBody.transform.position;
            if (SavedPosition == NewPosition)
            {
                //InteractionProgression();
            }
        }

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                SavedPosition = PlayerBody.transform.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                NewPosition = PlayerBody.transform.position;
                if (NewPosition == SavedPosition)
                {
                    //InteractionProgression();
                }
            }
        }
    }

    //public void InteractionProgression()
    //{   
    //    while (FirstCall)
    //    {
    //        TextBox.SetActive(true);
    //        Joystick.SetActive(false);
    //        TextBox.GetComponentInChildren<Text>().text = Dialogue;

    //        YesButton.onClick.AddListener(ChangePlayerPosition);
    //        NoButton.onClick.AddListener(CancelInteraction);
    //    }
    //}

    public void ChangePlayerPosition()
    {
        PlayerBody.transform.position = DesiredPosition;

        //var gameObjects = GameObject.FindGameObjectsWithTag("Joystick");

        //for (var i = 0; i < gameObjects.Length; i++)
        //{
        //    Destroy(gameObjects[i]);
        //}

        ////JoyStick.SetActive(true);
        //GameObject go = Instantiate(NewJoystick, JoystickPos, Quaternion.identity, transform.parent = GameObject.FindGameObjectWithTag("Canvas").transform) as GameObject; // 327, 87, 0, 600, 50     \     327, 26, 0, 600, 50
        ////go.transform.position = JoystickPos;
        //PlayerBody.GetComponent<playerMovement>().joyStick = go.GetComponentsInChildren<FloatingJoystick>()[0];

        //YesButton.onClick.RemoveListener(ChangePlayerPosition);
        //NoButton.onClick.RemoveListener(CancelInteraction);

        //TextBox.GetComponentInChildren<Text>().text = "";
        //TextBox.SetActive(false);

        CancelInteraction();
    }

    public void CancelInteraction()
    {
        TextBox.SetActive(false);
        TextBox.GetComponentInChildren<Text>().text = "";

        var gameObjects = GameObject.FindGameObjectsWithTag("Joystick");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }
        //JoyStick.SetActive(true);
        GameObject go = Instantiate(NewJoystick, JoystickPos, Quaternion.identity, transform.parent = GameObject.FindGameObjectWithTag("Canvas").transform) as GameObject; //327, 87, 0, 600, 50     \     327, 26, 0, 600, 50
        //go.transform.position = JoystickPos;
        PlayerBody.GetComponent<playerMovement>().joyStick = go.GetComponentsInChildren<FloatingJoystick>()[0];

        YesButton.onClick.RemoveListener(ChangePlayerPosition);
        NoButton.onClick.RemoveListener(CancelInteraction);

        canEncounter = false;

        FirstCall = false;
    }
}
