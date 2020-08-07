using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Saving;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;
using UnityEngine.EventSystems;

namespace Menu
{
    public class MenuController : MonoBehaviour
    {

        [SerializeField] private GameObject mainMenu, selectMenu, optionsMenu;

        [SerializeField] private TMP_Dropdown resolutionDropdown;

        [SerializeField] private GameObject selectedOnStart;
        [SerializeField] private GameObject selectedOnLevelSelectionOpen;
        [SerializeField] private GameObject selectedOnLevelSelectionClose;
        [SerializeField] private GameObject selectedOnOptionsOpen;
        [SerializeField] private GameObject selectedOnOptionsClose;
        
        public void Start()
        {
            Audio.SoundManager.InitializeSoundEffects();
            Audio.SoundManager.InitializeSoundPlayer();
            Audio.SoundManager.InitializeSoundTracks();
            Audio.SoundManager.InitializeMusicPlayer();
            Audio.SoundManager.PlaySoundtrack(Audio.SoundManager.SoundTracks.TitleTrack);
            
            Values.SetIsInPause(false);        //Evita casini al reload
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            var settings = new SettingsScript();
            Values.SetSettings(settings);
            settings.Init();
            settings.SetResolutionOptions(resolutionDropdown);

            //Sets selected button
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(selectedOnStart);
        }

        public void StartNewGame()
        {
            mainMenu.SetActive(false);
            Values.SetIsStartingNewGame(true);
            Values.SetIsWeaponControllerDoneLoading(false);        //todo aggiungi nel load level del sceneloader
            LoadLevel(1);
            //StartCoroutine(SceneLoader.LoadWeapons(Values.WeaponEnum.Pistol, true));

            //Values.AddHeldWeapon(Values.WeaponEnum.Knife, true);
        }

        public void LoadGame()
        {
            print("caricamento da menu");

            GetComponentInParent<Canvas>().enabled = false;

            GameObject saveManager;
            if (!Values.GetCurrentSaveManager())
            {
                saveManager = Instantiate(Resources.Load("Prefabs/SaveManager")) as GameObject;
                Values.SetCurrentSaveManager(saveManager);

            }
            else
            {
                saveManager = Values.GetCurrentSaveManager();
            }

            DontDestroyOnLoad(saveManager);
            saveManager.GetComponent<SaveSystem>().Load();
        }
        public void QuitGame()
        {
            Application.Quit();
        }

        /// <summary>
        /// Select level menu
        /// </summary>
        public void SelectLevelMenu()
        {
            //Clear button selection
            EventSystem.current.SetSelectedGameObject(null);

            mainMenu.SetActive(false);
            selectMenu.SetActive(true);
            Values.SetGiveAllWeapons(true);

            //Sets selected button
            EventSystem.current.SetSelectedGameObject(selectedOnLevelSelectionOpen);

            //todo tutte le armi di default..... usare uno sceneloader object come per il savesystem object?

        }

        public void LoadLevel(int id)
        {
            mainMenu.SetActive(false);        //Lo disattiva per evitare che il player possa continaure a clickare sui buttons
            selectMenu.SetActive(false);
            StartCoroutine(SceneLoader.LoadScene("Scenes/Level" + id));

        }
        public void GetBackToMainMenuFromSelect()
        {
            //Clear button selection
            EventSystem.current.SetSelectedGameObject(null);

            selectMenu.SetActive(false);
            mainMenu.SetActive(true);
            Values.SetGiveAllWeapons(false);

            //Sets selected button
            EventSystem.current.SetSelectedGameObject(selectedOnLevelSelectionClose);
        }

        public void SetResolution(int index)
        {
            Values.GetSettings().SetChosenResolution(index);
        }

        
        
        
        
        /// <summary>
        /// Options
        /// </summary>

        public void OptionsMenu()
        {
            //Clear button selection
            EventSystem.current.SetSelectedGameObject(null);

            mainMenu.SetActive(false);
            optionsMenu.SetActive(true);

            //Sets selected button
            EventSystem.current.SetSelectedGameObject(selectedOnOptionsOpen);
        }

        public void GetBackToMainMenuFromOptions()
        {
            //Clear button selection
            EventSystem.current.SetSelectedGameObject(null);

            optionsMenu.SetActive(false);
            mainMenu.SetActive(true);

            //Sets selected button
            EventSystem.current.SetSelectedGameObject(selectedOnOptionsClose);
        }
    
    }
}