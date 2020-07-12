using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Utility
{

    public class BossSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject bossPrefab;
        

        private void Start()
        {
            
            GameObject boss = PrefabUtility.InstantiatePrefab(bossPrefab) as GameObject;
            
            boss.GetComponent<NavMeshAgent>().Warp(transform.position);
            boss.transform.position = transform.position;
            boss.transform.rotation = Quaternion.Euler(0,180,0);

            Values.GetEnemySpritesManager().AddEnemyToEnemyList(boss); //needed to make the sprite viewing works

            this.enabled = false;
        }
    }
}
