using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Enemies;
using Player;
using Unity.UNetWeaver;
using UnityEditor;
using UnityEngine;
using Utility;

namespace Saving
{
    public class SaveSystem
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
            save.pickupStatus = manager.GetPickups();

            //Spawners
            save.enemySpawnerStatus = manager.GetSpawnerStatus();

            //Triggers
            //save.triggersStatus = manager.GetTriggerStatus();

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

            //Clean the level and set it up 
            Object.Destroy(GameObject.FindWithTag("Level"));
            GameObject newLevel = Resources.Load<GameObject>("Prefabs/Levels/" + save.levelName);

            PrefabUtility.UnpackPrefabInstance(PrefabUtility.InstantiatePrefab(newLevel) as GameObject
                , PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);


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

            foreach (var oldSpawner  in GameObject.FindGameObjectsWithTag("Spawner"))
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
            
            Values.SetIsLoadingSave(false);        //Finished loading

        }
    }
}