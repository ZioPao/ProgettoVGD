using System;
using System.Collections;
using System.Collections.Generic;
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
            SceneManager.LoadScene("BaseGame");

            SceneManager.activeSceneChanged += OnSceneWasLoaded;
            

        }

        public void QuitGame()
        {
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }

        private static void OnSceneWasLoaded (Scene from, Scene to) {
            var playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
            var level = Resources.Load<GameObject>("Prefabs/Levels/Level1");

            PrefabUtility.InstantiatePrefab(level);
            var playerInstance = PrefabUtility.InstantiatePrefab(playerPrefab) as GameObject;        //Where?
            RenderSettings.skybox = Resources.Load<Material>("Prefabs/Levels/Level1/Skybox");;
            //debug
            var spawnPoint = new Vector3(1237, 115, 1041);
            //spawnPoint = new Vector3(145, 67, 40); //level 3
            
            
            //spawnPoint = new Vector3()
            playerInstance.transform.position = spawnPoint;
        }

    }
}