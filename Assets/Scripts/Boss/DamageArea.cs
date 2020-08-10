using Enemies;
using Player;
using UnityEngine;
using Utility;

namespace Boss
{
    public class DamageArea : MonoBehaviour
    {

        [SerializeField] private Transform boss;
    
        private string timerName = "AREADAMAGE_TIMER";
        private bool shouldRunTimer = false;
    
        private EnemyStatus bossStatus;
        void Start()
        {
            TimerController.AddTimer(timerName, 1f);
            TimerController.AddCurrentTime(timerName, 0f);

            bossStatus = boss.GetComponent<EnemyBase>().GetStatus();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                shouldRunTimer = true;
                //decreases player health

                Values.DecreaseHealth(1);
            }
        }


        private void OnTriggerStay(Collider other)
        {
            //if player stays in, deal damage after a while
            //the boss gets some boosted health :) 

            if (other.CompareTag("Player"))
            {
                TimerController.RunTimer(timerName);

                if (TimerController.GetCurrentTime()[timerName] <= 0)
                {
                    bossStatus.ModifyHealth(100);
                    Values.DecreaseHealth(3);
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
}