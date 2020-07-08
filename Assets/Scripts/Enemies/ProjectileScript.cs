using Player;
using UnityEngine;

namespace Enemies
{
    public class ProjectileScript : MonoBehaviour
    {

        [SerializeField] private float projectileTimeMax = 15f;
        [SerializeField] private int projectileSpeed = 15;

        private float projectileTimeLeft;
        private Transform enemyTransform;
        private Transform spriteTransform;

        private void Start()
        {
            projectileTimeLeft = projectileTimeMax;
            spriteTransform = gameObject.transform.Find("Sprite");
        }

        private void FixedUpdate()
        {
            if (projectileTimeLeft > 0)
            {
                LookPlayer();
                
                transform.position += transform.forward * (Time.deltaTime * projectileSpeed);
                projectileTimeLeft -= Time.deltaTime;

            }
            else
            {
                Destroy(gameObject);
            }

        }
        public void LookPlayer()
        {
            //Manage the looking at player stuff
            Vector3 playerPosition = Values.GetPlayerTransform().position;
            spriteTransform.LookAt(new Vector3(playerPosition.x,
                spriteTransform.position.y,
                playerPosition.z));

        }
        public void SetSpeed(int speed)
        {
            projectileSpeed = speed;
        }
        
    }
}
