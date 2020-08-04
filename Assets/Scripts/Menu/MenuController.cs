using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Saving;
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

        [SerializeField] private Dropdown resolutionDropdown;

        private Resolution[] resolutions;
        public void Awake()
        {
            Audio.SoundManager.InitializeSoundEffects();
            Audio.SoundManager.InitializeSoundPlayer();
            Audio.SoundManager.InitializeSoundTracks();
            Audio.SoundManager.InitializeMusicPlayer();

            Audio.SoundManager.PlaySoundtrack(Audio.SoundManager.SoundTracks.TitleTrack);
            
            Values.SetIsInPause(false);        //Evita casini al reload
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            
            //Settings stuff
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            List<string> possibleResolutions = new List<string>();


            int index = 0;
            int currentResolutionindex = 0;
            foreach (var res in resolutions)
            {
                possibleResolutions.Add(res.width + "x" + res.height);


                if (resolutions[index].width == Screen.currentResolution.width &&
                    resolutions[index].height == Screen.currentResolution.height)
                    currentResolutionindex = index;

                index++;

            }

            resolutionDropdown.AddOptions(possibleResolutions);
            resolutionDropdown.value = currentResolutionindex;
            resolutionDropdown.RefreshShownValue();

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

        }

        public void LoadLevel(int id)
        {
            StartCoroutine(SceneLoader.LoadScene("Scenes/Level" + id));

        }
        public void GetBackToMainMenuFromSelect()
        {
            selectMenu.SetActive(false);
            mainMenu.SetActive(true);
        }

        
        
        
        
        /// <summary>
        /// Options
        /// </summary>

        public void OptionsMenu()
        {
            mainMenu.SetActive(false);
            optionsMenu.SetActive(true);

        }

        public void SetResolution(int index)
        {
            Resolution resolution = resolutions[index];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void GetBackToMainMenuFromOptions()
        {
            optionsMenu.SetActive(false);
            mainMenu.SetActive(true);
        }


       
    }
}