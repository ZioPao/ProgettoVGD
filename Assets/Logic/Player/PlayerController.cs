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
    }


    private void GetMovement()
    {
        float xMovement = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        float yMovement = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;

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




            Vector3 tmp = transform.up * jumpForce * Time.deltaTime;
            rb.AddForce(tmp, ForceMode.VelocityChange);
        }

        //else if (isJumping) //decrease the timer
        //    timerCheckCollision -= Time.deltaTime;
          
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
        if (isGrounded && isJumping)
        {
            isJumping = false;
        }
    }










}
