using Player;
using UnityEngine;
using Utility;

namespace Boss
{
    public class BossTrigger : MonoBehaviour, ITriggerMidGame
    {
        [SerializeField] private GameObject rockBlocker;
        [SerializeField] private GameObject bossSpawnerObject;
        [SerializeField] private GameObject sun;

    
        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Player"))
            {
                RunTrigger();
            }
         
        }

        public void RunTrigger()
        {

            //Disabilita il salvataggio durante le boss battles
            Values.SetCanSave(false);

            //riattiva la roccia e fa un rumorone
            rockBlocker.GetComponent<MeshCollider>().enabled = true;
            rockBlocker.GetComponent<MeshRenderer>().enabled = true;

            //Attiva il boss
            BossSpawner bossSpawner = bossSpawnerObject.GetComponent<BossSpawner>();
            bossSpawner.enabled = true;

            //Play Sound
            Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.FallingRocks);

            //Play Boss Soundtrack
            Audio.SoundManager.PlaySoundtrack(Audio.SoundManager.SoundTracks.BossTrack);

            //Diminuisce la luce

            if (sun)
            {
                sun.GetComponent<Light>().intensity = 0;
            }

            gameObject.SetActive(false); //disattiva i lgameobject

        }
    }
}
