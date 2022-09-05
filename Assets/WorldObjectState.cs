using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldObjectState : MonoBehaviour
{
    public bool interacted;
    public int ID;
    public List<Item> Loot = new List<Item>();

    public Sprite preInteraction;
    public Sprite postInteraction;

    string full = "You found a ";
    string empty = "Empty";

    bool playerNextTo;
    bool canEncounter;

    Vector3 NewPosition;
    Vector3 SavedPosition;

    public GameObject playerBody;
    public GameObject Joystick;
    public GameObject Textbox;

    private Scene previousScene;

    bool firstTime = true;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        playerNextTo = true;
    }

    public void Start()
    {
        if (gameObject.GetComponent<WorldObjectState>().interacted == true)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = postInteraction;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = preInteraction;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        canEncounter = true;
        playerNextTo = false;

        NewPosition = Vector3.zero;
        SavedPosition = Vector3.zero;
    }

    void Update()
    {
        if (playerNextTo == true && canEncounter == true)
        {
            onPlayerInteraction();
        }
    }

    void Open()
    {
        if (firstTime)
        {
            if (gameObject.GetComponent<WorldObjectState>().interacted == false)
            {
                Joystick.SetActive(false);
                Textbox.SetActive(true);

                Textbox.GetComponentInChildren<Text>().text = "" + full + Loot[0].Name;

                gameObject.GetComponent<WorldObjectState>().interacted = true;
                gameObject.GetComponent<SpriteRenderer>().sprite = postInteraction;

                ItemList.Instance().Items.AddRange(Loot);

                foreach (WorldObjectState s in gameObject.transform.parent.GetComponent<WorldStateManager>().savedObject.GetComponentsInChildren<WorldObjectState>())
                {
                    if(s. ID == ID)
                    {
                        s.interacted = gameObject.GetComponent<WorldObjectState>().interacted;

                        WorldStateManager.Instance().markedForSaveObjects.Add(gameObject.transform.parent.gameObject);
                        WorldStateManager.Instance().SaveMark.Add(SceneManager.GetActiveScene().name.ToString());

                        break;
                    }
                }

                
                firstTime = false;
            }
            else
            {
                Joystick.SetActive(false);
                Textbox.SetActive(true);

                Textbox.GetComponentInChildren<Text>().text = "" + empty;
                firstTime = false;
            }
        }
        else
        {
            Joystick.SetActive(true);
            Textbox.SetActive(false);
            canEncounter = false;

            firstTime = true;
        }

    }

    void onPlayerInteraction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SavedPosition = playerBody.transform.position;
        }
        if (Input.GetMouseButtonUp(0))
        {
            NewPosition = playerBody.transform.position;
            if (SavedPosition == NewPosition)
            {
                Open();
            }
        }

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
                    Open();
                }
            }
        }
    }
}
