using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Enemies;
using Player;
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
            // save.enemyIntelligenceStatus = manager.enemyIntelligenceStatus;
            // save.enemyMovementStatus = manager.enemyMovementStatus;
            // save.enemyShootingStatus = manager.enemyShootingStatus;

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
            bf.Serialize(file, save);
            file.Close();

            UnityEngine.Debug.Log("Saved");
        }

        public void Load()
        {
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
            
            PrefabUtility.InstantiatePrefab(newLevel);


            //Deletes all the enemies and projectiles
            foreach (var enemy in GameObject.FindGameObjectsWithTag("enemy"))
            {
                Object.Destroy(enemy);
            }

            //creates them again
            GameObject enemyPrefab =
                Resources.Load<GameObject>("Prefabs/Enemies/" + save.levelName); //Level name = enemy type
            foreach (var element in save.enemiesStatus)
            {
                GameObject tmpEnemy = PrefabUtility.InstantiatePrefab(enemyPrefab) as GameObject;
                
                tmpEnemy.GetComponent<EnemyBase>().Reload(element.Value);

                tmpEnemy.transform.position = element.Value.GetPosition();
                tmpEnemy.transform.rotation = element.Value.GetRotation();
                tmpEnemy.GetComponent<EnemyMovement>().Reload();

                tmpEnemy.GetComponent<EnemyIntelligence>().Start();
                tmpEnemy.GetComponent<EnemyShooting>().Start();
            }
        }
    }
}