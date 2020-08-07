using System;
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
        private SignController signTemp;

        public void Awake()
        {
            /*Setup Camera*/

            cameraMain = GameObject.Find("Camera_Main");
            signParent = GameObject.Find("Signs");
        }


        public void Interact()
        {
            RaycastHit interactor;
            if (Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out interactor,
                Values.GetInteractionDistance()))
            {
                if (interactor.collider.CompareTag("Interactable") || interactor.collider.CompareTag("InteractableSign"))
                {
                    Values.SetIsNearInteractable(true);
                    if (Input.GetKeyDown("e") && !Values.GetIsInteracting())
                    {
                        Values.SetIsInteracting(true);
                        interactor.collider.GetComponent<IInteractableMidGame>().InteractableBehaviour();
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
            if (Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out picker,
                Values.GetInteractionDistance()))
            {
                if (picker.collider.CompareTag("Pickup"))
                {
                    Values.SetIsNearPickup(true);
                    if (Input.GetKey("e"))
                    {
                        switch (picker.transform.name)
                        {
                            case "AmmoBox":
                                CollectAmmo();
                                Destroy(picker.transform.gameObject);
                                break;

                            case "Key":
                                CollectKey();
                                Destroy(picker.transform.gameObject);
                                break;

                            default:
                                break;
                        }

                        //Play Audio
                        Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.CollectiblePickup);
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
                Values.SetCanPause(true);
                Values.SetIsFrozen(false);


                //Check ulteriore per la boss battle finale
                if (GameObject.FindGameObjectWithTag("Level").name.Equals("Level3") &&
                    Values.GetCurrentSignController().GetCurrentSignID() == 2)
                {
                    Values.SetHasInteractedWithWinObject(true);
                }
            }
        }

        /*Pickup Methods*/

        private void CollectAmmo()
        {
            Values.IncrementAmmoReserve(Values.GetCurrentWeapon(), 25);
        }

        private void CollectKey()
        {
            Values.SetHasKey(true);
        }
    }
}