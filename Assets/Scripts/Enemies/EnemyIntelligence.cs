using System;
using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

namespace Enemies
{
    [Serializable]

    public class EnemyIntelligence : MonoBehaviour
    {
        [SerializeField] private int viewDistance = 10;
        [SerializeField] private float maxMemoryTime = 10f;

        private EnemyStatus status;
        
        
        private float memoryTimeLeft;
        private Transform frontEnemyTransform;
        private NavMeshAgent agent;

        public void Awake()
        {

            status = GetComponent<EnemyBase>().GetStatus();
            status.SetupIntelligence(viewDistance, false, true, maxMemoryTime, 0f, 0f);
            
            frontEnemyTransform = transform.Find("ViewCheck").Find("Front");

            agent = GetComponent<NavMeshAgent>();
            try
            {
                agent.isStopped = true; //di base dev'essere fermo

            }
            catch (Exception)
            {
                Destroy(this);
            }
        }


        private void FixedUpdate()
        {
            status.SetIsPlayerInView(CheckIfPlayerIsInView());

            bool isPlayerInView = status.GetIsPlayerInView();
            
            if (isPlayerInView)
            {
                if (status.GetWaitingTimeLeft() > 0)
                {
                    status.ModifyWaitingTimeLeft(-Time.deltaTime);
                    return;
                }

                
                //todo this stuff is kinda broken now
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

            return Physics.Linecast(frontEnemyTransform.position, Values.GetPlayerTransform().position,
                       out RaycastHit rayEnemySprite,
                       tmp) &&
                   rayEnemySprite.collider.CompareTag("Player");
        }

        public void AlertEnemy()
        {
            //In case we need to alert manually the ai of the player presence
            agent.isStopped = false;
            memoryTimeLeft = maxMemoryTime;
        }
        
        public float GetMemoryTimeLeft()
        {
            return memoryTimeLeft;
        }

        public void SetAgentStoppedStatus(bool value)
        {
            agent.isStopped = value;
            status.SetIsStopped(value);
        }
    }
}