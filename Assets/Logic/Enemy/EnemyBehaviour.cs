using UnityEngine;
using UnityEngine.AI;

namespace Logic.Enemy
{
    public class EnemyBehaviour : MonoBehaviour
    {
        


        [SerializeField] private float memoryTime = 100f; //How long does the enemy remember the player?
        [SerializeField] private float playerDistance = 5f; //How far does he have to stay?
        [SerializeField] private float maxViewDistance = 150f; //How far can he see?
        [SerializeField] private float maxTimerAlternativeMovement = 2f;

        //Various components
        private Transform playerTransform;
        private Transform frontEnemy;
        private Transform textureRenderer;
        private NavMeshAgent agent;

        //Enemy memory stuff
        private float memoryTimeLeft = 0f;
        private float timerAlternativeMovement = 0f;
        private Vector3 savedDestination;
        private bool isPlayerInView = false;

        private void Start()
        {
            playerTransform = GameObject.Find("Player").GetComponent<Transform>();
            agent = GetComponent<NavMeshAgent>();
            textureRenderer = transform.Find("Texture");
            frontEnemy = transform.Find("ViewCheck").Find("Front");
        }
        private void FixedUpdate()
        {
            //Manage the looking at player stuff
            var playerPosition = playerTransform.position;

            Vector3 targetPostition = new Vector3(playerPosition.x,
                textureRenderer.position.y,
                playerPosition.z);
            textureRenderer.LookAt(targetPostition);

            textureRenderer.rotation =
                Quaternion.Euler(90, textureRenderer.rotation.eulerAngles.y, textureRenderer.eulerAngles.z);

            /*Check whether or not it spotted the player.*/
            isPlayerInView = CheckIfPlayerIsInView();

            if (isPlayerInView)
            {
                agent.isStopped = false;
                MoveTowardsPlayer();
                memoryTimeLeft = memoryTime;
            }
            
            //No memory time left, so the enemy will have to stop

            else if (memoryTimeLeft <= 0)
            {
                agent.isStopped = true;
            }
            //Move towards the player and decrease the memory time of the enemy
            else
            {
                MoveTowardsPlayer();
                memoryTimeLeft -= Time.deltaTime;
            }
        }

        

        /*The enemy will try to follow the player and flank him. Will not get too close*/
        private void MoveTowardsPlayer()
        {
            //Se sta effettuando un movimento alternativo, continuerà per un certo tot
            if (timerAlternativeMovement > 0f)
            {
                timerAlternativeMovement -= Time.deltaTime;
                return;
            }


            //in caso contrario, operazione normale
            if (Vector3.Distance(agent.nextPosition, playerTransform.position) <= playerDistance)
            {
                timerAlternativeMovement = maxTimerAlternativeMovement;
                float randomChoice = Random.Range(0f, 1f);

                if (randomChoice < 0.5f)
                {
                    agent.destination = playerTransform.position + agent.transform.right * 100f;
                    ;
                }
                else
                {
                    agent.destination = playerTransform.position - agent.transform.right * 100f;
                    ;
                }
            }
            else
            {
                agent.destination = playerTransform.position;
            }
        }

        private bool CheckIfPlayerIsInView()
        {
            LayerMask tmp = ~ LayerMask.GetMask("Enemy"); //ignore viewchecks for sprite management

            if (!(Vector3.Distance(frontEnemy.position, transform.position) < maxViewDistance)) return false;

            return Physics.Linecast(frontEnemy.position, playerTransform.position, out RaycastHit rayEnemySprite,
                       tmp) &&
                   rayEnemySprite.collider.CompareTag("Player");
        }

        //Getter

        public bool GetIsPlayerInView()
        {
            return isPlayerInView;
        }
    }
}