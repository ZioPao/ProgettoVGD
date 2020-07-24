﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Player
{
    public class InteractionController : MonoBehaviour
    {
	    
	    /*Camera Module*/
        
	    private GameObject cameraMain, signParent;
	    private SignController signScript, signTemp;
	    
	    private void Start()
	    {
		    /*Setup Camera*/
            
		    cameraMain = GameObject.Find("Camera_Main");
		    signParent = GameObject.Find("InteractableObjects");
		    try
		    {
			    signScript = signParent.GetComponent<SignController>();

		    }
		    catch (NullReferenceException)
		    {
			    ;
		    }
	    }


	    public void Interact()
        {
            RaycastHit interactor;
            if (Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out interactor, Values.GetInteractionDistance()))
            {
	            if (interactor.collider.CompareTag("Interactable"))
                {
	                Values.SetIsNearInteractable(true);
                    if (Input.GetKeyDown("e") && !Values.GetIsInteracting())
                    {
                        Values.SetIsInteracting(true);
                        //todo potenzialmente rotto con chest se fatte con un singolo modello. Da capire un po
                        switch (interactor.transform.name)
                        {
	                        //todo sta cosa coi parent parent parent non va bene. Da rifare il prefab del sign
                            case "SignParent":
	                            signTemp = interactor.collider.GetComponentInParent<SignController>();
	                            signScript.SetCurrentSignID(signTemp.GetSignID());
                                InteractWithSign();
                                break;
                            case "Door":
	                            InteractWithDoor();
	                            break;
                            case "LeverBoss":
	                            InteractWithLever();
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
		
		public void Pickup()
		{
			RaycastHit picker;
            if (Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out picker, Values.GetInteractionDistance()))
			{
				if(picker.collider.CompareTag("Pickup"))
				{
					Values.SetIsNearPickup(true);
					if (Input.GetKey("e"))
					{
						switch(picker.transform.name)
						{
							case "AmmoBox":
								CollectAmmo();
								break;
							
							case "MedKit":
								CollectMedKit();
								break;
							
							case "Key":
								CollectKey();
								break;
							
							default:
								break;
							
						}
						Destroy(picker.transform.gameObject);
					}
				}
				else
				{
                    Values.SetIsNearPickup(false);
				}
			}
		}
		
        /*Sign Methods*/
        
        public void SignBuffer()
        {
	        if ((Values.GetIsReadingSign()) && Input.GetKeyDown("q"))
	        {
		        Values.SetIsReadingSign(false);
		        Values.SetIsFrozen(false);
	        }
        }
        private void InteractWithSign()
        {
	        Values.SetIsReadingSign(true);
            Values.SetIsFrozen(true);
        }

        private void InteractWithDoor()
        {
	        Values.SetIsUsingDoor(true);
        }

        private void InteractWithLever()
        {
	        Values.SetIsUsingLever(true);
        }
        /*Pickup Methods*/
		
		private void CollectAmmo(){
			Values.IncrementAmmoReserve(Values.GetCurrentWeapon(),25);
		}

		private void CollectMedKit()
		{
			Values.IncreaseHealth(25);
		}

		private void CollectKey()
		{
			Values.SetHasKey(true);
		}
		

    }

}