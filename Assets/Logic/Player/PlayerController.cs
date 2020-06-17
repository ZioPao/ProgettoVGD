﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{


    //Reimplementation of the original controller made by unity

    //if we're not touching anything, then jumping - no wasd
    //if we're touching something but on a slope - nerfed wasd


    [SerializeField] private float movementMultiplier = 50f;
    [SerializeField] private float boostSpeed = 2f;
    [SerializeField] private float jumpForce = 50000f;

    private Rigidbody rb;
    private Animator anim;
    private GameObject bulletRotationPoint;
    private GameObject bulletSpawnPoint;

    private Vector3 movementVec;
    private float movementSpeed = 10;
    
    //Various booleans
    private bool isJumping = false;
    private bool isRunning = false;
    private bool isShooting = false;
    private bool isMovingOnSlope = false;
    private bool canMove = true;


    //Stuff for moving 
    private float oldForwardMovement;
    private float oldRightMovement;


    // Start is called before the first frame update
    void Start()
    {
        movementSpeed *= movementMultiplier;
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        bulletSpawnPoint = GameObject.Find("Camera_Main");
        

    }

    private void FixedUpdate()
    {
        GetMovement();
        Shoot();


        SetAnimations();
        Move();
        Jump();


        //Debug stuff
        //print("CanMove: " + canMove);
        //print("movingOnSlope: " + movingOnSlope);
    }


    /** MOVEMENT 
     */
    private void GetMovement()
    {



        float forwardMovement, rightMovement;
        float movementSpeedCustom = movementSpeed;

        if (canMove)
        {

            //Boost
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeedCustom *= boostSpeed;
                isRunning = true;

            }
            else
            {
                isRunning = false;

            }

            //CheckOnSlope
            if (isMovingOnSlope)
            {
                movementSpeedCustom -= 150;

            }

            forwardMovement = Input.GetAxis("Vertical") * movementSpeedCustom * Time.deltaTime;
            rightMovement = Input.GetAxis("Horizontal") * movementSpeedCustom * Time.deltaTime;

            //Check Diagonale

            //todo there must be a better way
            if (forwardMovement != 0 && rightMovement != 0)
            {
                forwardMovement /= 1.42f;       //todo determinare se è sempre questo valore
                rightMovement /= 1.42f;
            }


            oldForwardMovement = forwardMovement;
            oldRightMovement = rightMovement;
        }
        else
        {
            forwardMovement = oldForwardMovement / 2;
            rightMovement = oldRightMovement / 2;


            // mid air, doesn't make sense to keep the animation
            isRunning = false;
        }

       
        Vector3 forwardMov = transform.forward * forwardMovement;
        Vector3 rightMov = transform.right * rightMovement;

        //Saves all of it in that private variable
        movementVec = (forwardMov + rightMov + GetGravitySpeed());
        
      

    }

    private void moveOnSlope()
    {
        //The steeper the slope, the less speed we can have
    }
    private Vector3 GetGravitySpeed()
    {
        //Testing a much stronger force
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
    /** Check e attivazione dello shooting
     */
    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit projectile;
            isShooting = true;
            if (Physics.Raycast(bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.forward, out projectile, 100))
            {
                if (projectile.transform.gameObject.CompareTag("EnemyModel"))
                    Destroy(projectile.transform.parent.gameObject);

                //ParticleSystem exp = GetComponent<ParticleSystem>();
                //exp.Play();
                //when it hits something, it destroys it
                //print(projectile.transform);
            }

        }
        else
            isShooting = false;



    }


    private void SetAnimations()
    {
        //todo something is broken about the running anim 
        if (isRunning)
        {
            if (movementVec.x != 0 || movementVec.z != 0)
                anim.SetBool("isRunning", true);
            else
                anim.SetBool("isRunning", false);
            
        }
        else
            anim.SetBool("isRunning", false);



        //todo add if is running and shooting, since it doesnt work right now
        if (isShooting)
        {
            anim.SetBool("isShooting", true);
        }
        else
        {
            anim.SetBool("isShooting",false);
        }

    }


    /*Check collisioni*/

    private bool isPlayerGrounded(Collision collision)
    {
        //This check must start ONLY if we know that we're touching something flat

        //Gets the contact point that our charchter is colliding with
        ContactPoint contact = collision.GetContact(0);

        //Checks if it's touching our character feet
        if (Vector3.Dot(contact.normal, Vector3.up) > 0.8)
        {
            //print(Vector3.Dot(contact.normal, Vector3.up));
            return true;
        }

        else
            return false;

    }

    private bool isPlayerOnASlope(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);

        if (Vector3.Dot(contact.normal, Vector3.up) < 0.85)
        {
            return true;
        }

        else
            return false;
    }

    //Overrided methods
    private void OnCollisionEnter(Collision collision)
    {
     
        if (isPlayerGrounded(collision))
        {
            canMove = true;

            //Reactivate jump
            if (isJumping)
                isJumping = false;
        }
        else if (!isJumping && !isPlayerGrounded(collision))
        {
            isMovingOnSlope = true; 
            canMove = true;
        }
        ///* Jumping stuff*/
        //bool isGrounded = isPlayerGrounded(collision);
        //if (isGrounded)
        //{
        //    canMove = true;

        //    if (isJumping)
        //        isJumping = false;
        //}
        ////Slopes
        //else
        //{
        //    if (!isJumping)
        //    {
        //        isMovingOnSlope = true;
        //        canMove = true;

        //    }

        //}

        //if (isJumping)
        //{
        //    //Stop movement?
        //    oldForwardMovement = 0;
        //    oldRightMovement = 0;
        //}

     
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.contactCount == 0)
            isMovingOnSlope = false;
        //if (collision.contactCount == 0)
        //{
        //    canMove = false;
        //   // isMovingOnSlope = false;

        //}

    }
    private void OnCollisionStay(Collision collision)
    {
        if (isPlayerGrounded(collision))
            canMove = true;
        


    }

}

