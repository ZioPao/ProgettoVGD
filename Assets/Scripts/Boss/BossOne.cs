using Enemies;
using UnityEngine;


namespace Boss
{

    public class BossOne : MonoBehaviour
    {


        [SerializeField] private int bossHealth = 150;
        [SerializeField] private int bossProjectileSpeed = 100;
        [SerializeField] private int bossProjectileRate = 150;

        
        private EnemyBase boss;
        private EnemyShooting bossShooting;
        private EnemySpawner enemySpawner;
        //private SpriteRenderer renderer; 
        
        private bool isInPhaseTwo;
        
        
        
        
    /*  Inizialmente identico a Nemico 1, quando subisce una certa quantità di danni cambia fase diventando molto più
        aggressivo (Attacca sparando un maggior numero di proiettili)
    */
        private void Start()
        {

            boss = GetComponent<EnemyBase>();
            bossShooting = GetComponent<EnemyShooting>();
            enemySpawner = gameObject.AddComponent<EnemySpawner>();        //per spawnare i nemici nella fase 2    
            isInPhaseTwo = false;
            //Spawn

            //SpawnEnemy(chicken1);
        }

        private void FixedUpdate()
        {

            if (!isInPhaseTwo)
            {
                if (boss.GetHealth() > 50) return;
                
                
                //Change appearance
                
                //renderer.material.mainTexture = "...";
                
                //Reset health
                boss.SetHealth(bossHealth);

                //Increase projectile rate and speed
                bossShooting.SetProjectileSpawnRate(bossProjectileRate);
                bossShooting.SetProjectileSpeed(bossProjectileSpeed);


                isInPhaseTwo = true;

            }
            
            //check health. When health
            //Movement is managed by BossBase... I guess?
            
            
        }
    }
}
