using UnityEngine;
using UnityEngine.AI;

namespace Logic.Enemy
{
    public class EnemyBehaviour : MonoBehaviour
    {
        private Transform playerTransform;
        private Transform frontEnemy;
        NavMeshAgent agent;
        Transform textureRenderer;

        [SerializeField] private float memoryTime = 5f; //How long does the enemy remember the player?
        [SerializeField] private float playerDistance = 5f; //How far does he have to stay?
        [SerializeField] private float maxViewDistance = 150f; //How far can he see?

        private float memoryTimeLeft = 0f;


        protected bool isPlayerInView = false;

        // Start is called before the first frame update
        void Start()
        {
            playerTransform = GameObject.Find("Player").GetComponent<Transform>();
            agent = GetComponent<NavMeshAgent>();
            textureRenderer = transform.Find("Texture");
            frontEnemy = transform.Find("ViewCheck").Find("Front");
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //Look at player
            Vector3 targetPostition = new Vector3(playerTransform.position.x,
                textureRenderer.position.y,
                playerTransform.position.z);
            textureRenderer.LookAt(targetPostition);

            textureRenderer.rotation =
                Quaternion.Euler(90, textureRenderer.rotation.eulerAngles.y, textureRenderer.eulerAngles.z);

            /*Check wheter or not it spotted the player.*/
            isPlayerInView = CheckIfPlayerIsInView();

            if (isPlayerInView)
            {
                FollowPlayer();
                MoveTowardsPlayer();
                memoryTimeLeft = memoryTime;
            }

            else
            {
                //No memory time left, so the enemy will have to stop
                if (memoryTimeLeft <= 0)
                {
                    StopFollowingPlayer();
                }
                else
                {
                    FollowPlayer();
                    memoryTimeLeft -= Time.deltaTime;
                }
            }
        }


        /*The enemy will try to follow the player and flank him. Will not get too close*/
        private void MoveTowardsPlayer()
        {
            if (Vector3.Distance(agent.nextPosition, playerTransform.position) <= playerDistance)
                print("");
        }

        private void FollowPlayer()
        {
            agent.isStopped = false;
            agent.destination = playerTransform.position;
        }

        private void StopFollowingPlayer()
        {
            agent.isStopped = true;
        }


        private bool CheckIfPlayerIsInView()
        {
            RaycastHit rayEnemySprite;
            //todo probabilmente meglio un layer apposito

            LayerMask tmp = ~ LayerMask.GetMask("Enemy"); //ignore viewchecks for sprite management
            Debug.DrawLine(frontEnemy.position, playerTransform.position);

            if (!(Vector3.Distance(frontEnemy.position, transform.position) < maxViewDistance)) return false;
        
            return Physics.Linecast(frontEnemy.position, playerTransform.position, out rayEnemySprite, tmp) && rayEnemySprite.collider.CompareTag("Player");
        }


        //Getter

        public bool GetIsPlayerInView()
        {
            return isPlayerInView;
        }
    }
}