using Player;
using UnityEngine;
using Utility;

public class KeyScript : MonoBehaviour, IPickup
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Values.PlayerTag))
        {
            //Play Audio
            Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.CollectiblePickup);

            Values.SetHasKey(true);
            Destroy(gameObject);
        }

    }

    public Values.PickupEnum GetPickupType()
    {
        return Values.PickupEnum.Key;
    }
}
