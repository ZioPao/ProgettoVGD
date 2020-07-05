using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

namespace Enemies
{
    public class EnemyIntelligence : MonoBehaviour
    {
        [SerializeField] private int viewDistance = 10;
        [SerializeField] private float maxMemoryTime = 10f;
        
        private Transform playerTransform;
        
        private float memoryTimeLeft;
        private Transform frontEnemyTransform;
        private NavMeshAgent agent;

        private float waitingTimeLeft;
        private bool isPlayerInView;


        private void Start()
        {
            frontEnemyTransform = transform.Find("ViewCheck").Find("Front");
            playerTransform = GameObject.Find("Player").GetComponent<Transform>();

            waitingTimeLeft = 0f; //di default a zero
            agent = GetComponent<NavMeshAgent>();
            agent.isStopped = true;        //di base dev'essere fermo
        }


        private void FixedUpdate()
        {
            isPlayerInView = CheckIfPlayerIsInView();
            if (isPlayerInView)
            {
                if (waitingTimeLeft > 0)
                {
                    waitingTimeLeft -= Time.deltaTime;
                    return;
                }

                if (isPlayerInView)
                {
                    agent.isStopped = false;
                    memoryTimeLeft = maxMemoryTime;
                }

                //No memory time left, so the enemy will have to stop

                else if (memoryTimeLeft <= 0)
                {
                    agent.isStopped = true;
                }
                //Move towards the player and decrease the memory time of the enemy
                else
                {
                    memoryTimeLeft -= Time.deltaTime;
                }
            }
        }

        private bool CheckIfPlayerIsInView()
        {
            LayerMask tmp = ~ LayerMask.GetMask("Enemy"); //ignore viewchecks for sprite management

            if (!(Vector3.Distance(frontEnemyTransform.position, this.transform.position) < viewDistance)) return false;

            return Physics.Linecast(frontEnemyTransform.position, playerTransform.position,
                       out RaycastHit rayEnemySprite,
                       tmp) &&
                   rayEnemySprite.collider.CompareTag("Player");
        }


        public bool IsPlayerInView()
        {
            return isPlayerInView;
        }

        public float GetMemoryTimeLeft()
        {
            return memoryTimeLeft;
        }
    }
}