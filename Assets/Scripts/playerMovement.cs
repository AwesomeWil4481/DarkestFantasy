using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerMovement : MonoBehaviour
{
    public float speed;

    bool nextToObject = false;
    bool firstTime = true;
    bool inCutscene;

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
    Vector3 Pos { get; set; }

    public Joystick joyStick;
    public GameObject JoystickContainer { get; set; }
    public GameObject NewJoystick;

    public Animator playerAnimator;

    void Start()
    {
        RETimerMax = 5f;
        RETimer = RETimerMax;

        posPos = new Vector3(4, 4, 0);
        posNeg = new Vector3(4, -4, 0);
        negPos = new Vector3(-4, 4, 0);
        negNeg = new Vector3(-4, -4, 0);

        joyStick = FindObjectOfType<Joystick>();
        playerAnimator = GetComponent<Animator>();
        PCRB = GetComponent<Rigidbody2D>();
        PCRB.gameObject.transform.position = LoadCharacterPosition();
    }
    public static Vector3 LoadCharacterPosition()
    {
        Vector3 pos = VPos.savedPlayerPos;
        return pos;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            JoystickContainer = GameObject.Find("Joystick");
            StartCoroutine(CutsceneEnter(5));
        }

        var botLeft = Vector3.Angle(movement, negNeg);
        var botRight = Vector3.Angle(movement, posNeg);
        var topLeft = Vector3.Angle(movement, negPos);
        var topRight = Vector3.Angle(movement, posPos);

        movement = PCRB.velocity;

        if (movement == zero)
        {
            playerAnimator.SetBool("moving", false);
        }
        else
        {
            RETimer -= Time.deltaTime;
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
        if (inCutscene != true)
        {
            PlayerMove();
            PCRB.velocity = new Vector2(joyStick.Horizontal * speed, joyStick.Vertical * speed);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        nextToObject = true;   
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        nextToObject = false;
    }
    private void PlayerMove()
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

    public IEnumerator CutsceneEnter(int Length)
    {
        int CutsceneSpeedY = -50;
        Vector3 JoystickPos = new Vector3(300,300,0);
        PCRB.velocity = zero;
        var gameObjects = GameObject.FindGameObjectsWithTag("Joystick");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            JoystickPos = gameObjects[i].transform.position;
            Destroy(gameObjects[i]);
        }
        inCutscene = true;
        while (inCutscene == true)
        {
            CutsceneMovement(CutsceneSpeedY);
            yield return new WaitForSeconds(Length);
            print("fase two starting");
            while (inCutscene == true)
            {
                PCRB.velocity = zero;
                CutsceneMovement(100);
                yield return new WaitForSeconds(2);
                print("fase 2 complete");
                print("fase 3 starting");

                while (inCutscene == true)
                {
                    PCRB.velocity = zero;
                    CutsceneSpeedY = -300;
                    CutsceneMovement(CutsceneSpeedY);
                    yield return new WaitForSeconds(Length);
                    print("fase 3 complete");
                    inCutscene = false;
                }
            }
        }
        if (inCutscene == false)
        {
            GameObject go = Instantiate(NewJoystick, JoystickPos, Quaternion.identity, transform.parent = GameObject.FindGameObjectWithTag("Canvas").transform) as GameObject; //327, 87, 0, 600, 50     \     327, 26, 0, 600, 50
            joyStick = go.GetComponentInChildren<Joystick>();
        }
    }

    public void CutsceneMovement(int Speed)
    {
        PCRB.AddForce(new Vector2(0, Speed));
    }
}

