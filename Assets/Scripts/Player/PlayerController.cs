using System.Collections;
using System.Collections.Generic;
using Saving;
using UnityEditor;
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


        private const string TmpSaveObject = "TmpSaveObject";

        void Awake()
        {
            /*Setup Collisions*/

            Values.SetRigidbody(GetComponent<Rigidbody>());
            Values.SetCollider(GetComponent<CapsuleCollider>());

            /*Setup Camera*/

            cameraScript = GetComponentInChildren<CameraScript>();

            /*Setup EnemySpritesManager*/
            Values.SetEnemySpritesManager(GetComponentInChildren<EnemySpritesManager>());

            /*Setup transform*/
            Values.SetPlayerTransform(transform);


            /*Setup Modules*/

            movementScript = GetComponent<MovementController>();
            collisionScript = GetComponent<CollisionController>();
            weaponScript = GetComponent<WeaponController>();
            interactionScript = GetComponent<InteractionController>();

            var playerAnimations = GetComponentInChildren<PlayerAnimations>();
            var enemySpritesManager = GetComponentInChildren<EnemySpritesManager>();
            var guiController = GetComponentInChildren<UiScript>();

            playerAnimations.enabled = true;
            weaponScript.enabled = true;
            interactionScript.enabled = true;
            enemySpritesManager.enabled = true;
            guiController.enabled = true;


            /*Setup basic stats*/
            if (!Values.GetIsChangingScene())
            {
                Values.SetHealth(Values.GetMaxHealth());
                Values.SetStamina(Values.GetMaxStamina());
                Values.SetOxygen(Values.GetMaxOxygen());

                /*Setup Timer*/

                TimerController.Setup();
            }


            //allo spawn del player si attivano gli spawner

            foreach (var x in GameObject.FindGameObjectsWithTag("Spawner"))
            {
                x.GetComponent<EnemySpawner>().enabled = true;
            }
        }


        private void FixedUpdate()
        {
            //print(Values.GetRigidbody().velocity.magnitude);
            /*Manage movements*/
            collisionScript.CheckCollisions();

            if (!Values.GetIsFrozen() || Values.GetIsInPause())
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

            if (Input.GetKeyDown(KeyCode.F5) && !Values.GetIsGameOver())
            {
                GameObject saveManager;
                if (Values.GetCurrentSaveManager() != null)
                {
                    saveManager = Values.GetCurrentSaveManager();
                }
                else
                {
                    saveManager = PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/SaveManager")) as GameObject;
                    DontDestroyOnLoad(saveManager);
                    Values.SetCurrentSaveManager(saveManager);
                }

                saveManager.GetComponent<SaveSystem>().Save();
            }


            if (Input.GetKeyDown(KeyCode.F6))
            {
                GameObject saveManager;
                if (Values.GetCurrentSaveManager() != null)
                {
                    saveManager = Values.GetCurrentSaveManager();
                }
                else
                {
                    saveManager = PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/SaveManager")) as GameObject;
                    DontDestroyOnLoad(saveManager);
                    Values.SetCurrentSaveManager(saveManager);
                }

                saveManager.GetComponent<SaveSystem>().Load();
            }

            // if (Input.GetKeyDown(KeyCode.F7))
            // {
            //     var interactableObject = GameObject.Find("Lever");
            //     interactableObject.GetComponent<LeverScript>().ForceActivation();
            //
            //
            // }

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


        private void ManageHealth()
        {
            if (Values.GetHealth() <= 0)
            {
                //game over.
                Values.SetGameOver(true);
            }


            if (Values.GetOxygen() < 1)
            {
                Values.DecreaseHealth(Time.deltaTime * 10);
            }
        }

        private void ManageOxygen()
        {
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
                Values.DecreaseStamina(Time.deltaTime * 10);
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