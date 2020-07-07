using System;
using System.Collections.Generic;
using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Utility;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private int maxEnemiesConcurrently;

        [SerializeField] private int maxEnemiesSpawned;
        [SerializeField] private int rangeSpawn;
        [SerializeField] private String customName = "e1";
        [SerializeField] private bool isBoss;


        private EnemySpritesManager enemySpritesManager;
        private List<GameObject> enemiesSpawned;

        private int counter;

        private void Start()
        {
            enemiesSpawned = new List<GameObject>();
            enemySpritesManager = GameObject.Find("Camera_Main").GetComponent<EnemySpritesManager>();
            counter = 0;
        }

        private void FixedUpdate()
        {
            //Starts the timer
            if (!isBoss)
            {
                Utility.TimerController.RunTimer(TimerController.TimerEnum.EnemySpawn);

                if (Utility.TimerController.GetCurrentTime()[Utility.TimerController.TimerEnum.EnemySpawn] <= 0)
                {
                    RemoveDestroyedEnemies();

                    if (enemiesSpawned.Count < maxEnemiesConcurrently)
                    {
                        Utility.TimerController.ResetTimer(TimerController.TimerEnum.EnemySpawn);

                        var enemy = PrefabUtility.InstantiatePrefab(enemyPrefab) as GameObject;
                        enemy.name = customName + "_" + counter;
                        enemiesSpawned.Add(enemy); //So we can check if they're destroyed or not

                        //todo sta roba è un macigno
                        enemy.GetComponent<NavMeshAgent>()
                            .Warp(transform.position + new Vector3(Random.Range(-rangeSpawn, rangeSpawn), 0,
                                Random.Range(-rangeSpawn, rangeSpawn)));

                        enemy.transform.position = transform.position +
                                                   new Vector3(Random.Range(-rangeSpawn, rangeSpawn), 0,
                                                       Random.Range(-rangeSpawn, rangeSpawn));
                        enemy.transform.rotation = Random.rotation;

                        enemySpritesManager.AddEnemyToEnemyList(enemy); //needed to make the sprite viewing works


                        //After tot enemies spawned, the spawner destroys itself
                        counter++;
                        if (counter == maxEnemiesSpawned)
                            Destroy(gameObject);
                    }
                }
            }
        }

        private void Awake()
        {
            if (isBoss)
            {
                enemySpritesManager = GameObject.Find("Camera_Main").GetComponent<EnemySpritesManager>();

                var boss = PrefabUtility.InstantiatePrefab(enemyPrefab) as GameObject;
                boss.name = customName;

                //todo sta roba è un macigno
                boss.GetComponent<NavMeshAgent>()
                    .Warp(transform.position + new Vector3(Random.Range(-rangeSpawn, rangeSpawn), 0,
                        Random.Range(-rangeSpawn, rangeSpawn)));

                boss.transform.position = transform.position;
                boss.transform.rotation = Quaternion.Euler(0,180,0);

                enemySpritesManager.AddEnemyToEnemyList(boss); //needed to make the sprite viewing works

                this.enabled = false; //disable the module
            }
        }


        private void RemoveDestroyedEnemies()
        {
            //todo is it that expensive?
            enemiesSpawned.RemoveAll(item => item == null);
        }
    }
}