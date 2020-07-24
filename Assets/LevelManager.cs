using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using UnityEngine;
using Utility;

public class LevelManager : MonoBehaviour
{

    private const string EnemyTag = "enemy";
    private const string PickupTag = "Pickup";
    private const string InteractableTag = "Interactable";
    private const string InteractableOverTag = "InteractableOver";
    private const string TriggerTag = "Trigger";
    private const string SpawnerTag = "Spawner";
    private const string ProjectileTag = "Projectile";

    private const string LeverBossString = "LeverBoss";

    
    //Enemy stuff
    public Dictionary<String, EnemyStatus> enemiesStatus;



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

    public List<ProjectileStatus> GetProjectileStatus()
    {
        var projectiles = GameObject.FindGameObjectsWithTag(ProjectileTag).ToList();
        List<ProjectileStatus> projectilesStatus = new List<ProjectileStatus>();
        
        foreach (var x in projectiles)
        {
            projectilesStatus.Add(x.GetComponent<ProjectileScript>().GetStatus());
        }

        return projectilesStatus;


    }

    public Dictionary<string, bool> GetPickups()
    {
        GameObject[] pickups = GameObject.FindGameObjectsWithTag(PickupTag);

        return null;
    }

    public Dictionary<string, bool> GetInteractables()
    {


        Dictionary<string, bool> interactablesDictionary = new Dictionary<string, bool>();
        GameObject[] interactables = GameObject.FindGameObjectsWithTag(InteractableTag);

        foreach (var x in interactables)
        {

            switch (x.name)
            {
                case LeverBossString:
                    LeverScript tmpLever = x.GetComponent<LeverScript>();
                    interactablesDictionary.Add(x.name, tmpLever.enabled);
                    break;
                case "DoorOptional":
                    OpenDoor tmpDoor = x.GetComponent<OpenDoor>();
                    interactablesDictionary.Add(x.name, tmpDoor.enabled);
                    break;
            }
            
        }

        foreach (var x in GameObject.FindGameObjectsWithTag(InteractableOverTag))
        {
            switch (x.name)
            {
                case LeverBossString:
                    LeverScript tmpLever = x.GetComponent<LeverScript>();
                    interactablesDictionary.Add(x.name, tmpLever.enabled);
                    break;
                case "DoorOptional":
                    OpenDoor tmpDoor = x.GetComponent<OpenDoor>();
                    interactablesDictionary.Add(x.name, tmpDoor.enabled);
                    break;
            }
        }
        return interactablesDictionary;
    }
    


}
