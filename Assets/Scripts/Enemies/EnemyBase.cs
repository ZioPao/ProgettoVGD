using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Utility;

namespace Enemies
{
    public class EnemyBase : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private Sprite hitmarkerSprite;
        [SerializeField] private RuntimeAnimatorController hitmarkerAnimation;


        private EnemyStatus status;
        private EnemyIntelligence enemyIntelligence;
        private EnemyMovement enemyMovement;
        private EnemyShooting enemyShooting;


        //Hitmarkers
        private GameObject hitMarker;
        private Animator hitmarkerAnimator;

        //Health Bar
        private GameObject healthBar;
        private HealthBarManager healthBarScript;

        private string timerName;
        private bool isReloading = false;

        private void Awake()
        {
            if (!isReloading)
            {
                status = new EnemyStatus();
                status.SetupBase(maxHealth, maxHealth, false);
                status.SetName(gameObject.name);
            }
            else
            {
                name = status.GetName();
            }


            //Modules
            enemyIntelligence = GetComponent<EnemyIntelligence>();
            enemyMovement = GetComponent<EnemyMovement>();
            enemyShooting = GetComponent<EnemyShooting>();


            enemyIntelligence.enabled = true;
            enemyMovement.enabled = true;
            enemyShooting.enabled = true;


            var texture = transform.Find("Texture");

            hitMarker = texture.Find("Hitmarker").gameObject;
            hitMarker.GetComponent<SpriteRenderer>().sprite = hitmarkerSprite;
            hitmarkerAnimator = hitMarker.GetComponent<Animator>();
            hitmarkerAnimator.runtimeAnimatorController = hitmarkerAnimation;

            healthBar = texture.Find("Healthbar").gameObject;
            healthBarScript = healthBar.GetComponent<HealthBarManager>();


            texture.GetComponent<SpriteRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

            //Animation timer
            StartCoroutine(Values.WaitForTimer());
            timerName = name.ToUpper() + "_K";
            TimerController.AddTimer(timerName, 10f); //todo switch case per ogni tot per inserire il frame
            TimerController.AddCurrentTime(timerName, 0f);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //Hitmarkers
            if (status.GetIsHit())
            {
                //Controlla prima che l'animator non stesse già girando
                if (hitmarkerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >=
                    hitmarkerAnimator.GetCurrentAnimatorStateInfo(0).length + Values.GetHitmarkerAnimationLengthener())
                {
                    hitMarker.SetActive(false);
                    status.SetIsHit(false);
                    hitmarkerAnimator.enabled = false;
                }
                else if (hitmarkerAnimator.enabled == false)
                {
                    //Fa partire l'animazione
                    hitMarker.SetActive(true);
                    hitmarkerAnimator.enabled = true; //riattiva l'animator
                    hitmarkerAnimator.Play("hitmarker_Anim", -1, 0f);
                }

                if (!healthBar.activeSelf)
                {
                    healthBar.SetActive(true);
                }

            }

            //Change Health bar color (Health bar manager)
            healthBarScript.ChangeHealthBarColor(status.GetHealth(), maxHealth);

            //Change Health bar width (Health bar manager)
            healthBarScript.ChangeHealthBarWidth(status.GetHealth(), maxHealth);


            //Manage the looking at player stuff
            enemyMovement.LookPlayer();


            if (!status.GetForceStop())
            {
                /*Check whether or not it spotted the player.*/

                if (status.GetIsPlayerInView() && enemyIntelligence.GetMemoryTimeLeft() > 0)
                {
                    enemyShooting.Shoot();
                }

                /*Check health to destroy the object*/
                CheckHealth();

                status.SaveRotation(transform.rotation);
                status.SavePosition(transform.position);
            }
        }

        private void CheckHealth()
        {
            if (status.GetHealth() <= 0)
            {
                //Fa un controllo e randomicamente droppa roba
                if (Random.Range(0, 100) > 60)
                {
                    GameObject objPrefab;
                    Vector3 offset = Vector3.zero;
                    if (Values.GetHealth() >= Values.GetMaxHealth())
                    {
                        //Spawn ammobox
                        objPrefab = Values.GetAmmoBoxPrefab();
                        offset.y = 1;
                    }
                    else
                    {
                        //Spawn vita
                        objPrefab = Values.GetHealthPackPrefab();
                    }

                    var tmp = Instantiate(objPrefab, transform.position - offset, transform.rotation);
                    tmp.tag = Values.DynamicPickupTag;        //Set correct tag, default should be "Pickup"
                }

                Destroy(gameObject);
            }
        }


        public float GetAnimationTimer()
        {
            return TimerController.GetCurrentTime()[timerName];
        }


        public void SetDamage(int hit, Vector3 hitPoint)
        {
            status.ModifyHealth(-hit);

            //Create a hit on the model and make it stay on the model for some time
            hitMarker.SetActive(true);
            hitMarker.transform.position = hitPoint;
            status.SetIsHit(true);

            //when hit, the enemy should know the player location for a second or so
            enemyIntelligence.AlertEnemy();
        }

        public void Reload(EnemyStatus status)
        {
            this.status = status;

            isReloading = true;
            Awake(); //Reload everything about the enemy
            isReloading = false;
        }

        public EnemyStatus GetStatus()
        {
            return status;
        }

        public void SetName(string name)
        {
            gameObject.name = name;
        }
    }
}