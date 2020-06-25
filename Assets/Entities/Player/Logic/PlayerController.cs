using System;
using Logic.Player;
using UnityEngine;

namespace Entities.Player.Logic
{
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
	
        [SerializeField] private float projectileDistance = 100f;
        [SerializeField] private float interactionDistance = 5f;
        


        private Rigidbody rb;
        private CameraMovement cameraScript;
        private GameObject cameraMain;
        private Vector3 movementVec;


        //Stats
        private float health;
        private float stamina;
        private float oxygen;
        private bool isHoldingKnife, isHoldingPistol, isHoldingSmg;
        

        //Various booleans
        private bool isGrounded;
        private bool isTouchingWall;
        private bool isRunning = false;
        private bool isShooting = false;
        private bool isInteracting = false;
        private bool isNearInteractable = false;
        private bool isReadingSign = false;

        private bool isInWater = false;
        private bool isTouchingWallWithHead = false;

        float lastGoodYPosition;

        //Raycasting
        float raycastLength = 5f;
        float raycastSpread = 0.08f;



        
        //Weapons

        private GameObject weaponPistol, weaponKnife, weaponSmg;
        
        
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();

            cameraScript = GetComponentInChildren<CameraMovement>();
            cameraMain = GameObject.Find("Camera_Main");

            /*Setup basic stats*/
            oxygen = maxOxygen;
            health = maxHealth;
            stamina = maxStamina;
            
            
            //TEST
            isHoldingKnife = false;
            isHoldingPistol = false;
            isHoldingSmg = false;

            AddWeapon("PlayerPistol");
            AddWeapon("PlayerKnife");
            
