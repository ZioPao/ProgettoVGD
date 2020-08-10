using System.Collections;
using System.Collections.Generic;
using Enemies;
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

            weaponScript.enabled = true;
            playerAnimations.enabled = true;
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
        }


        private void FixedUpdate()
        {
            //print(Values.GetRigidbody().velocity.magnitude);
            /*Manage movements*/

            if (!Values.GetIsGameOver())
            {
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
            else
            {
                print("game over: " + Values.GetIsGameOver());
            }
        }

        private void Update()
        {
            if (!Values.GetIsLoadingSave() && !Values.GetIsStartingNewGame() && !Values.GetIsChangingScene() && Values.GetIsWeaponControllerDoneLoading())
            {
                //SAVE
                if (Input.GetKeyDown(KeyCode.F5) && !Values.GetIsGameOver() && !Values.GetIsReadingSign() &&
                    Values.GetCanSave())
                {
                    GameObject saveManager;
                    if (Values.GetCurrentSaveManager() != null)
                    {
                        saveManager = Values.GetCurrentSaveManager();
                    }
                    else
                    {
                        saveManager = Instantiate(Resources.Load("Prefabs/SaveManager")) as GameObject;
                        DontDestroyOnLoad(saveManager);
                        Values.SetCurrentSaveManager(saveManager);
                    }


                    saveManager.GetComponent<SaveSystem>().Save();
                }

                //LOAD
                if (Input.GetKeyDown(KeyCode.F6) && !Values.GetIsReadingSign())
                {
                    GameObject saveManager;
                    //print("game over: " + Values.GetIsGameOver());
                    if (Values.GetCurrentSaveManager() != null)
                    {
                        saveManager = Values.GetCurrentSaveManager();
                    }
                    else
                    {
                        saveManager = Instantiate(Resources.Load("Prefabs/SaveManager")) as GameObject;
                        DontDestroyOnLoad(saveManager);
                        Values.SetCurrentSaveManager(saveManager);
                    }

                    //In caso di game over, reset tutto
                    Values.SetGameOver(false);
                    Time.timeScale = 1;
                    Values.SetHealth(1); //tmp per evitare che riparta il game over
                    Values.SetCanSave(true); //reset nel caso il player abbia caricato da dentro una boss battle

                    saveManager.GetComponent<SaveSystem>().Load();

                    //saveCanvas.enabled = false;
                }

                
                if (Input.GetKeyDown(KeyCode.F7))
                {
                    Values.SetHealth(0);
                }


                //Normal controller stuff
                weaponScript.UseWeapon();
                weaponScript.ChangeWeapon();
                interactionScript.Interact();
                interactionScript.SignBuffer();
            }
        }

        private void ManageHealth()
        {
            if (Values.GetHealth() <= 0 && !Values.GetIsLoadingSave() && !Values.GetIsGameOver())
            {
                //game over.
                Values.SetGameOver(true);
                Time.timeScale = 0; //za warudo, lo dovrebbe fare una sola volta
                return;
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
                Values.DecreaseStamina(Time.deltaTime * 9);
            }
            else if (Values.GetIsMoving())
            {
                Values.IncreaseStamina(Time.deltaTime * 8);
            }
            else
            {
                Values.IncreaseStamina(Time.deltaTime * 14);
            }
        }


        //Waiting methods
    }
}