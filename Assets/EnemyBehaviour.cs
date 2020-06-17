using System.Collections;
using System.Collections.Generic;
using Unity.UNetWeaver;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    Transform playerTransform;
    NavMeshAgent agent;
    SpottingScript visibilityCone;

    [SerializeField] private float memoryTime = 5f;        //How long does the enemy remember the player?
    private float memoryTimeLeft = 0f;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        visibilityCone = GameObject.Find("VisibilityCone").GetComponent<SpottingScript>();
        agent = GetComponent<NavMeshAgent>();


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Check wheter or not it spotted the player. 
        if (visibilityCone.isPlayerVisible)
        {
            followPlayer();
            memoryTimeLeft = memoryTime;
        }

        else
        {
            //No memory time left, so the enemy will have to stop
            if (memoryTimeLeft <= 0)
            {
                stopFollowingPlayer();

            }
            else
            {
                followPlayer();
                memoryTimeLeft -= Time.deltaTime;
            }

        }
    }


    private void followPlayer()
    {
        agent.isStopped = false;
        agent.destination = playerTransform.position;

    }

    private void stopFollowingPlayer()
        {
        agent.isStopped = true;

        }
        /** SETUP
         */


    }
