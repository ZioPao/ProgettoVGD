using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Utility
{

    public class BossSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject bossPrefab;
        [SerializeField] private GameObject bossEndObject;
        

        private void Start()
        {
            
            GameObject boss = PrefabUtility.InstantiatePrefab(bossPrefab) as GameObject;
            
            //todo saving
            BossEnding bossEnding = bossEndObject.GetComponent<BossEnding>();
            bossEnding.enabled = true;
            
            boss.GetComponent<NavMeshAgent>().Warp(transform.position);
            boss.transform.position = transform.position;
            boss.transform.rotation = Quaternion.Euler(0,180,0);

            Values.GetEnemySpritesManager().AddEnemyToEnemyList(boss); //needed to make the sprite viewing works

            this.enabled = false;
        }
    }
}
