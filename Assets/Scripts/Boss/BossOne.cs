using System.Collections;
using Enemies;
using Player;
using UnityEngine;
using UnityEngine.AI;
using Utility;


namespace Boss
{
    public class BossOne : MonoBehaviour, IBoss
    {
        [SerializeField] private int bossHealth = 150;
        [SerializeField] private int bossProjectileSpeed = 20;
        [SerializeField] private float bossProjectileRate;
        [SerializeField] private float textureTransformModifier;

        private BossEndTrigger pathUnlocker;
        private EnemyBase boss;
        private EnemyStatus bossStatus;
        private EnemyShooting bossShooting;
        private EnemySpawner enemySpawner;


        //Graphical stuff
        private Transform textureTransform;
        private SpriteRenderer spriteRenderer;
        private Animator spriteAnimator;

        //Name
        private GameObject Phase1Name;
        private GameObject Phase2Name;

        //Change phase
        private bool isInPhaseTwo;


        /*  Inizialmente identico a Nemico 1, quando subisce una certa quantità di danni cambia fase diventando molto più
            aggressivo (Attacca sparando un maggior numero di proiettili)
        */
        private void Awake()
        {
            boss = GetComponent<EnemyBase>();
            bossShooting = GetComponent<EnemyShooting>();
            bossStatus = boss.GetStatus();

            textureTransform = transform.Find("Texture");
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            spriteAnimator = GetComponentInChildren<Animator>();
            //enemySpawner = gameObject.AddComponent<EnemySpawner>();        //per spawnare i nemici nella fase 2    
            isInPhaseTwo = false;

            //Gets boss names
            var texture = transform.Find("Texture");
            Phase1Name = texture.Find("NamePhase1").gameObject;
            Phase2Name = texture.Find("NamePhase2").gameObject;

            FindPathUnlocker();
            //Non lo attiva fino alla seconda fase per motivi di sprites

            
            Values.SetCanSave(false);
        }

        private void FixedUpdate()
        {
            if (!isInPhaseTwo)
            {
                if (bossStatus.GetHealth() < 50)
                {
                    ChangePhase();
                }
            }

            //check health. When health
            //Movement is managed by BossBase... I guess?
        }


        public void ChangePhase()
        {
            //Ferma il check degli sprites momentaneamente per metterglielo front ed evitare cambi strani
            Values.GetEnemySpritesManager().enabled = false;

            //Stops the player and let the enemy do an animation
            StartCoroutine(WaitForAnimation());

            //Reset health
            bossStatus.SetHealth(bossHealth);

            //Change name
            Phase1Name.SetActive(false);
            Phase2Name.SetActive(true);

            //Increase projectile rate and speed
            bossShooting.SetProjectileSpawnRate(bossProjectileRate);
            bossShooting.SetProjectileSpeed(bossProjectileSpeed);

            isInPhaseTwo = true;
        }

        private IEnumerator WaitForAnimation()
        {
            boss.GetStatus().SetForceStop(true);

            spriteRenderer.sprite = Resources.Load<Sprite>("Enemies/Level1/Boss/Wakeup");
            spriteAnimator.runtimeAnimatorController =
                Resources.Load("Enemies/Level1/Boss/Wakeup_AnimController") as RuntimeAnimatorController;

            textureTransform.position = textureTransform.position + new Vector3(0, textureTransformModifier, 0);
            yield return new WaitForSecondsRealtime(1.62f);

            boss.GetStatus().SetForceStop(false);

            //Values.SetIsFrozen(false);
            Values.SetCurrentBoss(gameObject);
            ActivatePathUnlocker(); //Solo dopo che il current boss è stato inserito
            Values.GetEnemySpritesManager().enabled = true; //Reactivates everything else
        }

        public void FindPathUnlocker()
        {
            pathUnlocker = GameObject.Find("Triggers").GetComponentInChildren<BossEndTrigger>();

        }
        public void ActivatePathUnlocker()
        {
            pathUnlocker.enabled = true;
        }
    }
}