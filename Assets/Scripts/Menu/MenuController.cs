using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Saving;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        public void LoadLevel1()
        {
            StartCoroutine(LoadLevel(1));

        }

        public void LoadLevel2()
        {
            StartCoroutine(LoadLevel(2));

        }

        public void LoadLevel3()
        {
            StartCoroutine(LoadLevel(3));

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

        private IEnumerator LoadLevel(int id)
        {

            //todo per evitare più click. Vedi di mettere la schermata di caricamento o qualcosa così
            GetComponentInParent<Canvas>().enabled = false;
            yield return null;
            
            //Begin to load the Scene you specify
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Scenes/Level" + id, LoadSceneMode.Single);
            //Don't let the Scene activate until you allow it to
            asyncOperation.allowSceneActivation = false;
            //When the load is still in progress, output the Text and progress bar
            while (!asyncOperation.isDone)
            {
                // Check if the load has finished
                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                } //spawnPoint = new Vector3(145, 67, 40); //level 3

                yield return null;

                //spawnPoint = new Vector3()
            }
        }

       
    }
}