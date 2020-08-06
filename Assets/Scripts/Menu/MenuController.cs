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

namespace Menu
{
    public class MenuController : MonoBehaviour
    {

        [SerializeField] private GameObject mainMenu, selectMenu, optionsMenu;

        [SerializeField] private TMP_Dropdown resolutionDropdown;
        
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
        }

        public void StartNewGame()
        {
            mainMenu.SetActive(false);
            Values.SetIsStartingNewGame(true);
            LoadLevel(1);
            //StartCoroutine(SceneLoader.LoadWeapons(Values.WeaponEnum.Pistol, true));

            //Values.AddHeldWeapon(Values.WeaponEnum.Knife, true);
        }

        public void LoadGame()
        {
            GetComponentInParent<Canvas>().enabled = false;

            GameObject saveManager =
                Instantiate(Resources.Load("Prefabs/SaveManager")) as GameObject;
            DontDestroyOnLoad(saveManager);
            Values.SetCurrentSaveManager(saveManager);
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
            
            mainMenu.SetActive(false);
            selectMenu.SetActive(true);
            Values.SetGiveAllWeapons(true);


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
            selectMenu.SetActive(false);
            mainMenu.SetActive(true);
            Values.SetGiveAllWeapons(false);
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
            mainMenu.SetActive(false);
            optionsMenu.SetActive(true);

        }

        public void GetBackToMainMenuFromOptions()
        {
            optionsMenu.SetActive(false);
            mainMenu.SetActive(true);
        }


       
    }
}