using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conversationManager : MonoBehaviour
{
    public GameObject speechBubble;
    public GameObject playerBody;
    public GameObject joyStick;
    public GameObject textBox;

    bool playerNextTo = false;

    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        speechBubble.SetActive(true);
        playerNextTo = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        speechBubble.SetActive(false);
        playerNextTo = false;
    }

    void Update()
    {

        if (playerNextTo == true)
        {
            playerInteraction();
        }
    }

    void playerInteraction()
    {
        if (playerBody.transform.position.y <= transform.position.y && Input.GetKeyDown(KeyCode.W))
        {
            speechBubble.SetActive(false);
            textBox.SetActive(true);
            joyStick.SetActive(false);
        }
        if (playerBody.transform.position.y >= transform.position.y && Input.GetKeyDown(KeyCode.S))
        {
            speechBubble.SetActive(false);
            textBox.SetActive(true);
            joyStick.SetActive(false);
        }
        if(playerBody.transform.position.x >= transform.position.x && Input.GetKeyDown(KeyCode.A))
        {
            speechBubble.SetActive(false);
            textBox.SetActive(true);
            joyStick.SetActive(false);
        }
        if(playerBody.transform.position.x <= transform.position.x && Input.GetKeyDown(KeyCode.D))
        {
            speechBubble.SetActive(false);
            textBox.SetActive(true);
            joyStick.SetActive(false);
        }
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                speechBubble.SetActive(false);

            }
        }
    }
}
