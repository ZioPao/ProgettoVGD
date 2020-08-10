using Player;
using UnityEngine;
using Utility;

public class DamageArea : MonoBehaviour
{
    private string timerName = "AREADAMAGE_TIMER";
    private bool shouldRunTimer = false;

    void Start()
    {
        TimerController.AddTimer(timerName, 1f);
        TimerController.AddCurrentTime(timerName, 0f);
    }

    // Update is called once per frame
    void Update()
    {
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldRunTimer = true;
            //decreases player health

            //Play sound effect
            Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.PlayerHurt);

            Values.DecreaseHealth(2);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        //if player stays in, deal damage after a while

        if (other.CompareTag("Player"))
        {
            TimerController.RunTimer(timerName);

            if (TimerController.GetCurrentTime()[timerName] <= 0)
            {
                //Play sound effect
                Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.PlayerHurt);

                Values.DecreaseHealth(10);
                TimerController.ResetTimer(timerName);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldRunTimer = false;
        }
    }
}