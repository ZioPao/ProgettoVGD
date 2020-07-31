using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Saving;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MenuController : MonoBehaviour
    {
        private Material skybox;

        public void NewGame()
        {
            StartCoroutine(LoadNewGame());
        }

        public void LoadGame()
        {
            GameObject saveManager =
                PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/SaveManager")) as GameObject;
            DontDestroyOnLoad(saveManager);
            Values.SetCurrentSaveManager(saveManager);
            saveManager.GetComponent<SaveSystem>().Load();
        }

        public void QuitGame()
        {
            EditorApplication.isPlaying = false;
            Application.Quit();
        }

        private IEnumerator LoadNewGame()
        {
            yield return null;

            //Begin to load the Scene you specify
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Scenes/Level1", LoadSceneMode.Single);
            //Don't let the Scene activate until you allow it to
            asyncOperation.allowSceneActivation = false;
            Debug.Log("Pro :" + asyncOperation.progress);
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

        private IEnumerator LoadSavedGame()
        {
            //Operations on hold until we can actually load
            // SaveSystem tmp = new GameObject().AddComponent<SaveSystem>();
            // tmp.Load();
            // Destroy(tmp);


            yield return null;
            //spawnPoint = new Vector3(145, 67, 40); //level 3


            //spawnPoint = new Vector3()
        }
    }
}