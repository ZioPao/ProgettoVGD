using Player;
using UnityEngine;

namespace Utility
{
    public class StaminaUpgradeScript : MonoBehaviour, IPickup
    {
        // Update is called once per frame
        void FixedUpdate()
        {
            this.transform.Rotate(0, 2, 0, 0);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Values.PlayerTag))
            {
                Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.HealthPickup);

                Values.SetMaxStamina(Values.GetMaxStamina() + 25);
                Values.SetStamina(Values.GetMaxStamina());
                Destroy(gameObject);
            }

        }

        public Values.PickupEnum GetPickupType()
        {
            return Values.PickupEnum.StaminaUpgrade;
        }
    }
}
