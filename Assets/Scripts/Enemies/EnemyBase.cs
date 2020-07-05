using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class EnemyBase : MonoBehaviour{

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
    }
}