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

            GameObject boss = PrefabUtility.InstantiatePrefab(bossPrefab) as GameObject;
            
            
            boss.GetComponent<NavMeshAgent>().Warp(transform.position);
            boss.transform.position = transform.position;
            boss.transform.rotation = Quaternion.Euler(0,180,0);

            Values.GetEnemySpritesManager().AddEnemyToEnemyList(boss); //needed to make the sprite viewing works
            
            //Disabilita il salvataggio durante le boss battles
            Values.SetCanSave(false);
            Values.SetCurrentBoss(boss);        //salva il current boss su values per il path unlocker
            
            //Attiva il pathUnlocker
            pathUnlocker.GetComponent<PathUnlocker>().enabled = true;
            
            this.enabled = false;        //disattiva lo spawner
        }
        
        
    }
}
