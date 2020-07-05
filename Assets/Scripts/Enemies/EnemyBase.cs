﻿using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class EnemyBase : MonoBehaviour
    {

        [SerializeField] private int maxHealth = 100;


        private int health;
        
        private EnemyIntelligence enemyIntelligence;
        private EnemyMovement enemyMovement;
        private EnemyShooting enemyShooting;

        void Start()
        {
            //Modules
            enemyIntelligence = GetComponent<EnemyIntelligence>();
            enemyMovement = GetComponent<EnemyMovement>();
            enemyShooting = GetComponent<EnemyShooting>();

            //Rendering stuff todo rivedi
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //Manage the looking at player stuff
            enemyMovement.LookPlayer();

            /*Check whether or not it spotted the player.*/
            if (enemyIntelligence.IsPlayerInView() && enemyIntelligence.GetMemoryTimeLeft() > 0)
            {
                enemyShooting.Shoot();
            }
        }


        public int GetHealth()
        {
            return health;
        }
        public void SetHealth(int h)
        {
            health = h;
        }


        public int DecreaseHealth(int hit)
        {
            health -= hit;
            return health;
        }
    }
}