using System;
using Player;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Enemies
{
    [Serializable]

    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float maxTimerAlternativeMovement;
        [SerializeField] private float minPlayerDistance = 5f; //How far does he have to stay?
        
        private EnemyStatus status;
        private NavMeshAgent agent;
        private Transform textureRenderer;
        
        private void Awake()
        {
            status = GetComponent<EnemyBase>().GetStatus();
            
            agent = GetComponent<NavMeshAgent>();
            textureRenderer = transform.Find("Texture");
            
            //Init variabili
            status.SetupMovement(maxTimerAlternativeMovement, 0f, minPlayerDistance);

        }

        public void FixedUpdate()
        {
            
            //Se sta effettuando un movimento alternativo, continuerà per un certo tot

            if (!status.GetForceStop())
            {
                float timerAlternativeMovement = status.GetTimerAlternativeMovement();
                
                if (timerAlternativeMovement > 0f)
                {
                    status.ModifyTimerAlternativeMovement(-Time.deltaTime);
                    return;
                } //in caso contrario, operazione normale
                if (Vector3.Distance(agent.nextPosition, Values.GetPlayerTransform().position) <= minPlayerDistance)
                {
                    status.SetTimerAlternativeMovement(maxTimerAlternativeMovement);
                    float randomChoice = Random.Range(0f, 1f);

                    if (randomChoice < 0.5f)
                    {
                        agent.destination = Values.GetPlayerTransform().position + agent.transform.right * 100f;
                    
                    }
                    else
                    {
                        agent.destination = Values.GetPlayerTransform().position - agent.transform.right * 100f;
                    
                    }
                }
                else
                {
                    agent.destination = Values.GetPlayerTransform().position;
                }
            }
            else
            {
                agent.destination = transform.position;        //just in case it keeps moving

            }

        }
        

        public void LookPlayer()
        {
            //Manage the looking at player stuff
            Vector3 playerPosition = Values.GetPlayerTransform().position;
            textureRenderer.LookAt(new Vector3(playerPosition.x,
                textureRenderer.position.y,
                playerPosition.z));

        }


        public void Reload()
        {
            Awake();            //non ho idea del percHé funziona tbh
            agent.Warp(transform.position);        //forces it 
        }


    }
}