using Enemies;
using UnityEngine;


namespace Boss
{

    public class BossOne : MonoBehaviour
    {

        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int projectileRateMax = 100;
        [SerializeField] private int projectileSpeedMax = 50;
        
        private BossBase boss;
        private EnemySpawner enemySpawner;
        //private SpriteRenderer renderer; 
        
        private bool isInPhaseTwo;
        
        
    /*  Inizialmente identico a Nemico 1, quando subisce una certa quantità di danni cambia fase diventando molto più
        aggressivo (Attacca sparando un maggior numero di proiettili)
    */
        private void Start()
        {

            boss = new BossBase(maxHealth);
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
                boss.SetHealth(maxHealth);

                //Increase projectile rate and speed
                boss.SetProjectileSpawnRate(projectileRateMax);
                boss.SetProjectileSpeed(projectileSpeedMax);


                isInPhaseTwo = true;

            }
            
            //check health. When health
            //Movement is managed by BossBase... I guess?
            
            
        }
    }
}
