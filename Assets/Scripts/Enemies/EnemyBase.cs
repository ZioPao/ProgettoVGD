using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Utility;

namespace Enemies
{
    public class EnemyBase : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;

        private int health;
        private GameObject hitMarker;

        private EnemyIntelligence enemyIntelligence;
        private EnemyMovement enemyMovement;
        private EnemyShooting enemyShooting;

        private string timerName;
        private bool isHit;

        void Start()
        {
            //Modules
            enemyIntelligence = GetComponent<EnemyIntelligence>();
            enemyMovement = GetComponent<EnemyMovement>();
            enemyShooting = GetComponent<EnemyShooting>();

            hitMarker = transform.Find("Texture").Find("Hitmarker").gameObject;

            //Animation timer
            timerName = name.ToUpper() + "_K";
            TimerController.AddTimer(timerName, 10f); //todo switch case per ogni tot per inserire il frame
            TimerController.AddCurrentTime(timerName, 0f);

            //Startup
            isHit = false;
            health = maxHealth;
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
            if (isHit)
            {
                if (TimerController.GetCurrentTime()[TimerController.HITMARKER_K] <= 0)
                {
                    hitMarker.SetActive(false);
                    isHit = false;
                }
            }

            //Manage the looking at player stuff
            enemyMovement.LookPlayer();

            /*Check whether or not it spotted the player.*/
            if (enemyIntelligence.IsPlayerInView() && enemyIntelligence.GetMemoryTimeLeft() > 0)
            {
                enemyShooting.Shoot();
            }

            /*Check health*/
            CheckHealth();
        }

        private void CheckHealth()
        {
            if (health <= 0)
                Destroy(gameObject);
        }

        public int GetHealth()
        {
            return health;
        }

        public float GetAnimationTimer()
        {
            return TimerController.GetCurrentTime()[timerName];
        }


        public void SetHealth(int h)
        {
            health = h;
        }

        public void SetDamage(int hit)
        {
            health -= hit;

            //Create a hit on the model and make it stay on the model for some time
            hitMarker.SetActive(true);
            isHit = true;
            TimerController.ResetTimer(TimerController.HITMARKER_K);

            //when hit, the enemy should know the player location for a second or so
            enemyIntelligence.AlertEnemy();
        }
    }
}