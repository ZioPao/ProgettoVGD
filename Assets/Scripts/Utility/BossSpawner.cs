using Boss;
using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Utility
{

    public class BossSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject bossPrefab;
        [SerializeField] private GameObject pathUnlocker;
        

        private void Start()
        {
            //todo gestisci saving
            
            GameObject boss = PrefabUtility.InstantiatePrefab(bossPrefab) as GameObject;
            
            
            boss.GetComponent<NavMeshAgent>().Warp(transform.position);
            boss.transform.position = transform.position;
            boss.transform.rotation = Quaternion.Euler(0,180,0);

            Values.GetEnemySpritesManager().AddEnemyToEnemyList(boss); //needed to make the sprite viewing works
            
            //todo perché?
            //Values.SetCurrentBoss(boss);        //salva il current boss su values. abbastanza sporco così ma no time
            
            //Attiva il pathUnlocker
            pathUnlocker.GetComponent<PathUnlocker>().enabled = true;
            
            this.enabled = false;        //disattiva lo spawner
        }
        
        
    }
}
