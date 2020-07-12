using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using Utility;

public class LevelManager : MonoBehaviour
{

    private const string EnemyTag = "enemy";
    private const string PickupTag = "Pickup";
    private const string TriggerTag = "Trigger";
    private const string SpawnerTag = "Spawner";
    
    
    //Enemy stuff
    public Dictionary<String, EnemyBase> enemyBaseStatus;
    public Dictionary<String, EnemyMovement> enemyMovementStatus;
    public Dictionary<String, EnemyIntelligence> enemyIntelligenceStatus;
    public Dictionary<String, EnemyShooting> enemyShootingStatus;



    private void Start()
    {
        enemyBaseStatus = new Dictionary<string, EnemyBase>();
        enemyIntelligenceStatus = new Dictionary<string, EnemyIntelligence>();
        enemyMovementStatus = new Dictionary<string, EnemyMovement>();
    }

    public Dictionary<String, bool> GetTriggerStatus()
    {
        //get if they're active or not
        GameObject[] triggers = GameObject.FindGameObjectsWithTag(TriggerTag);
        Dictionary<String, bool> triggerStatus = new Dictionary<string, bool>();

        foreach (var trigger in triggers)
        {
            triggerStatus.Add(trigger.name, trigger.activeSelf);
        }

        return triggerStatus;
    }

    public Dictionary<String, EnemySpawner> GetSpawnerStatus()
    {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag(SpawnerTag);

        Dictionary<String, EnemySpawner> spawnerStatus = new Dictionary<string, EnemySpawner>();

        foreach (var spawner in spawners)
        {
            spawnerStatus.Add(spawner.name, spawner.GetComponent<EnemySpawner>());
        }

        return spawnerStatus;

    }

    public void UpdateEnemiesStatus()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(EnemyTag);

        foreach (var enemy in enemies)
        {
            enemyBaseStatus.Add(enemy.name, enemy.GetComponent<EnemyBase>());
            enemyIntelligenceStatus.Add(enemy.name, enemy.GetComponent<EnemyIntelligence>());
            enemyMovementStatus.Add(enemy.name, enemy.GetComponent<EnemyMovement>());
            enemyShootingStatus.Add(enemy.name, enemy.GetComponent<EnemyShooting>());

        }

    }


    public Dictionary<string, bool> GetPickups()
    {
        GameObject[] pickups = GameObject.FindGameObjectsWithTag(PickupTag);

        return null;
    }
    
}
