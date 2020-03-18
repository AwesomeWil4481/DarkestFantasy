using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerMovement : MonoBehaviour
{
    public float speed;

    bool nextToObject = false;
    bool firstTime = true;

    private Rigidbody2D PCRB;

    public float RETimer;
    public float RETimerMax;
    public float REValue;
    public float REAreaReq = 0.2f;

    private Vector3 movement;
    private Vector3 posNeg;
    private Vector3 posPos;
    private Vector3 negPos;
    private Vector3 negNeg;
    private Vector3 change;
    private Vector3 zero = new Vector3(0,0,0);

    Joystick joyStick;

    public Animator playerAnimator;

    void Start()
    {
        RETimerMax = 2f;
        RETimer = RETimerMax;

        posPos = new Vector3(4, 4, 0);
        posNeg = new Vector3(4, -4, 0);
        negPos = new Vector3(-4, 4, 0);
        negNeg = new Vector3(-4, -4, 0);

        joyStick = FindObjectOfType<Joystick>();
        playerAnimator = GetComponent<Animator>();
        PCRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        var botLeft = Vector3.Angle(movement, negNeg);
        var botRight = Vector3.Angle(movement, posNeg);
        var topLeft = Vector3.Angle(movement, negPos);
        var topRight = Vector3.Angle(movement, posPos);
        RETimer -= Time.deltaTime;
        movement = PCRB.velocity;
        if (movement == zero)
        {
            playerAnimator.SetBool("moving", false);
        }
        if (RETimer <= 0)
        {
            RETimer = RETimerMax;
            REValue = Random.value;
            print("timer done");
            firstTime = false;
            if(REValue <= REAreaReq && !firstTime)
            {
                firstTime = true;
                print("switching scenes");
                StartCoroutine(SceneTransition.instance.EndScene("Fighting Scene"));
            }
        }
        if (nextToObject == false)
        {

            if (botLeft <= 90 && botRight <= 90 && movement != zero)
            {
                playerAnimator.SetFloat("moveY", -1);
                playerAnimator.SetFloat("moveX", 0);
                playerAnimator.SetBool("moving", true);
            }

            if (topRight <= 90 && topLeft <= 90 && movement != zero)
            {
                playerAnimator.SetFloat("moveY", 1);
                playerAnimator.SetFloat("moveX", 0);
                playerAnimator.SetBool("moving", true);

            }

            if (botLeft <= 90 && topLeft <= 90 && movement != zero)
            {
                playerAnimator.SetFloat("moveX", -1);
                playerAnimator.SetFloat("moveY", 0);
                playerAnimator.SetBool("moving", true);

            }

            if (topRight <= 90 && botRight <= 90 && movement != zero)
            {
                playerAnimator.SetFloat("moveX", 1);
                playerAnimator.SetFloat("moveY", 0);
                playerAnimator.SetBool("moving", true);

            }
        }

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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        nextToObject = true;   
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        nextToObject = false;
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
            //playerAnimator.SetBool("moving", false);
        }

        void moveCharacter()
        {
            PCRB.MovePosition(transform.position + change * speed * Time.deltaTime);
        }

    }
}

