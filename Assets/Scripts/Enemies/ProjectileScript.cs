using Player;
using UnityEngine;

namespace Enemies
{
    public class ProjectileScript : MonoBehaviour
    {

        [SerializeField] private float projectileTimeMax = 15f;
        [SerializeField] private int projectileSpeed = 15;
        [SerializeField] private int damage = 20;

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
        
        private void LookPlayer()
        {
            //It should face the player
            Vector3 playerPosition = Values.GetPlayerTransform().position;
            spriteTransform.LookAt(new Vector3(playerPosition.x,
                spriteTransform.position.y,
                playerPosition.z));

        }
        
        
        //Setters
        public void SetSpeed(int speed)
        {
            projectileSpeed = speed;
        }
        
        //Collisions with player
        private void OnTriggerEnter(Collider c)
        {

            //Set Damage to player

            if (c.gameObject.name.Equals("Player"))
            {
                Values.SetDamage(damage);

                Destroy(gameObject);        //Destroys itself
            }
 
        }
        
        
        
        
        
    }
    
    
}