            weaponKnife.SetActive(false);


        }

        private void FixedUpdate()
        {

            /*Manage movements*/
            CheckCollisions();
            SetupMovement();
            Jump();
            MakeMovement();


            /*Manage stats*/
            ManageHealth();
            ManageOxygen();
            ManageStamina();

            /* Manage actions*/
            Interact();
            ChangeWeapon();



        }

        private void Update()
        {
            //viene esguito dopo il fixedupdate
            Shoot();

        }

        /** MOVEMENT 
     */
        private void SetupMovement()
        {
            float forwardMovement, rightMovement;
            float movementSpeedMod = movementSpeed;
            float slopeSpeedMultiplier = 1 - (GetSlopeAngle() / 90);

            /*In water*/
            if (isInWater)
            {
                //todo i broke something 
                movementSpeedMod *= 0.5f; //Decrease
                //rb.mass = rigidBodyDefaultMass + 15f;
            }

            /*Get movement*/
            float axisMovementVertical = Input.GetAxis("Vertical");
            float axisMovementHorizontal = Input.GetAxis("Horizontal");

            /*Boost*/
            bool shouldBeBoosting;
            if (Input.GetKey(KeyCode.LeftShift) && !isTouchingWall && !isTouchingWallWithHead && (rb.velocity.magnitude > 0) && (axisMovementVertical > 0))
            {
                movementSpeedMod = movementSpeed* boostSpeed;
                shouldBeBoosting = true;

            }
            else
            {
                shouldBeBoosting = false;
            }
            
      
            forwardMovement = axisMovementVertical * movementSpeedMod;
            rightMovement = axisMovementHorizontal  * movementSpeedMod;


            /*Fix diagonal speed and check if player's boosting*/
            if (forwardMovement != 0 && rightMovement != 0)
            {
                forwardMovement /= 1.42f;       //todo determinare se è sempre questo valore
                rightMovement /= 1.42f;

                isRunning = shouldBeBoosting;
            }
            else
            {
                isRunning = false;
            }

            /*Setup vectors*/
            
            
            Vector3 forwardVec = transform.forward * (forwardMovement * slopeSpeedMultiplier);
            Vector3 rightVec = transform.right * (rightMovement * slopeSpeedMultiplier);
            movementVec = (forwardVec + rightVec);

        }

        private void MakeMovement()
        {

            if (isTouchingWallWithHead)
            {
                if (rb.position.y < lastGoodYPosition)
                    rb.MovePosition(transform.position + (movementVec/4) * Time.fixedDeltaTime);    //lo rende talmente lento da farlo diventare un non problema
                else
                    rb.MovePosition(transform.position + movementVec * Time.fixedDeltaTime);


            }

            else
            {
                float slopeAngleTmp = GetSlopeAngle();
                
                //ignore check if player is in water
                if (slopeAngleTmp > -50 && slopeAngleTmp <= 30 || isInWater)
                {
                    rb.MovePosition(transform.position + movementVec * Time.fixedDeltaTime);
                }
                else
                {
                    //Fa scendere forzatamente il giocatore
                    rb.MovePosition(transform.position + new Vector3(0, -9.81f, 0) * Time.deltaTime);
                }
            }

        }

        private void Jump()
     
        {
            if (Input.GetKey("space") && stamina >= 5 && (isGrounded || isInWater) && !isTouchingWallWithHead)
            {
                float jumpForceMod = jumpForce;
                //Continue going towards that way
                stamina -= 1;       //decrease stamina

                if (isInWater)
                    jumpForceMod /= 5;

                Vector3 tmp = (transform.up * jumpForceMod);
                rb.AddForce(tmp, ForceMode.Force);
            }

          
        }


        private void ChangeWeapon()
        {
            
            //si deve determinare che armi possiede.
            
            //1 Knife
            if (Input.GetKeyDown("1"))
            {
                weaponKnife.SetActive(isHoldingKnife);
                
                weaponPistol.SetActive(false);
                weaponSmg.SetActive(false);

            }

            
            //2 Pistola
            if (Input.GetKeyDown("2"))
            {
                weaponPistol.SetActive(isHoldingPistol);
                
                weaponKnife.SetActive(false);
                weaponSmg.SetActive(false);

            }
            
            //3 SMG
            if (Input.GetKeyDown("3"))
            {
                weaponSmg.SetActive(isHoldingSmg);
                
                weaponPistol.SetActive(false);
                weaponKnife.SetActive(false);

            }

        }

        private void AddWeapon(String weapon)
        {
            
            //should use an enum or something
            switch (weapon)
            {
                case "PlayerPistol":

                    if (!isHoldingPistol)
                    {
                        //add it
                        weaponPistol = GameObject.Find("PlayerPistol");
                        isHoldingPistol = true;

                    }
                    

                    break;
                case "PlayerKnife":

                    if (!isHoldingKnife)
                    {
                        //add it
                        weaponKnife = GameObject.Find("PlayerKnife");

                        isHoldingKnife = true;

                    }
                    break;
                case "PlayerSmg":

                    if (!isHoldingSmg)
                    {
                        //add it
                        isHoldingSmg = true;
                    }
                    break;
                default:
                    break;
            }
        }
        
        /** Check e attivazione dello shooting*/
        private void Shoot()
        {
            
            //Necessario inserirlo in Update, non FixedUpdate per via di come viene gestito il GetMouseButtonDown.
            //Usando il Fixed spesso perde input
            
            if (Input.GetMouseButtonDown(0) && !isRunning)
            {
                isShooting = true;
                if (Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out RaycastHit projectile, projectileDistance, LayerMask.GetMask("EnemyHitbox")))
                {
                    Destroy(projectile.transform.parent.gameObject);
                    
                    //ParticleSystem exp = GetComponent<ParticleSystem>();
                    //exp.Play();
                    //when it hits something, it destroys it
                    //print(projectile.transform);
                }

            }
            else
                isShooting = false;        //todo forse da togliere

        }
	
        /** Check e attivazione dell'interazione
	*/
	
        private void Interact()
        {
            RaycastHit interactor;
            if (Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out interactor, interactionDistance))
            {

                if (interactor.collider.CompareTag("Interactable"))
                {
                    //printa che puoi interagire
                    isNearInteractable = true;
                    if (Input.GetKey("e") && !isInteracting)
                    {
                        isInteracting = true;
                        //todo potenzialmente rotto con chest se fatte con un singolo modello. Da capire un po
                        switch (interactor.transform.parent.name)
                        {
                            case "Sign":
                                InteractWithSign();
                                break;
                            case "Chest":
                                //InteractWithObject("Chest");
                                break;
                            default:
                                break;
                        }
                    }

                }
                else
                {
                    isInteracting = false;
                }

            }
            else
            {

                isNearInteractable = false;
                isInteracting = false;
            }
        }
        /*shows a new layer in the hud*/
        private void InteractWithSign()
        {

            isReadingSign = true;
        

            //todo se è troppo distante dal sign, si toglie il canvas?

            //todo blocca il player?


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

        private void ManageStamina()
        {

            //todo adda che se è fermo la stamina torna molto più rapidamente
        

            //La stamina diminuisce solo quando effettivamente sta facenod l'animazione.
            if (false)        //todo riaggiungi anim
                stamina -= Time.deltaTime * 2;
            
            if (stamina < maxStamina)
                stamina += Time.deltaTime * 5;

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

            LayerMask tmp = ~ LayerMask.GetMask("Enemy"); //ignore viewchecks for sprite management
            isTouchingWall =
                (Physics.Raycast(cameraMain.transform.position + new Vector3(0, 0, raycastSpread), cameraMain.transform.forward, out _, 2, tmp)
                 || Physics.Raycast(cameraMain.transform.position - new Vector3(0, 0, raycastSpread), cameraMain.transform.forward, out _, 2, tmp)
                 || Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out _, 2, tmp));


            if (isTouchingWallWithHead)
            {
                isTouchingWallWithHead = Physics.Raycast(cameraMain.transform.position, cameraMain.transform.up, out _, 2.5f);

            }
            else
            {
                isTouchingWallWithHead = Physics.Raycast(cameraMain.transform.position, cameraMain.transform.up, out _, 2.5f);
                if (isTouchingWallWithHead)
                    lastGoodYPosition = rb.position.y;    
            }
   



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


        private void OnTriggerEnter(Collider c)
        {

            if (c.gameObject.CompareTag("Water"))
                isInWater = true;
        }

        private void OnTriggerExit(Collider c)
        {
            if (c.gameObject.CompareTag("Water"))
                isInWater = false;
        }


        /*SETTER*/

        public void SetIsPlayerShooting(bool value)
        {
            isShooting = value;
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

        public bool IsPlayerNearInteractable()
        {
            return isNearInteractable;
        }

        public bool IsPlayerReadingSign()
        {
            return isReadingSign;
        }

        public bool IsPlayerShooting()
        {
            return isShooting;
        }

        public bool IsPlayerRunning()
        {
            return isRunning;
        }

        public bool IsPistolInHand()
        {
            return isHoldingPistol;
        }

        public bool IsKnifeInHand()
        {
            return isHoldingKnife;
        }

        public GameObject GetPistol()
        {
            return weaponPistol;
        }
    }
}