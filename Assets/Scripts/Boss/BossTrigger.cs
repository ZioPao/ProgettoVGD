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
                //Disabilita il salvataggio durante le boss battles
                Values.SetCanSave(false);
                
                //Blocks the door
                BlockEntrance();
                
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

                enabled = false;        //Disattiva script
                GetComponent<Collider>().enabled = false;        //Disattiva trigger
            }
         
        }

        public void RunTrigger()
        {

           BlockEntrance();

        }

        private void BlockEntrance()
        {
            //riattiva la roccia e fa un rumorone
            rockBlocker.GetComponent<MeshCollider>().enabled = true;
            rockBlocker.GetComponent<MeshRenderer>().enabled = true;
            
            enabled = false;        //Disattiva script
            GetComponent<Collider>().enabled = false;        //Disattiva trigger
        }
    }
}
