using System;
using System.Collections.Generic;
using Enemies;
using Player;
using Utility;

namespace Saving
{
    [System.Serializable]
    public class Save
    
    {
 
        /*Player stuff*/
        public SerializableVector3 playerPosition;
        public SerializableQuaternion playerRotation;
        
        //Stats base
        public float health, stamina;
        public bool hasKey;

        
        //Armi
        public Dictionary<Values.WeaponEnum, bool> heldWeapons;
        public Dictionary<Values.WeaponEnum, int> weaponsCurrentAmmo;
        public Dictionary<Values.WeaponEnum, int> weaponsCurrentReserve;
        public Values.WeaponEnum currentWeapon;
        
        //Nome del livello
        public string levelName;

        
        //Pickups
        public List<string> pickups;
        
        //Spawners

        public Dictionary<string, EnemySpawnerStatus> enemySpawnerStatus;
        
        //Triggers

        public HashSet<string> triggers;
        
        //Interactables

        public Dictionary<string, bool> interactableStatus;
        
        //Nemici

        public Dictionary<string, EnemyStatus> enemiesStatus;

        public List<ProjectileStatus> projectileStatus;
    }
}
