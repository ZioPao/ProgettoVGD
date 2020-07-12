using Enemies;
using UnityEngine;
using Utility;


namespace Boss
{

    public class BossOne : MonoBehaviour
    {


        [SerializeField] private int bossHealth = 150;
        [SerializeField] private int bossProjectileSpeed = 20;
        [SerializeField] private float bossProjectileRate;
        [SerializeField] private Material materialPhaseTwo;

        
        private EnemyBase boss;
        private EnemyShooting bossShooting;
        private EnemySpawner enemySpawner;
        private SpriteRenderer spriteRenderer;         //todo forse da cambiare in SpriteRenderer
        
        private bool isInPhaseTwo;
        
        
        
        
    /*  Inizialmente identico a Nemico 1, quando subisce una certa quantità di danni cambia fase diventando molto più
        aggressivo (Attacca sparando un maggior numero di proiettili)
    */
        private void Start()
        {

            boss = GetComponent<EnemyBase>();
            bossShooting = GetComponent<EnemyShooting>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            //enemySpawner = gameObject.AddComponent<EnemySpawner>();        //per spawnare i nemici nella fase 2    
            isInPhaseTwo = false;
            //Spawn

            //SpawnEnemy(chicken1);
        }

        private void FixedUpdate()
        {

            if (!isInPhaseTwo)
            {
                EnemyStatus bossStatus = boss.GetStatus();
                if (bossStatus.GetHealth() < 50)
                {
                    //Change appearance
                    spriteRenderer.material = materialPhaseTwo;
                
                    //Reset health
                    bossStatus.SetHealth(bossHealth);

                    //Increase projectile rate and speed
                    bossShooting.SetProjectileSpawnRate(bossProjectileRate);
                    bossShooting.SetProjectileSpeed(bossProjectileSpeed);


                    isInPhaseTwo = true;
                };
                
                
             

            }
            
            //check health. When health
            //Movement is managed by BossBase... I guess?
            
            
        }
    }
}
