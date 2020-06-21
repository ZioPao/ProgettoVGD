using System.Collections;
using System.Collections.Generic;
using Unity.UNetWeaver;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    Transform playerTransform;
    NavMeshAgent agent;
    Transform textureRenderer;

    [SerializeField] private float memoryTime = 5f;        //How long does the enemy remember the player?
    [SerializeField] private float playerDistance = 5f;        //How far does he have to stay?
    [SerializeField] private float maxViewDistance = 150f;        //How far can he see?

    private float memoryTimeLeft = 0f;


    protected bool isPlayerInView = false;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        textureRenderer = transform.Find("Texture");

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //Look at player
        Vector3 targetPostition = new Vector3(playerTransform.position.x,
                                       textureRenderer.position.y,
                                       playerTransform.position.z);
        textureRenderer.LookAt(targetPostition);

        textureRenderer.rotation = Quaternion.Euler(90, textureRenderer.rotation.eulerAngles.y, textureRenderer.eulerAngles.z);

        /*Check wheter or not it spotted the player.*/
        isPlayerInView = CheckIfPlayerIsInView();

        if (isPlayerInView)
        {
            followPlayer();
            MoveTowardsPlayer();
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


    /*The enemy will try to follow the player and flank him. Will not get too close*/
    private void MoveTowardsPlayer()
    {
        if (Vector3.Distance(agent.nextPosition, playerTransform.position) <= playerDistance)
            print("Enemy is close");
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



    private bool CheckIfPlayerIsInView()
    {
        RaycastHit rayEnemySprite;
        //todo probabilmente meglio un layer apposito

        LayerMask tmp =~ LayerMask.GetMask("Enemy");        //ignore viewchecks for sprite management
        if (Vector3.Distance(playerTransform.position, transform.position) < maxViewDistance)

            if (Physics.Linecast(transform.position, playerTransform.position, out rayEnemySprite, tmp))
            {
                if (rayEnemySprite.collider.name.Equals("Player"));
                    return true;

            }

        return false;

    }


    //Getter

    public bool GetIsPlayerInView()
    {
        return isPlayerInView;
    }

}


