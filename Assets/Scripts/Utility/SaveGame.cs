using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.UNetWeaver;
using UnityEngine;

namespace Utility
{
    public class SaveGame
    {
        public SaveGame()
        {
            save = new Save();
        }
        
        
        private Save save;

        public void Save()
        {
            
            //Player stats
            //todo da fare
            
            //Get the level manager
            LevelManager manager = GameObject.FindWithTag("Level").GetComponent<LevelManager>();
            
            //Pickups
            save.pickupStatus = manager.GetPickups();
            
            //Spawners
            save.enemySpawnerStatus = manager.GetSpawnerStatus();
            
            //Triggers
            save.triggersStatus = manager.GetTriggerStatus();
            
            //Enemies status saving
            manager.UpdateEnemiesStatus();
            save.enemyBaseStatus = manager.enemyBaseStatus;
            save.enemyIntelligenceStatus = manager.enemyIntelligenceStatus;
            save.enemyMovementStatus = manager.enemyMovementStatus;
            save.enemyShootingStatus = manager.enemyShootingStatus;

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
            bf.Serialize(file, save);
            file.Close();

            UnityEngine.Debug.Log("Saved");
        }
    }
}