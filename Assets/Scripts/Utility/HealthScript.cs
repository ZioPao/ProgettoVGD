using System;
using Player;
using UnityEngine;

namespace Utility
{
    public class HealthScript : MonoBehaviour, IPickup
    {
        // Update is called once per frame
        void FixedUpdate()
        {
            this.transform.Rotate(0, 2, 0, 0);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Values.PlayerTag) && Values.GetHealth() < 100)
            {
                Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.HealthPickup);
                Values.AddHealth(25);
                Destroy(gameObject);
            }

            //when players touch it automatically add tot health to him
        }

        public Values.PickupEnum GetPickupType()
        {
            return Values.PickupEnum.HealthPack;
        }
        
    }
}

