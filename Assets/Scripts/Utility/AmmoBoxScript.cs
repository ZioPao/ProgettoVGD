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
                Values.IncrementAmmoReserve(Values.GetCurrentWeapon(), 25);
                Destroy(gameObject);
            }
        }


        public Values.PickupEnum GetPickupType()
        {
            return Values.PickupEnum.AmmoBox;
        }
    }
}