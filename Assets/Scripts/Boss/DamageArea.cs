﻿using Enemies;
using Player;
using UnityEngine;
using Utility;

namespace Boss
{
    public class DamageArea : MonoBehaviour
    {
        [SerializeField] private Transform boss;
        [SerializeField] private int absorbedHealth;
        [SerializeField] private int firstDamage;
        [SerializeField] private int addedDamage;

        private string timerName = "AREADAMAGE_TIMER";

        private EnemyStatus bossStatus;

        void Start()
        {
            bossStatus = boss.GetComponent<EnemyBase>().GetStatus();

            TimerController.AddTimer(timerName, 1f);
            TimerController.AddCurrentTime(timerName, 0f);
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Values.PlayerTag))
            {
                Values.DecreaseHealth(firstDamage);

                //Play sound effect
                Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.PlayerHurt);

                Values.DecreaseHealth(firstDamage);
                TimerController.ResetTimer(timerName);
            }
        }


        private void OnTriggerStay(Collider other)
        {
            //if player stays in, deal damage after a while
            //the boss gets some boosted health :) 

            if (other.CompareTag(Values.PlayerTag))
            {
                TimerController.RunTimer(timerName);

                if (TimerController.GetCurrentTime()[timerName] <= 0)
                {
                    if (bossStatus.GetHealth() + absorbedHealth < bossStatus.GetMaxHealth())
                    {
                        bossStatus.ModifyHealth(absorbedHealth);
                    }
                    else
                    {
                        bossStatus.SetHealth(bossStatus.GetMaxHealth());
                    }

                    Values.DecreaseHealth(addedDamage);
                    Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.PlayerHurt);
                    TimerController.ResetTimer(timerName);
                }
            }
        }
    }
}