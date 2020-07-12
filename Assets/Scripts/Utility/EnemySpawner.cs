using System;
using System.Collections.Generic;
using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Utility
{
    [Serializable]
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private int maxEnemiesConcurrently;

        [SerializeField] private int maxEnemiesSpawned;
        [SerializeField] private int rangeSpawn;
        [SerializeField] private String customName = "e1";

        private EnemySpawnerStatus status;

        private List<GameObject> enemiesSpawned;

        private int counter;

        private void Start()
        {
            
            status = new EnemySpawnerStatus();
            status.Setup(0, maxEnemiesSpawned, maxEnemiesConcurrently);
            
            enemiesSpawned = new List<GameObject>();
        }

        private void FixedUpdate()
        {
            TimerController.RunTimer(TimerController.ENEMYSPAWN_K);
            status.SetSpawnedEnemiesCount(enemiesSpawned.Count);
            int enemiesSpawnedCount = status.GetSpawnedEnemiesCount();

                if (TimerController.GetCurrentTime()[TimerController.ENEMYSPAWN_K] <= 0)
                {
                    RemoveDestroyedEnemies();

                    if (enemiesSpawnedCount < maxEnemiesConcurrently)
                    {
                        TimerController.ResetTimer(TimerController.ENEMYSPAWN_K);
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

        public EnemySpawnerStatus GetStatus()
        {
            return status;
        }
    }
}