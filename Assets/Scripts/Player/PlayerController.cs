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
        private WeaponController weaponScript;
        
        // Start is called before the first frame update
        void Start()
        {
            Values.SetRigidbody(GetComponent<Rigidbody>());
            Values.SetCollider(GetComponent<CapsuleCollider>());

            cameraScript = GetComponentInChildren<CameraMovement>();
            cameraMain = GameObject.Find("Camera_Main");
            
            movementScript = GetComponent<MovementController>();
            collisionScript = GetComponent<CollisionController>();
            weaponScript = GetComponent<WeaponController>();


            /*Setup basic stats*/

            Values.SetHealth(Values.GetMaxHealth());
            Values.SetStamina(Values.GetMaxStamina());
            Values.SetOxygen(Values.GetMaxOxygen());
            
            /*Setup Timer*/
            Utility.TimerController.Setup();

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
            weaponScript.UseWeapon();
			weaponScript.ChangeWeapon();
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
                    if (Input.GetKeyDown("e") && !Values.GetIsInteracting())
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

    }
}