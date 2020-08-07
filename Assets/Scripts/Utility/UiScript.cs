using System;
using Player;
using Saving;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Utility
{
    public class UiScript : MonoBehaviour
    {
        private GameObject oxygenCanvas,
            interactionCanvas,
            pickupCanvas,
            signCanvas,
            ammoCanvas,
            mainPauseCanvas,
            pauseCanvas,
            inGameSettingsCanvas,
            gameOverCanvas,
            tipsCanvas;

        private TMP_Dropdown resolutionDropdown;

        private Text healthString, staminaText, oxygenText, ammoText, signText, tipsText;
        private Image healthSprite, staminaSprite, oxygenSprite, ammoSprite;

        [SerializeField] private GameObject selectedOnPause;
        [SerializeField] private GameObject selectedOnOptionsOpen;
        [SerializeField] private GameObject selectedOnOptionsClose;

        void Start()
        {
            oxygenCanvas = GameObject.Find("Oxygen_Canvas");
            interactionCanvas = GameObject.Find("Interaction_Canvas");
            pickupCanvas = GameObject.Find("Pickup_Canvas");
            signCanvas = GameObject.Find("Sign_Canvas");
            ammoCanvas = GameObject.Find("Ammo_Canvas"); //todo credo sia inutile


            //health
            healthString = GameObject.Find("health_edit").GetComponent<Text>();
            healthSprite = GameObject.Find("health_sprite").GetComponent<Image>();

            //stamina
            staminaText = GameObject.Find("stamina_edit").GetComponent<Text>();
            staminaSprite = GameObject.Find("stamina_sprite").GetComponent<Image>();

            //oxygen
            oxygenText = GameObject.Find("oxygen_edit").GetComponent<Text>();
            oxygenSprite = GameObject.Find("oxygen_sprite").GetComponent<Image>();

            //ammo
            ammoText = GameObject.Find("ammo_edit").GetComponent<Text>();

            signText = GameObject.Find("sign_text").GetComponent<Text>();

            //Pause
            mainPauseCanvas = GameObject.Find("MainPause_Canvas");

            pauseCanvas = mainPauseCanvas.transform.Find("Pause_Canvas").gameObject;
            pauseCanvas.SetActive(false);
            inGameSettingsCanvas = mainPauseCanvas.transform.Find("Settings_Canvas").gameObject;
            inGameSettingsCanvas.SetActive(false);
            resolutionDropdown = inGameSettingsCanvas.transform.Find("Resolution_Dropdown").GetComponent<TMP_Dropdown>();

            mainPauseCanvas.SetActive(false);
            
            //Game over
            gameOverCanvas = GameObject.Find("GameOver_Canvas");
            gameOverCanvas.SetActive(false);
            
            //Tips
            tipsCanvas = GameObject.Find("Tips_Canvas");
            tipsText = tipsCanvas.GetComponentInChildren<Text>();
            tipsCanvas.SetActive(false);
        }

        // Update is called once per frame
        private void Update()
        {
            if (!Values.GetIsLoadingSave() && !Values.GetIsStartingNewGame() && !Values.GetIsChangingScene() && Values.GetWeaponBehaviours() != null)
            {
                if (Values.GetIsGameOver())
                {
                    gameOverCanvas.SetActive(true);
                }
                else
                {

                    SetHealthSprite(Values.GetHealth(), Values.GetMaxHealth());
                    SetStaminaSprite(Values.GetStamina(), Values.GetMaxStamina());

                    healthString.text = Mathf.RoundToInt(Player.Values.GetHealth()).ToString();
                    staminaText.text = Mathf.RoundToInt(Player.Values.GetStamina()).ToString();

                    if (!Values.GetWeaponBehaviours()[Values.GetCurrentWeapon()].GetIsMelee())
                    {
                        ammoCanvas.SetActive(true);
                        ammoText.text = Values.GetCurrentAmmo()[Values.GetCurrentWeapon()] + "/" +
                                        Values.GetAmmoReserve()[Values.GetCurrentWeapon()];
                    }
                    else
                    {
                        ammoCanvas.SetActive(false);
                    }

                    float oxygen = Values.GetOxygen();

                    if (oxygen < Values.GetMaxOxygen())
                    {
                        SetOxygenSprite(Values.GetOxygen(), Values.GetMaxOxygen());

                        oxygenCanvas.SetActive(true);
                        oxygenText.text = Mathf.RoundToInt(oxygen).ToString();
                    }
                    else
                    {
                        oxygenCanvas.SetActive(false);
                    }

                    interactionCanvas.SetActive(Values.GetIsNearInteractable() && !Values.GetIsInteracting());
                    pickupCanvas.SetActive(Values.GetIsNearPickup());
                    signCanvas.SetActive(Values.GetIsReadingSign());


                    if (Values.GetIsReadingSign())
                    {
                        signText.text = Values.GetCurrentSignController().GetSignText();
                        signText.alignment = TextAnchor.MiddleCenter;
                    }


                    /*Manage Pause*/
                    //in debug mode non funge correttamente, canPause va inizializzato dal menù o da un caricamento

                    if (Input.GetKeyDown(KeyCode.Escape) && Values.GetCanPause())
                    {
                        Values.SetIsInPause(!Values.GetIsInPause());
                        mainPauseCanvas.SetActive(Values.GetIsInPause());

                        if (Values.GetIsInPause())
                        {

                            GetIntoPause();
                        }
                        else
                        {
                            GoBackInGame();

                        }
                    }


                    if (Values.GetTip() != null && TimerController.GetCurrentTime()[TimerController.TIP_K] > 0)
                    {
                        TimerController.RunTimer(TimerController.TIP_K);
                        tipsCanvas.SetActive(true);
                        tipsText.text = Values.GetTip();
                    }
                    else
                    {
                        tipsText.text = "";
                        tipsCanvas.SetActive(false);
                    }
                 
                }
                
                
            }
        }



        /// <summary>
        /// Pause menu
        /// </summary>

        private void GetIntoPause()
        {
            //Clear button selection
            EventSystem.current.SetSelectedGameObject(null);

            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;        //permette di spostare il mouse nel menu
            pauseCanvas.SetActive(true);

            //Sets selected button
            EventSystem.current.SetSelectedGameObject(selectedOnPause);
        }

        public void SaveGame()
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

        public void LoadGame()
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
            Values.SetHealth(1);        //tmp per evitare che riparta il game over
            Values.SetCanSave(true);        //reset nel caso il player abbia caricato da dentro una boss battle
            Values.SetIsInPause(false);
            saveManager.GetComponent<SaveSystem>().Load();
        }
        private void GoBackInGame()
        {
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;        //permette di spostare il mouse nel menu
            pauseCanvas.SetActive(false);
            inGameSettingsCanvas.SetActive(false);
        }

        public void GetBackToMainMenu()
        {

            Cursor.lockState = CursorLockMode.Confined;        //permette di spostare il mouse nel menu
            
            SceneManager.MoveGameObjectToScene(Values.GetPlayerTransform().gameObject, SceneManager.GetActiveScene());
            StartCoroutine(SceneLoader.LoadScene("Scenes/MainMenu"));


            Values.SetIsChangingScene(false);        //Reistanzia correttamente il player dal main menu
            Values.SetIsInPause(false);        //Evita casini al reload
            Cursor.visible = true;
        }
        
        public void GoToInGameSettings()
        {

            //Clear button selection
            EventSystem.current.SetSelectedGameObject(null);

            pauseCanvas.SetActive(false);
            inGameSettingsCanvas.SetActive(true);
            
            //Aggiorna il dropdown
            Values.GetSettings().SetResolutionOptions(resolutionDropdown);

            //Sets selected button
            EventSystem.current.SetSelectedGameObject(selectedOnOptionsOpen);
        }

        /// <summary>
        /// In game Settings menu
        /// </summary>

        public void GetBackToPauseMenu()
        {
            //Clear button selection
            EventSystem.current.SetSelectedGameObject(null);

            inGameSettingsCanvas.SetActive(false);
            pauseCanvas.SetActive(true);

            //Sets selected button
            EventSystem.current.SetSelectedGameObject(selectedOnOptionsClose);
        }

        public void SetResolutionInGame(int index)
        {
            Values.GetSettings().SetChosenResolution(index);
        }
        
        
        private void SetHealthSprite(float currentHealth, float maxHealth)
        {

            float percentHealth;

            healthSprite.preserveAspect = true;

            percentHealth = (currentHealth / maxHealth) * 100;

            switch (percentHealth)
            {
                case float n when (n <= 100f && n > 90f):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 100");
                    break;
                case float n when (n <= 90f && n > 80f):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 090");
                    break;
                case float n when (n <= 80f && n > 70f):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 080");
                    break;
                case float n when (n <= 70f && n > 60f):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 070");
                    break;
                case float n when (n <= 60f && n > 50f):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 060");
                    break;
                case float n when (n <= 50f && n > 40f):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 050");
                    break;
                case float n when (n <= 40f && n > 30f):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 040");
                    break;
                case float n when (n <= 30f && n > 20f):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 030");
                    break;
                case float n when (n <= 20f && n > 10f):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 020");
                    break;
                case float n when (n <= 10f && n > 0f):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 010");
                    break;
                default:
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 000");
                    break;
            }
        }

        private void SetStaminaSprite(float currentStamina, float maxStamina)
        {

            float percentStamina;

            staminaSprite.preserveAspect = true;

            percentStamina = (currentStamina / maxStamina) * 100;

            switch (percentStamina)
            {
                case float n when (n <= 100f && n > 90f):
                    staminaSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/stamina/Stamina 100");
                    break;
                case float n when (n <= 90f && n > 80f):
                    staminaSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/stamina/Stamina 090");
                    break;
                case float n when (n <= 80f && n > 70f):
                    staminaSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/stamina/Stamina 080");
                    break;
                case float n when (n <= 70f && n > 60f):
                    staminaSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/stamina/Stamina 070");
                    break;
                case float n when (n <= 60f && n > 50f):
                    staminaSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/stamina/Stamina 060");
                    break;
                case float n when (n <= 50f && n > 40f):
                    staminaSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/stamina/Stamina 050");
                    break;
                case float n when (n <= 40f && n > 30f):
                    staminaSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/stamina/Stamina 040");
                    break;
                case float n when (n <= 30f && n > 20f):
                    staminaSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/stamina/Stamina 030");
                    break;
                case float n when (n <= 20f && n > 10f):
                    staminaSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/stamina/Stamina 020");
                    break;
                case float n when (n <= 10f && n > 0f):
                    staminaSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/stamina/Stamina 010");
                    break;
                default:
                    staminaSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/stamina/Stamina 000");
                    break;
            }
        }

        private void SetOxygenSprite(float currentOxygen, float maxOxygen)
        {

            float percentOxygen;

            oxygenSprite.preserveAspect = true;

            percentOxygen = (currentOxygen / maxOxygen) * 100;

            switch (percentOxygen)
            {
                case float n when (n <= 100f && n > 90f):
                    oxygenSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/oxygen/Oxygen 100");
                    break;
                case float n when (n <= 90f && n > 80f):
                    oxygenSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/oxygen/Oxygen 090");
                    break;
                case float n when (n <= 80f && n > 70f):
                    oxygenSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/oxygen/Oxygen 080");
                    break;
                case float n when (n <= 70f && n > 60f):
                    oxygenSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/oxygen/Oxygen 070");
                    break;
                case float n when (n <= 60f && n > 50f):
                    oxygenSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/oxygen/Oxygen 060");
                    break;
                case float n when (n <= 50f && n > 40f):
                    oxygenSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/oxygen/Oxygen 050");
                    break;
                case float n when (n <= 40f && n > 30f):
                    oxygenSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/oxygen/Oxygen 040");
                    break;
                case float n when (n <= 30f && n > 20f):
                    oxygenSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/oxygen/Oxygen 030");
                    break;
                case float n when (n <= 20f && n > 10f):
                    oxygenSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/oxygen/Oxygen 020");
                    break;
                case float n when (n <= 10f && n > 0f):
                    oxygenSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/oxygen/Oxygen 010");
                    break;
                default:
                    oxygenSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/oxygen/Oxygen 000");
                    break;
            }
        }
    }
}