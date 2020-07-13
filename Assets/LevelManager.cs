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
    public Dictionary<String, EnemyStatus> enemiesStatus;
    public Dictionary<String, EnemyMovement> enemyMovementStatus;
    public Dictionary<String, EnemyIntelligence> enemyIntelligenceStatus;
    public Dictionary<String, EnemyShooting> enemyShootingStatus;



    private void Start()
    {
        enemiesStatus = new Dictionary<string, EnemyStatus>();

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

    public Dictionary<String, EnemySpawnerStatus> GetSpawnerStatus()
    {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag(SpawnerTag);

        Dictionary<String, EnemySpawnerStatus> spawnerStatus = new Dictionary<string, EnemySpawnerStatus>();

        foreach (var spawner in spawners)
        {
            spawnerStatus.Add(spawner.gameObject.name, spawner.GetComponent<EnemySpawner>().GetStatus());
        }

        return spawnerStatus;

    }

    public void UpdateEnemiesStatus()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(EnemyTag);

        foreach (var enemy in enemies)
        {

            EnemyStatus status = enemy.GetComponent<EnemyBase>().GetStatus();
            try
            {
                enemiesStatus.Add(enemy.name, status );

            }
            catch (ArgumentException)
            {
                enemiesStatus.Remove(enemy.name);
                enemiesStatus.Add(enemy.name, status);

            }
            
            //if there is currently another enemy with the same name, deletes it and then re add it
            // enemyIntelligenceStatus.Add(enemy.name, enemy.GetComponent<EnemyIntelligence>());
            // enemyMovementStatus.Add(enemy.name, enemy.GetComponent<EnemyMovement>());
            // enemyShootingStatus.Add(enemy.name, enemy.GetComponent<EnemyShooting>());

        }

    }


    public Dictionary<string, bool> GetPickups()
    {
        GameObject[] pickups = GameObject.FindGameObjectsWithTag(PickupTag);

        return null;
    }
    
}
