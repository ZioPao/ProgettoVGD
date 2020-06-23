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

        [SerializeField] private float memoryTime = 100f; //How long does the enemy remember the player?
        [SerializeField] private float playerDistance = 5f; //How far does he have to stay?
        [SerializeField] private float maxViewDistance = 150f; //How far can he see?
        [SerializeField] private float maxTimerAlternativeMovement = 2f;
        
        private float memoryTimeLeft = 0f;
        private float timerAlternativeMovement = 0f;

        private Vector3 savedDestination;


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
                agent.isStopped = false;
                MoveTowardsPlayer();
                memoryTimeLeft = memoryTime;
            }

            else
            {
                //No memory time left, so the enemy will have to stop
                if (memoryTimeLeft <= 0)
                {
                    agent.isStopped = true;
                }
                else
                {
                    MoveTowardsPlayer();
                    memoryTimeLeft -= Time.deltaTime;
                }
            }
        }


        /*The enemy will try to follow the player and flank him. Will not get too close*/
        private void MoveTowardsPlayer()
        {

            Debug.DrawLine(agent.transform.position, agent.destination);

            if (timerAlternativeMovement > 0f)
            {
                timerAlternativeMovement -= Time.deltaTime;
                return;

            }
            
            
            if (Vector3.Distance(agent.nextPosition, playerTransform.position) <= playerDistance)
            {
                timerAlternativeMovement = maxTimerAlternativeMovement;
                
                
                print("nemico vicino a player");

                float randomChoice = Random.Range(0f, 1f);

                if (randomChoice < 0.5f)
                {
                    agent.destination = playerTransform.position + agent.transform.right * 100f;;

                }
                else
                {
                    agent.destination = playerTransform.position - agent.transform.right * 100f;;
                }
                
            }
            else
            {
                agent.destination = playerTransform.position;
            }
        }


        private bool CheckIfPlayerIsInView()
        {
            RaycastHit rayEnemySprite;
            //todo probabilmente meglio un layer apposito

            LayerMask tmp = ~ LayerMask.GetMask("Enemy"); //ignore viewchecks for sprite management

            if (!(Vector3.Distance(frontEnemy.position, transform.position) < maxViewDistance)) return false;

            return Physics.Linecast(frontEnemy.position, playerTransform.position, out rayEnemySprite, tmp) &&
                   rayEnemySprite.collider.CompareTag("Player");
        }


        //Getter

        public bool GetIsPlayerInView()
        {
            return isPlayerInView;
        }
    }
}