using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class EnemyMovement : MonoBehaviour
    {

        [SerializeField] private float maxTimerAlternativeMovement;
        [SerializeField] private float minPlayerDistance = 5f; //How far does he have to stay?

        
        
        private float timerAlternativeMovement;
        private NavMeshAgent agent;
        private Transform playerTransform, textureRenderer;

        private void Start()
        {
            timerAlternativeMovement = 0f;
            agent = GetComponent<NavMeshAgent>();
            playerTransform = GameObject.Find("Player").GetComponent<Transform>();
            textureRenderer = transform.Find("Texture");

        }

        public void FixedUpdate()
        {
            
            //Se sta effettuando un movimento alternativo, continuerà per un certo tot
            if (timerAlternativeMovement > 0f)
            {
                timerAlternativeMovement -= Time.deltaTime;
                return;
            } //in caso contrario, operazione normale
            if (Vector3.Distance(agent.nextPosition, playerTransform.position) <= minPlayerDistance)
            {
                timerAlternativeMovement = maxTimerAlternativeMovement;
                float randomChoice = Random.Range(0f, 1f);

                if (randomChoice < 0.5f)
                {
                    agent.destination = playerTransform.position + agent.transform.right * 100f;
                    
                }
                else
                {
                    agent.destination = playerTransform.position - agent.transform.right * 100f;
                    
                }
            }
            else
            {
                agent.destination = playerTransform.position;
            }

        }
        

        public void LookPlayer()
        {
            //Manage the looking at player stuff
            Vector3 playerPosition = playerTransform.position;
            textureRenderer.LookAt(new Vector3(playerPosition.x,
                textureRenderer.position.y,
                playerPosition.z));

            textureRenderer.rotation =
                Quaternion.Euler(90, textureRenderer.rotation.eulerAngles.y, textureRenderer.eulerAngles.z);

        }
    }
}