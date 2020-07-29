using UnityEngine;
using Utility;

namespace Boss
{
    public class BossTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject rockBlocker;
        [SerializeField] private GameObject bossSpawnerObject;
        [SerializeField] private GameObject sun;


    
        private void OnTriggerEnter(Collider other)
        {
        
            //riattiva la roccia e fa un rumorone
            rockBlocker.GetComponent<MeshCollider>().enabled = true;
            rockBlocker.GetComponent<MeshRenderer>().enabled = true;

            //Attiva il boss
            BossSpawner bossSpawner = bossSpawnerObject.GetComponent<BossSpawner>();
            bossSpawner.enabled = true;

            //todo play sound

            //Diminuisce la luce

            sun.GetComponent<Light>().intensity = 0;
            enabled = false;        //disabilita il trigger
        }
    }
}
