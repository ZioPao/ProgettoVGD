using UnityEngine;

namespace Logic.Player
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
        private bool isInteracting = false;
        protected bool isNearInteractable = false;
        protected bool isReadingSign = false;

        protected bool isInWater = false;
        protected bool isTouchingWallWithHead = false;

        float lastGoodYPosition;

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
            MakeMovement();


            /*Manage stats*/
            ManageHealth();
            ManageOxygen();
            ManageStamina();

            /* Manage actions*/
            Shoot();
            Interact();

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

            /*In water*/
            if (isInWater)
            {
                movementSpeedMod *= 0.5f; //Decrease
                //rb.mass = rigidBodyDefaultMass + 15f;
            }

            /*Get movement*/
            float axisMovementVertical = Input.GetAxis("Vertical");
            float axisMovementHorizontal = Input.GetAxis("Horizontal");
            
            /*Boost*/
            if (Input.GetKey(KeyCode.LeftShift) && !isTouchingWall && !isTouchingWallWithHead && (rb.velocity.magnitude > 0) && (axisMovementVertical > 0))
            {
                movementSpeedMod = SetBoost();

            }
            else
            {
                isRunning = false;

            }
            forwardMovement = axisMovementVertical * movementSpeedMod;
            rightMovement = axisMovementHorizontal  * movementSpeedMod;


            /*Fix diagonal speed*/
            if (forwardMovement != 0 && rightMovement != 0)
            {
                forwardMovement /= 1.42f;       //todo determinare se è sempre questo valore
                rightMovement /= 1.42f;
            }

            /*Setup vectors*/
            
            
            Vector3 forwardVec = transform.forward * (forwardMovement * slopeSpeedMultiplier);
            Vector3 rightVec = transform.right * (rightMovement * slopeSpeedMultiplier);
            movementVec = (forwardVec + rightVec);

        
        }

        private void MakeMovement()
        {

            /*If touching wall with head*/
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
                
                if (slopeAngleTmp > -50 && slopeAngleTmp <= 15)
                    rb.MovePosition(transform.position + movementVec * Time.fixedDeltaTime);
                else
                {
                    print(slopeAngleTmp);
                }
            }

        }
        private float SetBoost()
        {
            isRunning = true;
            return movementSpeed* boostSpeed;
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
        /** Check e attivazione dello shooting
     */
   
        private void Shoot()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit projectile;
                isShooting = true;
                if (Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out projectile, projectileDistance, LayerMask.GetMask("EnemyHitbox")))
                {
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

        private void ManageStamina() {

            //todo adda che se è fermo la stamina torna molto più rapidamente
        

            //La stamina diminuisce solo quando effettivamente sta facenod l'animazione.
            if (anim.GetBool("isRunning"))
                stamina -= Time.deltaTime * 2;
            else if (stamina < maxStamina)
                stamina += Time.deltaTime * 5;
   
        }

        private void SetAnimations()
        {
            if (isRunning)
            {
                if (movementVec.x != 0 || movementVec.z != 0)
                    anim.SetBool("isRunning", true);
                else
                    anim.SetBool("isRunning", false);
            
            }
            else
                anim.SetBool("isRunning", false);

        
            anim.SetBool("isShooting", isShooting);
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


            RaycastHit rayHead = new RaycastHit();


            if (isTouchingWallWithHead)
            {
                isTouchingWallWithHead = Physics.Raycast(cameraMain.transform.position, cameraMain.transform.up, out rayHead, 2.5f);

            }
            else
            {
                isTouchingWallWithHead = Physics.Raycast(cameraMain.transform.position, cameraMain.transform.up, out rayHead, 2.5f);
                if (isTouchingWallWithHead)
                    lastGoodYPosition = rb.position.y;        //todo what should this do?
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

        public bool IsPlayerNearInteractable()
        {
            return isNearInteractable;
        }

        public bool IsPlayerReadingSign()
        {
            return isReadingSign;
        }
    }
}