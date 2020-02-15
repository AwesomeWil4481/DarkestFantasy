using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerJoystickMovement : playerMovement
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playerMoveRight()
    {
        playerAnimator.Play("Idle", 0, 0);
    }
}
