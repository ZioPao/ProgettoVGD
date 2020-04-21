using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed = 30f;
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float maxJump = 105.0f;

    private CharacterController controller;
    private Transform pt;
    private Rigidbody rb;
    private bool isJumping = false;
    private bool checkGrounded;
    private float maxHeight = 0;
    private float lastHeight = 0;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        pt = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        movePlayer();
        jumpMovement();
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

    void jumpMovement()
    {

        if (Input.GetKey("space") && controller.isGrounded)
        {
            maxHeight = pt.position.y + maxJump;

            print("Started a jump");
            print("Max height: " + (maxHeight.ToString()));
            isJumping = true;
        }


        if (isJumping)
        {

            //keeps going up until gravity starts doing its job
            controller.Move(new Vector3(0, jumpForce * Time.deltaTime, 0));


        }

        // print(pt.position.y.ToString() + " >= " + maxHeight.ToString());
        //print(lastHeight);
        if (isJumping && (pt.position.y >= maxHeight || lastHeight > pt.position.y))
        {
            print("ended a jump");
            isJumping = false;
            maxHeight = 0;      //reset
        }
        
        lastHeight = pt.position.y;
    }


    //    //float jumpMov = Convert.ToInt32(Input.GetKey("space")) * jumpForce * Time.deltaTime;
    //    while (false)
    //        {
    //            isJumping = true;
    //        }

    //    }
    //    else if (!controller.isGrounded && isJumping)
    //    {
    //        //add another mov
    //        print("still jumping");
    //        controller.Move(new Vector3(0, jumpForce * Time.deltaTime, 0));

    //    }
    //    else if (controller.isGrounded && pt.position.y >= maxHeight)
    //    {
    //        // when it has finished jumping
    //        print("Jumping finished");
    //        isJumping = false;
    //    }
        
    //}
}

 
