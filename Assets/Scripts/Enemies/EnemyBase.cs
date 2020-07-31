using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Utility;

namespace Enemies
{
    public class EnemyBase : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;
        
        private GameObject hitMarker;

        private EnemyStatus status;
        private EnemyIntelligence enemyIntelligence;
        private EnemyMovement enemyMovement;
        private EnemyShooting enemyShooting;

        private string timerName;
        private bool isReloading = false;

        private void Awake()
        {
            if (!isReloading)
            {
                status = new EnemyStatus();
                status.SetupBase(maxHealth, maxHealth, false);
                status.SetName(gameObject.name);
            }
            else
            {
                name = status.GetName();
            }
     
            

            
            //Modules
            enemyIntelligence = GetComponent<EnemyIntelligence>();
            enemyMovement = GetComponent<EnemyMovement>();
            enemyShooting = GetComponent<EnemyShooting>();

            hitMarker = transform.Find("Texture").Find("Hitmarker").gameObject;

            //Animation timer
            StartCoroutine(Values.WaitForTimer());
            timerName = name.ToUpper() + "_K";
            TimerController.AddTimer(timerName, 10f); //todo switch case per ogni tot per inserire il frame
            TimerController.AddCurrentTime(timerName, 0f);


        }

        // Update is called once per frame
        void FixedUpdate()
        {
            TimerController.RunTimer(timerName);
            TimerController.RunTimer(TimerController.HITMARKER_K);


            //Loops it
            if (TimerController.GetCurrentTime()[timerName] <= 0)
                TimerController.ResetTimer(timerName);

            //Hitmarkers
            if (status.GetIsHit())
            {
                if (TimerController.GetCurrentTime()[TimerController.HITMARKER_K] <= 0)
                {
                    hitMarker.SetActive(false);
                    status.SetIsHit(false);
                }
            }

            //Manage the looking at player stuff
            enemyMovement.LookPlayer();


            if (!status.GetForceStop())
            {            
                /*Check whether or not it spotted the player.*/

                if (status.GetIsPlayerInView() && enemyIntelligence.GetMemoryTimeLeft() > 0)
                {
                    enemyShooting.Shoot();
                }

                /*Check health to destroy the object*/
                CheckHealth();

                status.SaveRotation(transform.rotation);
                status.SavePosition(transform.position);
            }
          
        }

        private void CheckHealth()
        {
            if (status.GetHealth() <= 0)
                Destroy(gameObject);
        }

     

        public float GetAnimationTimer()
        {
            return TimerController.GetCurrentTime()[timerName];
        }


        public void SetDamage(int hit)
        {
            status.ModifyHealth(-hit);

            //Create a hit on the model and make it stay on the model for some time
            hitMarker.SetActive(true);
            status.SetIsHit(true);
            TimerController.ResetTimer(TimerController.HITMARKER_K);

            //when hit, the enemy should know the player location for a second or so
            enemyIntelligence.AlertEnemy();
        }

        public void Reload(EnemyStatus status)
        {
            this.status = status;

            isReloading = true;
            Awake();        //Reload everything about the enemy
            isReloading = false;
        }
        public EnemyStatus GetStatus()
        {
            return status;
        }

        public void SetName(string name)
        {
            gameObject.name = name;
        }
        
    }
}