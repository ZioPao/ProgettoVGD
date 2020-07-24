using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Enemies;
using Player;
using Unity.UNetWeaver;
using UnityEditor;
using UnityEngine;
using Utility;
using Object = UnityEngine.Object;

namespace Saving
{
    public class SaveSystem : MonoBehaviour
    {
        public SaveSystem()
        {
            save = new Save();
        }


        private Save save;

        public void Save()
        {
            //Player stats
            Transform player = Values.GetPlayerTransform();
            save.playerPosition = player.position;
            save.playerRotation = player.rotation;

            save.health = Values.GetHealth();
            save.stamina = Values.GetStamina();


            //Get the level manager
            GameObject level = GameObject.FindWithTag("Level");

            save.levelName = level.name;
            LevelManager manager = level.GetComponent<LevelManager>();

            //Pickups
            try
            {
                save.pickupStatus = manager.GetPickups();
            }
            catch (NullReferenceException)
            {
                ;
            }

            //Spawners
            try
            {
                save.enemySpawnerStatus = manager.GetSpawnerStatus();
            }
            catch (NullReferenceException)
            {
                ;
            }
            //Triggers
            //save.triggersStatus = manager.GetTriggerStatus();

            //Levers and doors
            save.interactableStatus = manager.GetInteractables();

            //Enemies status saving
            manager.UpdateEnemiesStatus();
            save.enemiesStatus = manager.enemiesStatus;

            //Projectiles

            save.projectileStatus = manager.GetProjectileStatus();

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
            bf.Serialize(file, save);
            file.Close();

            UnityEngine.Debug.Log("Saved");
        }

        public void Load()
        {
            Values.SetIsLoadingSave(true);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);

            save = (Save) bf.Deserialize(file);
            file.Close();


            //Player stats and position

            Values.SetHealth(save.health);
            Values.SetStamina(save.stamina);

            Transform playerTransform = Values.GetPlayerTransform();
            playerTransform.position = save.playerPosition;
            playerTransform.rotation = save.playerRotation;
            
            //todo salvare ammo


            StartCoroutine(LoadLevel(save.levelName));
            //WAIT FOR A SEC TO LET IT LOAD


            //Deletes all the enemies
            foreach (var enemy in GameObject.FindGameObjectsWithTag("enemy"))
            {
                Object.Destroy(enemy);
            }

            //creates them again
            GameObject enemyPrefab =
                Resources.Load<GameObject>("Prefabs/Enemies/" + save.levelName); //Level name = enemy type

            EnemySpritesManager spritesManager = Values.GetEnemySpritesManager();
            foreach (var element in save.enemiesStatus)
            {
                GameObject tmpEnemy = PrefabUtility.InstantiatePrefab(enemyPrefab) as GameObject;
                tmpEnemy.transform.SetParent(GameObject.Find("Enemies").transform);

                tmpEnemy.GetComponent<EnemyBase>().Reload(element.Value);

                tmpEnemy.transform.position = element.Value.GetPosition();
                tmpEnemy.transform.rotation = element.Value.GetRotation();
                tmpEnemy.GetComponent<EnemyMovement>().Reload();

                tmpEnemy.GetComponent<EnemyIntelligence>().Start();
                tmpEnemy.GetComponent<EnemyShooting>().Start();


                spritesManager.AddEnemyToEnemyList(tmpEnemy);
            }


            //Projectiles

            //Destroy all the old projectiles
            foreach (var p in GameObject.FindGameObjectsWithTag("Projectile"))
            {
                Object.Destroy(p);
            }

            GameObject projPrefab =
                Resources.Load<GameObject>("Prefabs/Projectiles/" + save.levelName); //Level name = projectile type
            foreach (var pStatus in save.projectileStatus)
            {
                GameObject tmpProj = PrefabUtility.InstantiatePrefab(projPrefab) as GameObject;

                ProjectileScript tmpScript = tmpProj.GetComponent<ProjectileScript>();

                tmpScript.SetTransform(pStatus.GetPosition(), pStatus.GetRotation());
                tmpScript.Reload(pStatus);
            }

            //SPAWNERS 
            GameObject spawnerPrefab =
                Resources.Load<GameObject>("Prefabs/Spawners/EnemySpawner"); //Level name = enemy type

            foreach (var oldSpawner in GameObject.FindGameObjectsWithTag("Spawner"))
            {
                Object.Destroy(oldSpawner);
            }

            foreach (var element in save.enemySpawnerStatus)
            {
                GameObject newSpawner = PrefabUtility.InstantiatePrefab(spawnerPrefab) as GameObject;


                //Per qualche ragione non ne vuole sapere di inserire i nuovi spawner come figli di Spawners. Mi arrendo al momento
                //newSpawner.transform.SetParent(GameObject.Find("Spawners").transform); 
                newSpawner.transform.position = element.Value.GetPosition();
                newSpawner.transform.rotation = element.Value.GetRotation();

                newSpawner.GetComponent<EnemySpawner>().SetStatus(element.Value);
                newSpawner.GetComponent<EnemySpawner>().SetEnemyPrefab(enemyPrefab);
            }


            //Interactables
            foreach (var interactable in save.interactableStatus)
            {
                var interactableObject = GameObject.Find(interactable.Key);

                switch (interactableObject.name)
                {
                    case "LeverBoss":
                        var lever = interactableObject.GetComponent<LeverScript>();

                        if (interactable.Value)
                        {
                            //Reset
                            
                            //Destroy(interactableObject);
                            //var z = LoadPrefab("Prefabs/Levels/Generic/Prefabs/LeverBoss");
                            
                            
                            
                        }
                        else
                        {
                            lever.ForceActivation();
                        }

                        break;

                    case "DoorOptional": 
                        var door = interactableObject.GetComponent<OpenDoor>();

                        if (!interactable.Value)
                        {
                            door.ForceActivation();
                            
                        }
                        break;
                    default:
                        break;
                }
            }

            Values.SetIsLoadingSave(false); //Finished loading
        }

        IEnumerator LoadLevel(string levelName)
        {
            //Clean the level and set it up 
            Object.Destroy(GameObject.FindWithTag("Level"));
            GameObject newLevel = Resources.Load<GameObject>("Prefabs/Levels/" + save.levelName);
            
            PrefabUtility.UnpackPrefabInstance(PrefabUtility.InstantiatePrefab(newLevel) as GameObject
                , PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            
            yield return new WaitForEndOfFrame();
        }

        IEnumerator LoadPrefab(string path)
        {
            ResourceRequest prefab = Resources.LoadAsync<GameObject>(path);

            while (!prefab.isDone)
                yield return null; //wait
            
            

            yield return new WaitForEndOfFrame();
            
        }

      
        
    }
}