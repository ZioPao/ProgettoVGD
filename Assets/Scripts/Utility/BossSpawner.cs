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
        [SerializeField] private string customName;

        private void Start()
        {

            //todo permetti di spawnarei l boss con una posizione programmabile
            GameObject boss = Instantiate(bossPrefab, transform.position, Quaternion.Euler(0,180,0));

            boss.transform.LookAt(Values.GetPlayerTransform());
            if (customName != null)
            {
                boss.transform.name = customName;
            }
            
            Values.GetEnemySpritesManager().AddEnemyToEnemyList(boss); //needed to make the sprite viewing works
            
            //Attiva il pathUnlocker
            enabled = false;        //disattiva lo spawner
        }
        
        
    }
}
