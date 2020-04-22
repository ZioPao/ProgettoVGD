using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed = 500f;
    [SerializeField] private float jumpForce = 10.0f;

    private CharacterController controller;


    private Vector3 vecMov = new Vector3(0, 0, 0);

    private bool isJumping = false;     //keeps track if the player is still jumping or not

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {

        //Creates a Vector with the new movement to do
        manageBasicMovement();
        manageJumping();

        //Using a SimpleMove we're kinda activating gravity on our player controller. 
        //Using two Moves isn't pretty clean, but it works and it seems kinda ok
        controller.SimpleMove(vecMov);

    }

    void manageBasicMovement()
    {

        float xMovement = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        float yMovement = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        //Diversi tipi di movimento

        Vector3 forwardMov = transform.forward * yMovement;
        Vector3 rightMov = transform.right * xMovement;

        vecMov = forwardMov + rightMov;

    }

    void manageJumping()
    {
        //Jumping will stop once a collision with the terrain is found.

        if (Input.GetKey("space"))
            isJumping = true;

        if (isJumping)
            controller.Move(new Vector3(0, jumpForce * Time.deltaTime, 0));

    }


    bool checkGrounded(Collision collision)
    {

        //Gets the contact point that our charchter is colliding with
        ContactPoint contact = collision.GetContact(0);

        //Checks if it's touching our character feet
        if (Vector3.Dot(contact.normal, Vector3.up) > 0.5)
        {
            print("Touched terrain");
            return true;
        }

        else
            return false;

    }
    private void OnCollisionEnter(Collision collision)
    {

        /* Jumping stuff*/
        if (checkGrounded(collision) && isJumping)
        {
            isJumping = false;
        }
    }


}