using Enemies;
using UnityEngine;

namespace Player
{
    public class WeaponBehaviour : MonoBehaviour
    {
        
        /*Weapon Attributes*/

        [SerializeField] private bool isMelee = false;
        [SerializeField] private int damagePerShot;
        [SerializeField] private float shotSpread;

        private bool input;
        private Utility.TimerController.TimerEnum cooldownTimer;
        private Utility.TimerController.TimerEnum attackTimer;
        
        /*Camera Module*/
        
        private GameObject cameraMain;

        void Start()
        {
            
            /*Setup Camera*/
            
            cameraMain = GameObject.Find("Camera_Main");
            
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
                    cooldownTimer = Utility.TimerController.TimerEnum.KnifeCooldown;
                    attackTimer = Utility.TimerController.TimerEnum.KnifeAttack;
                    break;
                case "PlayerPistol":
                    input = Input.GetMouseButtonDown(0);
                    cooldownTimer = Utility.TimerController.TimerEnum.PistolCooldown;
                    attackTimer = Utility.TimerController.TimerEnum.PistolAttack;
                    break;
                case "PlayerSMG":
                    input = Input.GetMouseButton(0);
                    cooldownTimer = Utility.TimerController.TimerEnum.SMGCooldown;
                    attackTimer = Utility.TimerController.TimerEnum.SMGAttack;
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
            }
        }

        public void MeleeHit()
        {
            //When an attack goes through the attack cooldown is reset
            Utility.TimerController.ResetTimer(cooldownTimer);
                
            Values.SetIsAttacking(Values.GetCurrentWeapon(), true);
            Utility.TimerController.ResetTimer(attackTimer);
            
            if (Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out RaycastHit hit, Values.GetMeleeDistance(), LayerMask.GetMask("EnemyHitbox")))
            {
                GameObject enemy =  hit.transform.parent.gameObject;
                EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
                enemyScript.DecreaseHealth(damagePerShot);
            }
        }
        
        public void ShootProjectile()
        {
            if (Values.GetCurrentAmmo()[Values.GetCurrentWeapon()] > 0)
            {
                //When an attack goes through the attack cooldown is reset
                Utility.TimerController.ResetTimer(cooldownTimer);
                
                Values.SetIsAttacking(Values.GetCurrentWeapon(), true);
                Utility.TimerController.ResetTimer(attackTimer);
                
                Values.DecrementCurrentAmmo(Values.GetCurrentWeapon(), 1);
                
                if (Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out RaycastHit projectile, Values.GetProjectileDistance(), LayerMask.GetMask("EnemyHitbox")))
                {
                    GameObject enemy =  projectile.transform.parent.gameObject;
                    EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
                    enemyScript.DecreaseHealth(damagePerShot);


                }
            }
        }

        
        private void Reload()
        {
            if (Input.GetKeyDown("r") && !Values.GetIsRunning() &&!Values.GetIsAttacking()[Values.GetCurrentWeapon()] && (Utility.TimerController.GetCurrentTime()[Utility.TimerController.TimerEnum.ReloadTime] == 0))
            {
                //On successful reload the cooldown is reset
                
                Values.SetIsReloading(true);
                Utility.TimerController.ResetTimer(Utility.TimerController.TimerEnum.ReloadTime);

                //Reloads only while having enough ammo
                //Figures out how much ammunition to reload
                
                if (Values.GetAmmoReserve()[Values.GetCurrentWeapon()] >= Values.GetReloadAmount()[Values.GetCurrentWeapon()])
                {
                    Values.DecrementAmmoReserve(Values.GetCurrentWeapon(), (Values.GetReloadAmount()[Values.GetCurrentWeapon()] - Values.GetCurrentAmmo()[Values.GetCurrentWeapon()]));
                    Values.IncrementCurrentAmmo(Values.GetCurrentWeapon(), (Values.GetReloadAmount()[Values.GetCurrentWeapon()] - Values.GetCurrentAmmo()[Values.GetCurrentWeapon()]));
                }
            }
            
            if (Utility.TimerController.GetCurrentTime()[Utility.TimerController.TimerEnum.ReloadTime] == 0)
            {
                Values.SetIsReloading(false);
            }
            
            //The cooldown depletes over time
            Utility.TimerController.RunTimer(Utility.TimerController.TimerEnum.ReloadTime);
            
        }

        public bool GetIsMelee()
        {
            return isMelee;
        }
        
    }
}
