using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    

    //Reimplementation of the original controller made by unity


    
    [SerializeField] private float movementMultiplier = 50f;
    [SerializeField] private float boostSpeed = 2f;
    [SerializeField] private float jumpForce = 50000f;

    private Rigidbody rb;
    private Animator anim;

    private Vector3 movementVec;
    private float movementSpeed = 10;
    
    private bool isJumping = false;
    private bool canMove = true;
    private bool isRunning = false;


    //Stuff for moving 
    private float oldXMovement;
    private float oldYMovement;


    // Start is called before the first frame update
    void Start()
    {
        movementSpeed *= movementMultiplier;
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

    }

    private void FixedUpdate()
    {
        GetMovement();


        SetAnimations();
        Move();
        Jump();
        //print(rb.velocity);
    }


    private void GetMovement()
    {
        float xMovement;
        float yMovement;

        float movementSpeedCustom = movementSpeed;

        if (canMove)
        {

            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeedCustom *= boostSpeed;
                isRunning = true;

            }

            else
                isRunning = false;

            xMovement = Input.GetAxis("Horizontal") * movementSpeedCustom * Time.deltaTime;
            yMovement = Input.GetAxis("Vertical") * movementSpeedCustom * Time.deltaTime;

            oldXMovement = xMovement;
            oldYMovement = yMovement;
        }
        else
        {
            xMovement = oldXMovement / 2;
            yMovement = oldYMovement / 2;


            // mid air, doesn't make sense to keep the animation
            isRunning = false;
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

    //Using the rigidbody we add a force to push our character until topSpeed
    private void Move()
    {

        rb.velocity = movementVec;

    }




    private void Jump()
     
    {

        if (Input.GetKey("space") && !isJumping)
        {
            //print("Jumping");
            isJumping = true;
            //timerCheckCollision = 2f;        //Sets the initial timer


            //Continue going towards that way
            Vector3 tmp = (transform.up * jumpForce);
            rb.AddForce(tmp, ForceMode.Force);
        }

          
    }

    private void Shoot()
    {
        bool isShooting = Input.GetMouseButton(0);

        if (isShooting)
        {
            //todo starts shooting animation
        }
    }

    private void SetAnimations()
    {
        if (isRunning)
        {
            if (movementVec.x != 0 || movementVec.z != 0)
                anim.SetBool("isRunning", true);
            
        }
        else
            anim.SetBool("isRunning", false);

    }


    /*Check collisioni*/

    private bool isPlayerGrounded(Collision collision)
    {
        //This check must start ONLY if we know that we're touching something flat

        //Gets the contact point that our charchter is colliding with
        ContactPoint contact = collision.GetContact(0);

        //Checks if it's touching our character feet
        if (Vector3.Dot(contact.normal, Vector3.up) > 0.5)
        {
            //print("Touched terrain");
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
        if (collision.contactCount == 0)
            canMove = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (isPlayerGrounded(collision))
            canMove = true;
        


    }

}
