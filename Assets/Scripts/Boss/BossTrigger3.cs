﻿using UnityEngine;
using Utility;

namespace Boss
{
    public class BossTrigger3 : MonoBehaviour, ITriggerMidGame
    {
        [SerializeField] private GameObject building1, building2;
        [SerializeField] private GameObject bossSpawnerObject;
        
        private Quaternion newRotation1 = Quaternion.Euler(86, 0,0);
        private Vector3 newPosition1 = new Vector3(204, 64, 192);

        private Quaternion newRotation2 = Quaternion.Euler(-73, 0, 0);
        private Vector3 newPosition2 = new Vector3(202, 64, 263);

        private bool alreadyExecuted = false;
    
        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Player") && !alreadyExecuted)
            {
                RunTrigger();
                alreadyExecuted = true;
            }
            
        }

        public void RunTrigger()
        {
            //Set buildings positions
            building1.transform.position = newPosition1;
            building1.transform.rotation = newRotation1;

            building2.transform.position = newPosition2;
            building2.transform.rotation = newRotation2;
            
            //Attiva il boss
            BossSpawner fakeBossSpawner = bossSpawnerObject.GetComponent<BossSpawner>();
            fakeBossSpawner.enabled = true;

            //Play Sound
            Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.FallingRocks);

            //Play Boss Soundtrack
            Audio.SoundManager.PlaySoundtrack(Audio.SoundManager.SoundTracks.BossTrack);
        }
    }
}