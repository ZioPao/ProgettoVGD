using System.Collections;
using Enemies;
using Player;
using UnityEngine;
using UnityEngine.AI;
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
        private EnemyMovement bossMovement;
        private EnemySpawner enemySpawner;


        private Transform textureTransform;
        
        private SpriteRenderer spriteRenderer;         //todo forse da cambiare in SpriteRenderer
        private Animator spriteAnimator;
        
        private bool isInPhaseTwo;
        
        
        
        
    /*  Inizialmente identico a Nemico 1, quando subisce una certa quantità di danni cambia fase diventando molto più
        aggressivo (Attacca sparando un maggior numero di proiettili)
    */
        private void Awake()
        {

            boss = GetComponent<EnemyBase>();
            bossMovement = GetComponent<EnemyMovement>();
            bossShooting = GetComponent<EnemyShooting>();

            textureTransform = transform.Find("Texture");
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            spriteAnimator = GetComponentInChildren<Animator>();
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
                    
                    //blocca il player 
                    //Values.SetIsFrozen(true);
                    var x =  Values.GetEnemySpritesManager();
                    x.enabled = false;

                    //bossMovement.enabled = false;
                    //boss.enabled = false;

                    StartCoroutine(WaitForAnimation());

                        
                    //spriteRenderer.material = materialPhaseTwo;
                
                    
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

        private IEnumerator WaitForAnimation()
        {
  

            boss.GetStatus().SetForceStop(true);
            
            spriteRenderer.sprite = Resources.Load<Sprite>("Enemies/Level1/Boss/Wakeup");
            spriteAnimator.runtimeAnimatorController =  Resources.Load("Enemies/Level1/Boss/Wakeup_AnimController") as RuntimeAnimatorController;

            textureTransform.position = textureTransform.position + new Vector3(0, 0.5f, 0);
            yield return new WaitForSecondsRealtime(1.3f);

            boss.GetStatus().SetForceStop(false);

            //Values.SetIsFrozen(false);
            Values.SetCurrentBoss(gameObject);
            Values.GetEnemySpritesManager().enabled = true;        //Reactivates everything else
        }
        private void SwitchSprite()
        {
            //Prende tutti gli sprite corretti
            
        }
    }
}
