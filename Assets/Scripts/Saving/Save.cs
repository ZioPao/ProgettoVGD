using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Saving;
using UnityEngine;

namespace Utility
{
    [System.Serializable]
    public class Save
    
    {
 
        /*Player stuff*/
        public SerializableVector3 playerPosition;
        public SerializableQuaternion playerRotation;
        
        //Stats base
        public float health, stamina;
        
        //Nome del livello
        public string levelName;

        
        //Pickups
        public Dictionary<String, bool> pickupStatus;
        
        //Spawners

        public Dictionary<string, EnemySpawnerStatus> enemySpawnerStatus;
        
        //Triggers

        public Dictionary<string, bool> triggersStatus;
        
        //Nemici

        public Dictionary<string, EnemyStatus> enemiesStatus;

        public List<ProjectileStatus> projectileStatus;
    }
}
