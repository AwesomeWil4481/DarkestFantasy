using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed;

    public bool isPlayer = true;

    private Rigidbody2D PCRB;

    private Vector3 change;

    Joystick joyStick;

    public Animator playerAnimator;

    void Start()
    {
        joyStick = FindObjectOfType<Joystick>();
        playerAnimator = GetComponent<Animator>();
        PCRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {

        if (PCRB.transform.rotation.y != 0)
        {
            PCRB.transform.position = new Vector3(0, 0, 0);
        }
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        playerMove();
        PCRB.velocity = new Vector2(joyStick.Horizontal * 4, joyStick.Vertical * 4);
    }

    private void playerMove()
    {
        if (change != Vector3.zero)
        {
            moveCharacter();
            playerAnimator.SetFloat("moveX", change.x);
            playerAnimator.SetFloat("moveY", change.y);
            playerAnimator.SetBool("moving", true);
        }
        else
        {
            playerAnimator.SetBool("moving", false);
        }

        void moveCharacter()
        {
            PCRB.MovePosition(transform.position + change * speed * Time.deltaTime);
        }

    }
}

