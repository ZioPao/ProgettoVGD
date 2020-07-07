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
		    signParent = GameObject.Find("SignParent");
		    signScript = signParent.GetComponent<SignController>();
		    
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
                        switch (interactor.transform.parent.parent.name)
                        {
                            case "SignParent":
	                            signTemp = interactor.collider.GetComponentInParent<SignController>();
	                            signScript.SetCurrentSignID(signTemp.GetSignID());
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
						switch(picker.transform.parent.parent.name)
						{
							case "AmmoBoxParent":
								CollectAmmo();
								Destroy(picker.transform.parent.gameObject);
								break;
							
							case "MedKitParent":
								CollectMedKit();
								Destroy(picker.transform.parent.gameObject);
								break;
							
							case "KeyParent":
								CollectKey();
								Destroy(picker.transform.parent.gameObject);
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