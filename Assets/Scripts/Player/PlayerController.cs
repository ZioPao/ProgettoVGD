﻿using System.Collections.Generic;
using Saving;
using UnityEngine;
using Utility;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        
        //Other Module Declarations
        private CameraScript cameraScript;

        private MovementController movementScript;
        private CollisionController collisionScript;
        private WeaponController weaponScript;
        private InteractionController interactionScript;
        
        // Start is called before the first frame update
        void Start()
        {
            
            /*Setup Collisions*/
            
            Values.SetRigidbody(GetComponent<Rigidbody>());
            Values.SetCollider(GetComponent<CapsuleCollider>());

            /*Setup Camera*/
            
            cameraScript = GetComponentInChildren<CameraScript>();
            
            /*Setup EnemySpritesManager*/
            Values.SetEnemySpritesManager(GetComponentInChildren<EnemySpritesManager>());
            
            /*Setup transform*/
            Values.SetPlayerTransform(this.transform);
            
            
            /*Setup Modules*/
            
            movementScript = GetComponent<MovementController>();
            collisionScript = GetComponent<CollisionController>();
            weaponScript = GetComponent<WeaponController>();
            interactionScript = GetComponent<InteractionController>();
            
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

            if (!Values.GetIsFrozen())
            {
                movementScript.SetupMovement();
                movementScript.Jump();
                movementScript.MakeMovement();
            }

            /*Manage stats*/
            
            ManageHealth();
            ManageOxygen();
            ManageStamina();
            
    

        }

        private void Update()
        {
            /*Manage Weapons*/
            //test stuff
            
            if (Input.GetKeyDown(KeyCode.F5))
            {
                new SaveSystem().Save();
            }

                        
            if (Input.GetKeyDown(KeyCode.F6))
            {
                new SaveSystem().Load();

            }
            if (!Values.GetIsFrozen())
            {
                weaponScript.UseWeapon();
                weaponScript.ChangeWeapon();
            }

            /* Manage actions*/

            interactionScript.Interact();
            interactionScript.Pickup();
            interactionScript.SignBuffer();
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