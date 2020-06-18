using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{


    //Reimplementation of the original controller made by unity

    //if we're not touching anything, then jumping - no wasd
    //if we're touching something but on a slope - nerfed wasd


    [SerializeField] private float boostSpeed = 2f;
    [SerializeField] private float jumpForce = 50000f;
    [SerializeField] private float movementSpeed = 5f;


    private Rigidbody rb;
    private Animator anim;
    private GameObject bulletRotationPoint;
    private GameObject bulletSpawnPoint;

    private Vector3 movementVec;
    
    //Various booleans
    private bool isJumping = false;
    private bool isRunning = false;
    private bool isShooting = false;
    private bool canMove = true;


    //Stuff for moving 
    private float oldForwardMovement;
    private float oldRightMovement;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        bulletSpawnPoint = GameObject.Find("Camera_Main");
        

    }

    private void FixedUpdate()
    {
        GetMovement();
        Shoot();


        SetAnimations();

        //Move the player
        rb.MovePosition(transform.position + movementVec * Time.fixedDeltaTime);

        //Check jumping
        Jump();

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

      

            forwardMovement = Input.GetAxis("Vertical") * movementSpeedCustom;
            rightMovement = Input.GetAxis("Horizontal") * movementSpeedCustom;

            //Check Diagonale

            //todo there must be a better way

            /*
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

       */

            float speedMultiplier = 1 - (getSlopeAngle() / 90);
            print(speedMultiplier);
            //forwardMovement *= slopeAngle;
            //rightMovement *= slopeAngle;


            Vector3 forwardMov = transform.forward * forwardMovement * speedMultiplier;
            Vector3 rightMov = transform.right * rightMovement * speedMultiplier;

            //Saves all of it in that private variable
            movementVec = (forwardMov + rightMov);
        }
    }




    private void Jump()
     
    {

        if (Input.GetKey("space") && !isJumping)
        {
            isJumping = true;

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

    private float getSlopeAngle()
    {
        float slopeRaycastLength = 5f;
        float slopeRaycastSpread = 0.08f;
        float slopeAngle = 0;
        RaycastHit ray, ray2;
        if (Physics.Raycast(rb.transform.position + new Vector3(slopeRaycastSpread, 0, 0), Vector3.down, out ray, slopeRaycastLength))
        {
            Debug.DrawLine(transform.position, ray.point, Color.blue);

            if (Physics.Raycast(rb.transform.position - new Vector3(slopeRaycastSpread, 0, 0), Vector3.down, out ray2, slopeRaycastLength))
                slopeAngle = Mathf.Atan2(ray.point.y - ray2.point.y, ray.point.x - ray2.point.x) * 180 / Mathf.PI;

        }
        return slopeAngle;
        
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
        else if (!isJumping)
        {
            canMove = true;
        }
      
     
    }
    private void OnCollisionExit(Collision collision)
    {


    }
    private void OnCollisionStay(Collision collision)
    {
        if (isPlayerGrounded(collision))
            canMove = true;
        


    }

}

