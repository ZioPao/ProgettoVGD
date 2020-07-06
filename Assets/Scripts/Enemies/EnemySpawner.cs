using System.Collections.Generic;
using Player;
using UnityEditor;
using UnityEngine;
using Utility;

namespace Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private int maxEnemiesConcurrently;


        private CameraScript cameraPlayer;
        private List<GameObject> enemiesSpawned;

        private void Start()
        {
            enemiesSpawned = new List<GameObject>();
            cameraPlayer = GameObject.Find("Camera_Main").GetComponent<CameraScript>();
        }

        private void FixedUpdate()
        {
            //Starts the timer

            if (Utility.TimerController.GetCurrentTime()[Utility.TimerController.TimerEnum.EnemySpawn] <= 0
                && enemiesSpawned.Count <= maxEnemiesConcurrently
                )
            {
                Utility.TimerController.ResetTimer(TimerController.TimerEnum.EnemySpawn);

                var enemy = PrefabUtility.InstantiatePrefab(enemyPrefab) as GameObject;
                enemiesSpawned.Add(enemy); //So we can check if they're destroyed or not
                enemy.transform.position = transform.position;

                cameraPlayer.AddEnemyToEnemyList(enemy);        //needed to make the sprite viewing works
            }
        }
    }
}