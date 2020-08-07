using Player;
using UnityEngine;

namespace Utility
{
    public class AmmoBoxScript : MonoBehaviour, IPickup
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Values.PlayerTag) && Values.GetCurrentWeapon() != Values.WeaponEnum.Knife)
            {
                //Play Sound
                Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.CollectiblePickup);

                Values.IncrementAmmoReserve(Values.GetCurrentWeapon(), 15);
                Destroy(gameObject);
            }
        }


        public Values.PickupEnum GetPickupType()
        {
            return Values.PickupEnum.AmmoBox;
        }
    }
}