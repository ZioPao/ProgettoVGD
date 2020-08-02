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

            //todo permetti di spawnarei l boss con una posizione programmabile
            GameObject boss = Instantiate(bossPrefab, transform.position, Quaternion.Euler(0,180,0));

            Values.GetEnemySpritesManager().AddEnemyToEnemyList(boss); //needed to make the sprite viewing works
            
            //Disabilita il salvataggio durante le boss battles
            Values.SetCanSave(false);
            
            //Attiva il pathUnlocker
            pathUnlocker.GetComponent<PathUnlocker>().enabled = true;
            
            this.enabled = false;        //disattiva lo spawner
        }
        
        
    }
}
