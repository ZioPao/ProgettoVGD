using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    

    //Reimplementation of the original controller made by unity


    
    [SerializeField] private float movementMultiplier = 50f;
    [SerializeField] private float jumpForce = 500f;

    private Rigidbody rb;
    private Vector3 movementVec;
    private float movementSpeed = 10;
    private bool isJumping = false;
    private bool canMove = true;



    //Stuff for moving 
    private float oldXMovement;
    private float oldYMovement;

    // Start is called before the first frame update
    void Start()
    {
        movementSpeed *= movementMultiplier;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        GetMovement();
        Move();
        Jump();
        //print(rb.velocity);
    }


    private void GetMovement()
    {
        float xMovement;
        float yMovement;
        if (canMove)
        {
            xMovement = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
            yMovement = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;

            oldXMovement = xMovement;
            oldYMovement = yMovement;
        }
        else
        {
            xMovement = oldXMovement / 2;
            yMovement = oldYMovement / 2;
        }



        Vector3 forwardMov = transform.forward * yMovement;
        Vector3 rightMov = transform.right * xMovement;


            //Saves all of it in that private variable
         movementVec = (forwardMov + rightMov + GetGravitySpeed());

    }


    private Vector3 GetGravitySpeed()
    {

        float gravityVelocity = rb.velocity.y;
        return new Vector3(0, gravityVelocity, 0);
    }
    public void Move()
    //Using the rigidbody we add a force to push our character until topSpeed
    {

        rb.velocity = movementVec;

    }



    public void Jump()
     
    {

        if (Input.GetKey("space") && !isJumping)
        {
            print("Jumping");
            isJumping = true;
            //timerCheckCollision = 2f;        //Sets the initial timer


            //Continue going towards that way
            Vector3 tmp = (transform.up * jumpForce);
            rb.AddForce(tmp, ForceMode.Force);
        }

          
    }

    bool isPlayerGrounded(Collision collision)
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
        bool isGrounded = isPlayerGrounded(collision);
        if (isGrounded)
        {
            canMove = true;

            if (isJumping)
                isJumping = false;
        }
     
    }


    private void OnCollisionExit(Collision collision)
    {


        //Check the contacts

        //todo add a better check to determine if it's a terrain and not only this
        if (collision.contacts.Length == 0)
        {
            //todo add check if last collision was terrain

            canMove = false;
        }

        


    }










}
