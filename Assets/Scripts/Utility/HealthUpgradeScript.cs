using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using Utility;

public class HealthUpgradeScript : MonoBehaviour, IPickup
{
    // Start is called before the first frame update
    void FixedUpdate()
    {
        this.transform.Rotate(0, 2, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Values.PlayerTag))
        {
            Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.HealthPickup);
            Values.SetMaxHealth(Values.GetMaxHealth()+ 50);
            Values.SetHealth(Values.GetMaxHealth());
            Destroy(gameObject);
        }
    }
    public Values.PickupEnum GetPickupType()
    {
        return Values.PickupEnum.HealthUpgrade;
    }
}
