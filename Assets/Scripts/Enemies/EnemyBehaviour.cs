using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class EnemyBehaviour : MonoBehaviour
    {
        //todo da separare in più moduli

        //Timers

        [SerializeField] private float memoryTime = 100f; //How long does the enemy remember the player?
        [SerializeField] private float playerDistance = 5f; //How far does he have to stay?
        [SerializeField] private float maxViewDistance = 150f; //How far can he see?
        [SerializeField] private float maxTimerAlternativeMovement = 2f;

        [SerializeField] private float waitingTime = 1f;
        private float waitingTimeLeft;

        //Various components
        private Transform playerTransform;
        private Transform frontEnemy;
        private Transform textureRenderer;
        private NavMeshAgent agent;
        private EnemyShooting enemyShootingScript;

        //Enemy memory stuff
        private float memoryTimeLeft = 0f;
        private float timerAlternativeMovement = 0f;
        private Vector3 savedDestination;
        private bool isPlayerInView = false;
        
        
        

        private void Start()
        {
            playerTransform = GameObject.Find("Player").GetComponent<Transform>();
            agent = GetComponent<NavMeshAgent>();
            enemyShootingScript = GetComponentInChildren<EnemyShooting>();
            textureRenderer = transform.Find("Texture");
            frontEnemy = transform.Find("ViewCheck").Find("Front");
            waitingTimeLeft = 0;        //di default a zero
        }
        private void FixedUpdate()
        {
            //Manage the looking at player stuff
            Vector3 playerPosition = playerTransform.position;
            Vector3 targetPostition = new Vector3(playerPosition.x,
                textureRenderer.position.y,
                playerPosition.z);
            textureRenderer.LookAt(targetPostition);

            textureRenderer.rotation =
                Quaternion.Euler(90, textureRenderer.rotation.eulerAngles.y, textureRenderer.eulerAngles.z);

            /*Check whether or not it spotted the player.*/
            isPlayerInView = CheckIfPlayerIsInView();
            
            //todo da sistemare


            if (waitingTimeLeft > 0)
            {
                waitingTimeLeft -= Time.deltaTime;
                return;
            }
            
            
            if (enemyShootingScript.IsEnemyShooting())
            {
                agent.isStopped = true;
                waitingTimeLeft = waitingTime;        //set del timer d'attesa
                return;
            }
            
            
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

            //todo is it broken?
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