using UnityEngine;
using Utility;

namespace Boss
{
    public class BossTrigger : MonoBehaviour, ITriggerMidGame
    {
        [SerializeField] private GameObject rockBlocker;
        [SerializeField] private GameObject bossSpawnerObject;
        [SerializeField] private GameObject sun;
		[SerializeField] private AudioSource fallEffect;

    
        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Player"))
            {
                RunTrigger();
            }
         
        }

        public void RunTrigger()
        {
            //riattiva la roccia e fa un rumorone
            rockBlocker.GetComponent<MeshCollider>().enabled = true;
            rockBlocker.GetComponent<MeshRenderer>().enabled = true;

            //Attiva il boss
            BossSpawner bossSpawner = bossSpawnerObject.GetComponent<BossSpawner>();
            bossSpawner.enabled = true;

            //Play Sound
            fallEffect.Play();

            //Diminuisce la luce

            if (sun)
            {
                sun.GetComponent<Light>().intensity = 0;
            }

            gameObject.SetActive(false); //disattiva i lgameobject

        }
    }
}
