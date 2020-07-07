using System.Collections.Generic;
using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Utility;

namespace Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private int maxEnemiesConcurrently;


        private EnemySpritesManager enemySpritesManager;
        private List<GameObject> enemiesSpawned;

        private void Start()
        {
            enemiesSpawned = new List<GameObject>();
            enemySpritesManager = GameObject.Find("Camera_Main").GetComponent<EnemySpritesManager>();
        }

        private void FixedUpdate()
        {
            //Starts the timer

            Utility.TimerController.RunTimer(TimerController.TimerEnum.EnemySpawn);


            print(enemiesSpawned.Count);

            if (Utility.TimerController.GetCurrentTime()[Utility.TimerController.TimerEnum.EnemySpawn] <= 0)
            {
                RemoveDestroyedEnemies();

                if (enemiesSpawned.Count < maxEnemiesConcurrently)
                {
                    Utility.TimerController.ResetTimer(TimerController.TimerEnum.EnemySpawn);

                    var enemy = PrefabUtility.InstantiatePrefab(enemyPrefab) as GameObject;
                    enemiesSpawned.Add(enemy); //So we can check if they're destroyed or not

                    //todo sta roba è un macigno
                    enemy.GetComponent<NavMeshAgent>()
                        .Warp(transform.position + new Vector3(Random.Range(-5, 5), 0, 0));
                    enemy.transform.position = transform.position + new Vector3(Random.Range(-5, 5), 0, 0);

                    enemySpritesManager.AddEnemyToEnemyList(enemy); //needed to make the sprite viewing works
                }
            }
        }


        private void RemoveDestroyedEnemies()
        {
            //todo is it that expensive?
            enemiesSpawned.RemoveAll(item => item == null);
        }
    }
}