using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Utility
{
    public class Save
    {
 
        /*Player stuff*/
        
        //Stats base
        public int health, stamina;
        
        //Nome del livello
        public string levelName = GameObject.FindWithTag("Level").name;

        
        //Pickups
        public Dictionary<String, bool> pickupStatus;
        
        //Spawners

        public Dictionary<string, EnemySpawner> enemySpawnerStatus;
        
        //Triggers

        public Dictionary<string, bool> triggersStatus;
        
        //Nemici

        public Dictionary<String, EnemyBase> enemyBaseStatus;
        public Dictionary<String, EnemyMovement> enemyMovementStatus;
        public Dictionary<String, EnemyIntelligence> enemyIntelligenceStatus;
        public Dictionary<String, EnemyShooting> enemyShootingStatus;
        
    }
}
