using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Rotate(0, 2, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Values.GetHealth() < 100)
        {
            Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.Sound.HealthPickup);
            Values.AddHealth(25);
            Destroy(gameObject);
        }

        //when players touch it automatically add tot health to him
    }
	
}    
