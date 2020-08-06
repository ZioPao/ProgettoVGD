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
        private Image healthSprite;

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
            healthString = GameObject.Find("health_text").GetComponent<Text>();
            healthSprite = GameObject.Find("health_sprite").GetComponent<Image>();

            staminaText = GameObject.Find("stamina_edit").GetComponent<Text>();
            oxygenText = GameObject.Find("oxygen_edit").GetComponent<Text>();
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
                    int currentHealth = Mathf.RoundToInt(Player.Values.GetHealth());
                    SetHealthSprite(currentHealth);

                    healthString.text = currentHealth.ToString();
                    staminaText.text = Mathf.RoundToInt(Player.Values.GetStamina()).ToString();

                    //Prende l'arma attiva al momento
                    //todo still pesante


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
        
        
        private void SetHealthSprite(int health)
        {
            healthSprite.preserveAspect = true;

            switch (health)
            {
                case int n when (n >= 100):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 100");
                    break;
                case int n when (n < 90 && n >= 80):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 090");
                    break;
                case int n when (n < 80 && n >= 70):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 080");
                    break;
                case int n when (n < 70 && n >= 60):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 070");
                    break;
                case int n when (n < 60 && n >= 50):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 060");
                    break;
                case int n when (n < 50 && n >= 40):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 050");
                    break;
                case int n when (n < 40 && n >= 30):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 040");
                    break;
                case int n when (n < 30 && n >= 20):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 030");
                    break;
                case int n when (n < 20 && n >= 10):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 020");
                    break;
                case int n when (n < 10 && n >= 0):
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 010");
                    break;
                default:
                    healthSprite.sprite = Resources.Load<Sprite>("Common/Textures/GUI/health/Health 000");
                    break;
            }
        }
    }
}