using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        
        //Reimplementation of the original controller made by unity

        //if we're not touching anything, then jumping - no wasd
        //if we're touching something but on a slope - nerfed wasd
        
        private Rigidbody rb;
        private CameraMovement cameraScript;
        private GameObject cameraMain;
        private Vector3 movementVec;
        private CapsuleCollider collider;
        
		//Weapon types
		
		public enum WeaponEnum{
			
			Knife,
			Pistol,
			SMG,
			
		}

        //Weapons
        
        private Dictionary<WeaponEnum, GameObject> playerWeaponsObjects;
        private Dictionary<WeaponEnum, WeaponScript> playerWeaponsScripts;
        private Dictionary<WeaponEnum, bool> holdingWeapons;
        private WeaponEnum currentWeapon;
        
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();

            cameraScript = GetComponentInChildren<CameraMovement>();
            cameraMain = GameObject.Find("Camera_Main");
            collider = GetComponent<CapsuleCollider>();
            
            /*Setup basic stats*/

            Values.SetHealth(Values.GetMaxHealth());
            Values.SetStamina(Values.GetMaxStamina());
            Values.SetOxygen(Values.GetMaxOxygen());
            
            /*Setup armi*/
            playerWeaponsObjects = new Dictionary<WeaponEnum, GameObject>();
            playerWeaponsScripts = new Dictionary<WeaponEnum, WeaponScript>();
            holdingWeapons = new Dictionary<WeaponEnum, bool>();
            
            playerWeaponsObjects.Add(WeaponEnum.Pistol, GameObject.Find("PlayerPistol"));
            playerWeaponsObjects.Add(WeaponEnum.Knife, GameObject.Find("PlayerKnife"));

            playerWeaponsScripts.Add(WeaponEnum.Pistol, GameObject.Find("PlayerPistol").GetComponent<WeaponScript>());
            playerWeaponsScripts.Add(WeaponEnum.Knife, GameObject.Find("PlayerKnife").GetComponent<WeaponScript>());

            holdingWeapons.Add(WeaponEnum.Pistol, true);
            holdingWeapons.Add(WeaponEnum.Knife, true);
            holdingWeapons.Add(WeaponEnum.SMG, false);
            
            playerWeaponsObjects[WeaponEnum.Knife].SetActive(false);

            currentWeapon = WeaponEnum.Pistol;
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
			Pickup();
            
        }

        private void Update()
        {
            //viene esguito dopo il fixedupdate
            ShootControl();
			ChangeWeapon();

        }

        /** MOVEMENT 
     */
        private void SetupMovement()
        {
            float forwardMovement, rightMovement;
            float movementSpeedMod = Values.GetMovementSpeed();
            float slopeSpeedMultiplier = 1 - (GetSlopeAngle() / 90);

            /*In water*/
            if (Values.GetIsInWater())
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
            if (Input.GetKey(KeyCode.LeftShift) && !Values.GetIsTouchingWall() && !Values.GetIsTouchingWallWithHead() && (rb.velocity.magnitude > 0) && (axisMovementVertical > 0))
            {
                movementSpeedMod = Values.GetMovementSpeed() * Values.GetBoostSpeed();
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

                Values.SetIsRunning(shouldBeBoosting);
            }
            else
            {
                Values.SetIsRunning(false);
            }

            /*Setup vectors*/
            
            
            Vector3 forwardVec = transform.forward * (forwardMovement * slopeSpeedMultiplier);
            Vector3 rightVec = transform.right * (rightMovement * slopeSpeedMultiplier);
            movementVec = (forwardVec + rightVec);

        }

        private void MakeMovement()
        {

            if (Values.GetIsTouchingWallWithHead())
            {
                if (rb.position.y < Values.GetLastGoodYPosition())
                    rb.MovePosition(transform.position + (movementVec/4) * Time.fixedDeltaTime);    //lo rende talmente lento da farlo diventare un non problema
                else
                    rb.MovePosition(transform.position + movementVec * Time.fixedDeltaTime);


            }

            else
            {
                float slopeAngleTmp = GetSlopeAngle();
                
                //ignore check if player is in water
                if (slopeAngleTmp > -50 && slopeAngleTmp <= 35 || Values.GetIsInWater())
                {
                    rb.MovePosition(transform.position + movementVec * Time.fixedDeltaTime);
                }
                else
                {
                    //Fa scendere forzatamente il giocatore
                    print("stuck boy");
                    rb.MovePosition(transform.position + new Vector3(0, -9.81f, 0) * Time.deltaTime);
                }
            }

        }

        private void Jump()
     
        {
            if (Input.GetKey("space") && Values.GetStamina() >= 5 && (Values.GetIsGrounded() || Values.GetIsInWater()) && !Values.GetIsTouchingWallWithHead())
            {
                float jumpForceMod = Values.GetJumpForce();
                //Continue going towards that way
                
                //Decrease Stamina
                Values.SetStamina(Values.GetStamina()-1);

                if (Values.GetIsInWater())
                    jumpForceMod /= 5;

                Vector3 tmp = (transform.up * jumpForceMod);
                rb.AddForce(tmp, ForceMode.Force);
            }

          
        }


        private void ChangeWeapon()
        {
            
            //si deve determinare che armi possiede.
            
            //1 Knife
            if (Input.GetKeyDown("1") && holdingWeapons[WeaponEnum.Knife])
            {
                playerWeaponsObjects[WeaponEnum.Knife].SetActive(true);
                playerWeaponsObjects[WeaponEnum.Pistol].SetActive(false);
                playerWeaponsObjects[WeaponEnum.SMG].SetActive(false);

                currentWeapon = WeaponEnum.Knife;
            }

            
            //2 Pistola
            if (Input.GetKeyDown("2") && holdingWeapons[WeaponEnum.Pistol])
            {
                playerWeaponsObjects[WeaponEnum.Knife].SetActive(false);
                playerWeaponsObjects[WeaponEnum.Pistol].SetActive(true);
                playerWeaponsObjects[WeaponEnum.SMG].SetActive(false);
                
                currentWeapon = WeaponEnum.Pistol;

            }
            
            //3 SMG
            if (Input.GetKeyDown("3") &&  holdingWeapons[WeaponEnum.SMG])
            {
                playerWeaponsObjects[WeaponEnum.Knife].SetActive(false);
                playerWeaponsObjects[WeaponEnum.Pistol].SetActive(false);
                playerWeaponsObjects[WeaponEnum.SMG].SetActive(true);
                
                currentWeapon = WeaponEnum.SMG;

            }
        }

        private void AddWeapon(WeaponEnum weaponToAdd)
        {
            
            if (holdingWeapons.TryGetValue(weaponToAdd, out bool value))
            {
                if (!value)
                {
                    //Setta che l'arma è effettivamente in mano al player
                    holdingWeapons[weaponToAdd] = true;
                }
                else
                {
                    print("Arma gia presente e attiva");
                }
            }
        }
        
        /** Check e attivazione dello shooting*/
        private void ShootControl()
        {
            
            //Necessario inserirlo in Update, non FixedUpdate per via di come viene gestito il GetMouseButtonDown.
            //Usando il Fixed spesso perde input
            
            if (Input.GetMouseButtonDown(0) && !Values.GetIsRunning() && !Values.GetIsShooting())
            {
                Values.SetIsShooting(true);

                var weaponTmp = playerWeaponsScripts[currentWeapon];
                weaponTmp.ShootProjectile();
                if (Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out RaycastHit projectile, Values.GetProjectileDistance(), LayerMask.GetMask("EnemyHitbox")))
                {
                    Destroy(projectile.transform.parent.gameObject);
             
                }

            }
            else
                Values.SetIsShooting(false);        //todo forse da togliere

        }
	
        /** Check e attivazione dell'interazione
	*/
	
        private void Interact()
        {
            RaycastHit interactor;
            if (Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out interactor, Values.GetInteractionDistance()))
            {

                if (interactor.collider.CompareTag("Interactable"))
                {
                    //printa che puoi interagire
                    Values.SetIsNearInteractable(true);
                    if (Input.GetKey("e") && !Values.GetIsInteracting())
                    {
                        Values.SetIsInteracting(true);
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
                    Values.SetIsInteracting(false);
                }				

            }
            else
            {
                Values.SetIsNearInteractable(false);
                Values.SetIsInteracting(false);
            }
        }
		
		private void Pickup()
		{
			RaycastHit picker;
            if (Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out picker, Values.GetInteractionDistance()))
			{
				if(picker.collider.CompareTag("Pickup"))
				{
					Values.SetIsNearPickup(true);
					if (Input.GetKey("e"))
					{
						switch(picker.transform.parent.name)
						{
							case "AmmoBox":
								CollectAmmo();
								break;
							default:
								break;
							
						}
					}
				}
				else
				{
                    Values.SetIsNearPickup(false);
				}
			}
		}
		
        /*shows a new layer in the hud*/
        private void InteractWithSign()
        {

            Values.SetIsReadingSign(true);
     

            //todo se è troppo distante dal sign, si toglie il canvas?

            //todo blocca il player?


        }
		
		private void CollectAmmo(){
			
			//todo
			
		}
		
        private void ManageHealth() {

            if (Values.GetOxygen() < 1)
                Values.SetHealth(Values.GetHealth() - Time.deltaTime * 10);
                
        }

        private void ManageOxygen() {

            if (cameraScript.IsCameraUnderWater())
            {
                if (Mathf.RoundToInt(Values.GetOxygen()) > 0)
                    Values.SetOxygen(Values.GetOxygen() - Time.deltaTime * 2);
            }

            else if (Values.GetOxygen() < Values.GetMaxOxygen())
                Values.SetOxygen(Values.GetOxygen() + Time.deltaTime*5);


        }

        private void ManageStamina()
        {

            //todo adda che se è fermo la stamina torna molto più rapidamente
        

            //La stamina diminuisce solo quando effettivamente sta facenod l'animazione.
            //if (false)        //todo riaggiungi anim
                //stamina -= Time.deltaTime * 2;
            
            if (Values.GetStamina() < Values.GetMaxStamina())
                Values.SetStamina(Values.GetStamina() + Time.deltaTime * 5);

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

            Values.SetIsGrounded
            (
                Physics.Raycast(rb.transform.position, Vector3.down, out RaycastHit rayGround, 2)
            );     //todo determina l'altezza corretta

            LayerMask tmp = ~ LayerMask.GetMask("Enemy"); //ignore viewchecks for sprite management
            
            Values.SetIsTouchingWall
            (
                (Physics.Raycast(collider.transform.position + new Vector3(0, 0, Values.GetRaycastSpread()), collider.transform.forward, out _, 2, tmp)
                 || Physics.Raycast(collider.transform.position - new Vector3(0, 0, Values.GetRaycastSpread()), collider.transform.forward, out _, 2, tmp)
                 || Physics.Raycast(collider.transform.position, collider.transform.forward, out _, 2, tmp))
            );

            LayerMask layerTmp = ~ LayerMask.GetMask("Player");

            if (Values.GetIsTouchingWallWithHead())
            {
                Values.SetIsTouchingWallWithHead
                (
                    Physics.Raycast(collider.transform.position, collider.transform.up, out var ray, 2.5f, layerTmp)
                );
            }
            else
            {
                Values.SetIsTouchingWallWithHead
                (
                    Physics.Raycast(collider.transform.position, collider.transform.up, out var ray, 2.5f,layerTmp) //ignore viewchecks for sprite management
                );
                
                if (Values.GetIsTouchingWallWithHead())
                {
                    Values.SetLastGoodYPosition(rb.position.y);
                }
            }
   



        }

        private float GetSlopeAngle()
        {
            float slopeAngle = 0;       //base value

            if (Physics.Raycast(rb.transform.position + new Vector3(Values.GetRaycastSpread(), 0, 0), Vector3.down, out RaycastHit raySlope1, Values.GetRaycastLength()))
            {
                if (Physics.Raycast(rb.transform.position - new Vector3(Values.GetRaycastSpread(), 0, 0), Vector3.down, out RaycastHit raySlope2, Values.GetRaycastLength()))
                    slopeAngle = Mathf.Atan2(raySlope1.point.y - raySlope2.point.y, raySlope1.point.x - raySlope2.point.x) * 180 / Mathf.PI;
            }

            return slopeAngle;

        }


        private void OnTriggerEnter(Collider c)
        {

            if (c.gameObject.CompareTag("Water"))
                Values.SetIsInWater(true);

        }

        private void OnTriggerExit(Collider c)
        {
            if (c.gameObject.CompareTag("Water"))
                Values.SetIsInWater(false);
        }



        
        
        

        public bool IsPistolInHand()
        {
            return holdingWeapons[WeaponEnum.Pistol];
        }

        public bool IsKnifeInHand()
        {
            return holdingWeapons[WeaponEnum.Knife];
        }

        public GameObject GetPistol()
        {
            return playerWeaponsObjects[WeaponEnum.Pistol];
        }
        
        public WeaponScript GetPistolScript()
        {
            return playerWeaponsScripts[WeaponEnum.Pistol];
        }
    }
}