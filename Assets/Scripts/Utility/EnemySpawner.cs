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
        [SerializeField] private GameObject spawnEffectPrefab;
        [SerializeField] private Color spawnEffectColor;
        [SerializeField] private int maxEnemiesConcurrently;

        [SerializeField] private int maxEnemiesSpawned;
        [SerializeField] private int rangeSpawn;
        [SerializeField] private String customName = "e1";

        private GameObject enemiesParent;
        private EnemySpawnerStatus status;

        private List<GameObject> enemiesSpawned;

        private void Awake()
        {
            //Debug.Log("Awake EnemySpawner");
            if (!Values.GetIsLoadingSave())
            {
                status = new EnemySpawnerStatus();
                status.Setup(name, 0, maxEnemiesSpawned, maxEnemiesConcurrently);

                status.SaveTransform(transform.position, transform.rotation);
            }


            enemiesParent = GameObject.Find("Enemies");
            enemiesSpawned = new List<GameObject>();
        }

        private void FixedUpdate()
        {
            if (!Values.GetIsLoadingSave() && !Values.GetIsChangingScene())
            {
                TimerController.RunTimer(TimerController.ENEMYSPAWN_K);
                status.SetSpawnedEnemiesCount(enemiesSpawned.Count);
                int enemiesSpawnedCount = status.GetSpawnedEnemiesCount();

                if (TimerController.GetCurrentTime()[TimerController.ENEMYSPAWN_K] <= 0)
                {
                    RemoveDestroyedEnemies();

                    if (enemiesSpawnedCount < status.GetMaxEnemiesConcurrently())
                    {
                        TimerController.ResetTimer(TimerController.ENEMYSPAWN_K);
                        SpawnCommonEnemy();

                        //After tot enemies spawned, the spawner destroys itself
                        status.AddOneToCounter();
                        if (status.GetCounter() == status.GetMaxEnemiesSpawned())
                            this.enabled = false;
                    }
                }
            }
        }


        private void SpawnCommonEnemy()
        {
            var correctPosition = (transform.position +
                                   new Vector3(Random.Range(-rangeSpawn, rangeSpawn), 0,
                                       Random.Range(-rangeSpawn, rangeSpawn)));
            var correctRotation = Random.rotation;

            var enemy = Instantiate(enemyPrefab, correctPosition, correctRotation);
            enemy.name = customName + "_" + status.GetCounter();

            enemy.GetComponent<NavMeshAgent>()
                .Warp(correctPosition);
            try
            {
                enemy.transform.parent = enemiesParent.transform;
            }
            catch (Exception)
            {
                enemiesParent = GameObject.Find("Enemies");
                enemy.transform.parent = enemiesParent.transform;
            }

            enemiesSpawned.Add(enemy); //So we can check if they're destroyed or not
            Values.GetEnemySpritesManager()
                .AddEnemyToEnemyList(enemy); //needed to make the sprite viewing works


            var spawnEffect = Instantiate(spawnEffectPrefab, correctPosition, Quaternion.identity);
            var spawnEffectMain = spawnEffect.GetComponent<ParticleSystem>().main;
            spawnEffectMain.startColor = spawnEffectColor;

            var light = spawnEffect.GetComponentInChildren<Light>();
            light.color = spawnEffectColor;
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


        public void SetStatus(EnemySpawnerStatus status)
        {
            this.status = status;
        }

        public void SetEnemyPrefab(GameObject prefab)
        {
            this.enemyPrefab = prefab;
        }
    }
}