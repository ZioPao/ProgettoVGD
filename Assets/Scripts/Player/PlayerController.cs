using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        
        //Reimplementation of the original controller made by unity

        //if we're not touching anything, then jumping - no wasd
        //if we're touching something but on a slope - nerfed wasd
        
        private CameraMovement cameraScript;
        private GameObject cameraMain;
        
        private MovementController movementScript;
        private CollisionController collisionScript;
        
        
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
            Values.SetRigidbody(GetComponent<Rigidbody>());
            Values.SetCollider(GetComponent<CapsuleCollider>());

            cameraScript = GetComponentInChildren<CameraMovement>();
            cameraMain = GameObject.Find("Camera_Main");
            
            movementScript = GetComponent<MovementController>();
            collisionScript = GetComponent<CollisionController>();


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
            collisionScript.CheckCollisions();
            movementScript.SetupMovement();
            movementScript.Jump();
            movementScript.MakeMovement();

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
            {
                Values.DecreaseHealth(Time.deltaTime * 10);
            }

        }

        private void ManageOxygen() {

            if (cameraScript.IsCameraUnderWater())
            {
                Values.DecreaseOxygen(Time.deltaTime * 2);
            }
            else
            {
                Values.IncreaseOxygen(Time.deltaTime * 5);
            }
            
        }

        private void ManageStamina()
        {

            if (Values.GetIsRunning())
            {
                Values.DecreaseStamina(Time.deltaTime * 5);
            }
            else if (Values.GetIsMoving())
            {
                Values.IncreaseStamina(Time.deltaTime * 3);
            }
            else
            {
                Values.IncreaseStamina(Time.deltaTime * 8);
            }

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