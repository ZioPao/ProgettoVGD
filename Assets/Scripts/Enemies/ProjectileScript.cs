using Player;
using UnityEngine;

namespace Enemies
{
    public class ProjectileScript : MonoBehaviour
    {

        [SerializeField] private float projectileTimeMax = 15f;
        [SerializeField] private int projectileSpeed = 15;
        [SerializeField] private int damage = 20;

        private ProjectileStatus status;
        private Transform enemyTransform;
        private Transform spriteTransform;

        private bool isReloading = false;

        private void Start()
        {

            if (!isReloading)
            {
                status = new ProjectileStatus(projectileTimeMax, projectileSpeed, gameObject);

            }
            spriteTransform = gameObject.transform.Find("Sprite");        //non ha senso salvarlo nello status
        }

        private void FixedUpdate()
        {
            
            //Updates transform
            status.SetTransform(transform);
            
            if (status.GetProjectileTimeLeft() > 0)
            {
                LookPlayer();
                
                //Movimento
                transform.position += transform.forward * (Time.deltaTime * status.GetProjectileSpeed());
                status.DecreaseTimer();

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
        
        //Getters
        public ProjectileStatus GetStatus()
        {
            return status;
        }
        
        //Setters
        public void SetSpeed(int speed)
        {
            projectileSpeed = speed;
        }

        public void SetTransform(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }

        public void Reload(ProjectileStatus status)
        {
            this.status = status;

            isReloading = true;
            Start();
            isReloading = false;


        }
        //Collisions with player
        private void OnTriggerEnter(Collider c)
        {

            //Set Damage to player

            if (c.gameObject.CompareTag(Values.playerTag))
            {
                Values.DecreaseHealth(damage);

                //Play sound effect
                Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.PlayerHurt);

                Destroy(gameObject);        //Destroys itself
            }
 
        }
        
        
        
        
        
    }
    
    
}
