﻿using System.Collections;
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

    [SerializeField] private float maxOxygen = 100f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float maxHealth = 100f;

    [SerializeField] private float rigidBodyDefaultMass = 2.45f;




    private Rigidbody rb;
    private Animator anim;
    private CameraMovement cameraScript;
    private GameObject cameraMain;

    private Vector3 movementVec;


    //Stats
    private float health;
    private float stamina;
    private float oxygen;

    //Various booleans
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isRunning = false;
    private bool isShooting = false;

    protected bool isInWater = false;
    


    //Raycasting
    float raycastLength = 5f;
    float raycastSpread = 0.08f;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        cameraScript = GetComponentInChildren<CameraMovement>();
        cameraMain = GameObject.Find("Camera_Main");

        /*Setup basic stats*/
        oxygen = maxOxygen;
        health = maxHealth;
        stamina = maxStamina;


    }

    private void FixedUpdate()
    {

        /*Manage movements*/
        CheckCollisions();
        GetMovement();
        Jump();
        rb.MovePosition(transform.position + movementVec * Time.fixedDeltaTime);


        /*Manage stats*/
        ManageHealth();
        ManageOxygen();
        ManageStamina();

        /* Manage actions*/
        Shoot();

        /*Additional stuff*/
        SetAnimations();


    }


    /** MOVEMENT 
     */
    private void GetMovement()
    {
        float forwardMovement, rightMovement;
        float movementSpeedMod = movementSpeed;
        float slopeSpeedMultiplier = 1 - (GetSlopeAngle() / 90);

        /*Boost*/
        if (Input.GetKey(KeyCode.LeftShift) && !isTouchingWall)
            movementSpeedMod = SetBoost();
        else
            isRunning = false;

        /*In water*/
        if (isInWater)
        {
            movementSpeedMod *= 0.5f; //Decrease
            //rb.mass = rigidBodyDefaultMass + 15f;
        }


        

        /*Get movement*/
        forwardMovement = Input.GetAxis("Vertical") * movementSpeedMod;
        rightMovement = Input.GetAxis("Horizontal") * movementSpeedMod;

        /*Fix diagonal speed*/
        if (forwardMovement != 0 && rightMovement != 0)
        {
            forwardMovement /= 1.42f;       //todo determinare se è sempre questo valore
            rightMovement /= 1.42f;
        }

        /*Setup vectors*/
        Vector3 forwardVec = transform.forward * forwardMovement * slopeSpeedMultiplier;
        Vector3 rightVec = transform.right * rightMovement * slopeSpeedMultiplier;
        movementVec = (forwardVec + rightVec);

        
    }

    private float SetBoost()
    {
        isRunning = true;
        return movementSpeed* boostSpeed;
    }

    private void Jump()
     
    {
        if (Input.GetKey("space") && stamina >= 5 && (isGrounded || isInWater))
        {
            float jumpForceMod = jumpForce;
            //Continue going towards that wayì
            stamina -= 1;       //decrease stamina

            if (isInWater)
                jumpForceMod /= 5;

            Vector3 tmp = (transform.up * jumpForceMod);
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
            if (Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out projectile, 100))
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

    private void ManageHealth() {

        if (oxygen < 1)
            health -= Time.deltaTime * 10;
    }

    private void ManageOxygen() {

        if (cameraScript.IsCameraUnderWater())
        {
            if (Mathf.RoundToInt(oxygen) > 0)
                oxygen -= Time.deltaTime * 2;
        }

        else if (oxygen < maxOxygen)
                oxygen += Time.deltaTime*5;
        

    }

    private void ManageStamina() {

        //todo adda che se è fermo la stamina torna molto più rapidamente

        if (isRunning)
            stamina -= Time.deltaTime * 2;
        else if (stamina < maxStamina)
            stamina += Time.deltaTime * 5;
   
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

    /// <summary>
    /// Collisions
    /// </summary>
    
    /**
     * setup some variables to determine if the player is grounded or not. In which case, checks
     * or not the slope angle */
    private void CheckCollisions()
    {
        //check ground
        isGrounded = Physics.Raycast
            (rb.transform.position, Vector3.down, out RaycastHit rayGround, 2);     //todo determina l'altezza corretta

        RaycastHit rayWall1 = new RaycastHit();
        RaycastHit rayWall2 = new RaycastHit();
        RaycastHit rayWall3 = new RaycastHit();

        isTouchingWall =
            (Physics.Raycast(cameraMain.transform.position + new Vector3(0, 0, raycastSpread), cameraMain.transform.forward, out rayWall1, 2)
            || Physics.Raycast(cameraMain.transform.position - new Vector3(0, 0, raycastSpread), cameraMain.transform.forward, out rayWall2, 2)
            || Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out rayWall3, 2));
         

    }

    private float GetSlopeAngle()
    {
        float slopeAngle = 0;       //base value

        if (Physics.Raycast(rb.transform.position + new Vector3(raycastSpread, 0, 0), Vector3.down, out RaycastHit raySlope1, raycastLength))
        {
            if (Physics.Raycast(rb.transform.position - new Vector3(raycastSpread, 0, 0), Vector3.down, out RaycastHit raySlope2, raycastLength))
                slopeAngle = Mathf.Atan2(raySlope1.point.y - raySlope2.point.y, raySlope1.point.x - raySlope2.point.x) * 180 / Mathf.PI;
        }
        return slopeAngle;

    }


    private void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.CompareTag("Water"))
            isInWater = true;
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Water"))
            isInWater = false;
    }


    /*GETTERS*/

    public bool IsInWater()
    {
        return isInWater;
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetStamina()
    {
        return stamina;
    }

    public float GetOxygen()
    {
        return oxygen;
    }

    public float GetMaxOxygen()
    {
        return maxOxygen;
    }
}