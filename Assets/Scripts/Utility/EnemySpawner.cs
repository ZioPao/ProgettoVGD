using System;
using System.Collections.Generic;
using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Utility;
using Random = UnityEngine.Random;
using TimerEnum = Utility.TimerController.TimerEnum;

namespace Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private int maxEnemiesConcurrently;

        [SerializeField] private int maxEnemiesSpawned;
        [SerializeField] private int rangeSpawn;
        [SerializeField] private String customName = "e1";


        private List<GameObject> enemiesSpawned;

        private int counter;

        private void Start()
        {
            enemiesSpawned = new List<GameObject>();
            counter = 0;
        }

        private void FixedUpdate()
        {
            TimerController.RunTimer(TimerEnum.EnemySpawn);

                if (TimerController.GetCurrentTime()[TimerEnum.EnemySpawn] <= 0)
                {
                    RemoveDestroyedEnemies();

                    if (enemiesSpawned.Count < maxEnemiesConcurrently)
                    {
                        TimerController.ResetTimer(TimerEnum.EnemySpawn);
                        SpawnCommonEnemy();

                        //After tot enemies spawned, the spawner destroys itself
                        counter++;
                        if (counter == maxEnemiesSpawned)
                            this.enabled = false;
                    }
                }
            
        }

        
        private void SpawnCommonEnemy()
        {
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

            Values.GetEnemySpritesManager().AddEnemyToEnemyList(enemy); //needed to make the sprite viewing works

        }

        private void RemoveDestroyedEnemies()
        {
            //todo is it that expensive?
            enemiesSpawned.RemoveAll(item => item == null);
        }
    }
}