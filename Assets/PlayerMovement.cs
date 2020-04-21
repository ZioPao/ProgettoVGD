using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed = 30f;
    [SerializeField] private float jumpForce = 10.0f;

    private CharacterController controller;
    private Transform pt;

    private bool isJumping = false;     //keeps track if the player is still jumping or not
    private float lastHeight = -1;      //keeps track of the last known height

    private bool checkJump;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        pt = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        movePlayer();
        jumpAction();
    }

    void movePlayer()
    {

        float xMovement = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        float yMovement = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        //Diversi tipi di movimento

        Vector3 forwardMov = transform.forward * yMovement;
        Vector3 rightMov = transform.right * xMovement;

        Vector3 finalMov = forwardMov + rightMov;
        //print(finalMov);

        controller.SimpleMove(forwardMov + rightMov);


    }

    void jumpAction()
    {

        if (Input.GetKey("space") && controller.isGrounded)
        {
            //print("Started a jump");
            isJumping = true;
        }


        //last one diventa meno della precedente, segnamo in un bool
        //se questo bool è attivo e last one diventa piu della precedente, stop

        if (isJumping)
        {
            //print("Last height: " + lastHeight.ToString());
            //print("checkjump :" + checkJump);
            controller.Move(new Vector3(0, jumpForce * Time.deltaTime, 0));


            if (checkJump && lastHeight < pt.position.y)
            {
                //print("Current height: " + pt.position.y.ToString());
                //print("Finito salto!");
                isJumping = false;
                checkJump = false;

                lastHeight = -1;        //reset
            }

            else if (lastHeight > pt.position.y && checkJump == false)
            {
                //print("Stopped going up");
                checkJump = true;
            }
 
        }

        //Keeps in a variable the last known height
        lastHeight = pt.position.y;



    }

  

}

 
