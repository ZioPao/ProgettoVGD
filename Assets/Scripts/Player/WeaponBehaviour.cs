using Enemies;
using UnityEngine;
using Utility;

namespace Player
{
    public class WeaponBehaviour : MonoBehaviour
    {
        
        /*Weapon Attributes*/

        [SerializeField] private bool isMelee = false;
        [SerializeField] private int damagePerShot;
        [SerializeField] private float shotSpread;

        private bool input;
        private string cooldownTimer;
        private string attackTimer;
        
        /*Camera Module*/

        private GameObject cameraMain;
        
        /*Lights*/
        private Light gunshotLight;

        void Start()
        {
            
            /*Setup Camera*/
            
            cameraMain = GameObject.Find("Camera_Main");
            gunshotLight = GameObject.Find("gunshot_light").GetComponent<Light>();

        }
        
        public void Action()
        {
            
            /*Define Weapon Actions*/
            SetupAttack();
            ExecuteAttack();
            Reload();    

        }

        private void SetupAttack()
        {
            switch (gameObject.name)
            {
                case "PlayerKnife":
                    input = Input.GetMouseButtonDown(0);
                    cooldownTimer = TimerController.KNIFECOOLDOWN_K;
                    attackTimer = TimerController.KNIFEATTACK_K;
                    break;
                case "PlayerPistol":
                    input = Input.GetMouseButtonDown(0);
                    cooldownTimer = TimerController.PISTOLCOOLDOWN_K;
                    attackTimer = TimerController.PISTOLATTACK_K;
                    break;
                case "PlayerSMG":
                    input = Input.GetMouseButton(0);
                    cooldownTimer = TimerController.SMGCOOLDOWN_K;
                    attackTimer = TimerController.SMGATTACK_K;
                    break;
                default:
                    break;
            }
        }

        private void ExecuteAttack()
        {
            if (input && !Values.GetIsRunning() && (Utility.TimerController.GetCurrentTime()[cooldownTimer] == 0))
            {
                if (isMelee)
                {
                    MeleeHit();
                }
                else
                {
                    ShootProjectile();
                }
            }
            
            //Timers for Attack Duration and Cooldown deplete over time
            Utility.TimerController.RunTimer(cooldownTimer);
            Utility.TimerController.RunTimer(attackTimer);

            if ((Utility.TimerController.GetCurrentTime()[attackTimer] == 0) && Values.GetIsAttacking()[Values.GetCurrentWeapon()])
            {
                Values.SetIsAttacking(Values.GetCurrentWeapon(), false);
                gunshotLight.enabled = false;

            }
        }

        public void MeleeHit()
        {
            //When an attack goes through the attack cooldown is reset
            TimerController.ResetTimer(cooldownTimer);
                
            Values.SetIsAttacking(Values.GetCurrentWeapon(), true);
            TimerController.ResetTimer(attackTimer);
            
            if (Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out RaycastHit hit, Values.GetMeleeDistance(), LayerMask.GetMask("EnemyHitbox")))
            {
                GameObject enemy =  hit.transform.parent.gameObject;
                EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
                enemyScript.SetDamage(damagePerShot);
            }
        }
        
        public void ShootProjectile()
        {
            if (Values.GetCurrentAmmo()[Values.GetCurrentWeapon()] > 0)
            {
                //When an attack goes through the attack cooldown is reset
                TimerController.ResetTimer(cooldownTimer);
                
                Values.SetIsAttacking(Values.GetCurrentWeapon(), true);
                TimerController.ResetTimer(attackTimer);
                
                Values.DecrementCurrentAmmo(Values.GetCurrentWeapon(), 1);
                
                if (Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out RaycastHit projectile, Values.GetProjectileDistance(), LayerMask.GetMask("EnemyHitbox")))
                {
                    GameObject enemy =  projectile.transform.parent.gameObject;
                    EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
                    enemyScript.SetDamage(damagePerShot);
                }
                
                //Activates the muzzle flash
                gunshotLight.enabled = true;

            }
        }


        
        private void Reload()
        {
            if (Input.GetKeyDown("r") && !Values.GetIsRunning() &&!Values.GetIsAttacking()[Values.GetCurrentWeapon()] && (Utility.TimerController.GetCurrentTime()[TimerController.RELOADTIME_K] == 0))
            {
                //On successful reload the cooldown is reset
                
                Values.SetIsReloading(true);
                Utility.TimerController.ResetTimer(TimerController.RELOADTIME_K);

                //Reloads only while having enough ammo
                //Figures out how much ammunition to reload
                
                if (Values.GetAmmoReserve()[Values.GetCurrentWeapon()] >= Values.GetReloadAmount()[Values.GetCurrentWeapon()])
                {
                    Values.DecrementAmmoReserve(Values.GetCurrentWeapon(), (Values.GetReloadAmount()[Values.GetCurrentWeapon()] - Values.GetCurrentAmmo()[Values.GetCurrentWeapon()]));
                    Values.IncrementCurrentAmmo(Values.GetCurrentWeapon(), (Values.GetReloadAmount()[Values.GetCurrentWeapon()] - Values.GetCurrentAmmo()[Values.GetCurrentWeapon()]));
                }
            }
            
            if (Utility.TimerController.GetCurrentTime()[TimerController.RELOADTIME_K] == 0)
            {
                Values.SetIsReloading(false);
            }
            
            //The cooldown depletes over time
            Utility.TimerController.RunTimer(TimerController.RELOADTIME_K);
            
        }

        public bool GetIsMelee()
        {
            return isMelee;
        }
        
    }
}
