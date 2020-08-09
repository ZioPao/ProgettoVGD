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
            TimerController.RunTimer(cooldownTimer);
            TimerController.RunTimer(attackTimer);

            
            if ((TimerController.GetCurrentTime()[attackTimer] == 0) && Values.GetIsAttacking().ContainsValue(true))
            {
                Values.SetIsAttacking(Values.GetCurrentWeapon(), false);
                gunshotLight.enabled = false;

            }
        }

        public void MeleeHit()
        {

            if (!Values.GetIsGameOver() && !Values.GetIsReloading() && !Values.GetIsInPause() && !Values.GetIsFrozen())
            {
                //Audio is Played
                Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.MeleeAttack);
			
                //When an attack goes through the attack cooldown is reset
                TimerController.ResetTimer(cooldownTimer);
                
                Values.SetIsAttacking(Values.GetCurrentWeapon(), true);
                TimerController.ResetTimer(attackTimer);
            
                if (Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out RaycastHit hit, Values.GetMeleeDistance(), LayerMask.GetMask("EnemyHitbox")))
                {
                    GameObject enemy =  hit.transform.parent.gameObject;
                    EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
                    enemyScript.SetDamage(damagePerShot, hit.point);
                }
            }
            
         
        }
        
        public void ShootProjectile()
        {
            if (Values.GetCurrentAmmo()[Values.GetCurrentWeapon()] > 0 && !Values.GetIsGameOver() && !Values.GetIsReloading() && !Values.GetIsInPause() && !Values.GetIsFrozen())
            {
                //Audio is Played
                Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.RangedAttack);

                //When an attack goes through the attack cooldown is reset
                TimerController.ResetTimer(cooldownTimer);
                
                Values.SetIsAttacking(Values.GetCurrentWeapon(), true);
                TimerController.ResetTimer(attackTimer);
                
                Values.DecrementCurrentAmmo(Values.GetCurrentWeapon(), 1);
                
                //Check se ha colpito nemico
                if (Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out RaycastHit projectile, Values.GetProjectileDistance(), LayerMask.GetMask("EnemyHitbox", "Default")))
                {

                    //0 equivale a default
                    if (projectile.collider.gameObject.layer == 0)
                    {
                        //print("colpito un muro");
                        
                        //Istanzia hit
                        
                        //todo finisci mettendo un prefab ad hoc
                        
                        
                    }
                    else
                    {
                        GameObject enemy =  projectile.transform.parent.gameObject;
                        EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
                        enemyScript.SetDamage(damagePerShot, projectile.point); 
                    }

                }
                
                //Activates the muzzle flash
                gunshotLight.enabled = true;

            }
            else if (Values.GetCurrentAmmo()[Values.GetCurrentWeapon()] == 0 && !Values.GetIsGameOver() && !Values.GetIsReloading() && !Values.GetIsInPause() && !Values.GetIsFrozen())
            {
                //Audio is Played
                Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.RangedAttackFail);

                TimerController.ResetTimer(cooldownTimer);
            }
        }


        
        private void Reload()
        {

            if (Input.GetKeyDown(KeyCode.R) && !Values.GetIsRunning() &&!Values.GetIsAttacking()[Values.GetCurrentWeapon()] && TimerController.GetCurrentTime()[TimerController.RELOADTIME_K] <= 0
                && Values.GetCurrentAmmo()[Values.GetCurrentWeapon()] < Values.GetReloadAmount()[Values.GetCurrentWeapon()] && Values.GetAmmoReserve()[Values.GetCurrentWeapon()] > 0)
            {

                //Audio is Played
                Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.WeaponReload);

                //On successful reload the cooldown is reset

                Values.SetIsReloading(true);
                Utility.TimerController.ResetTimer(TimerController.RELOADTIME_K);        //1.5f
            
            }

            if (Input.GetKeyDown(KeyCode.R) && !Values.GetIsRunning() && !Values.GetIsAttacking()[Values.GetCurrentWeapon()] && TimerController.GetCurrentTime()[TimerController.RELOADTIME_K] <= 0
                && ((Values.GetCurrentAmmo()[Values.GetCurrentWeapon()] >= Values.GetReloadAmount()[Values.GetCurrentWeapon()]) || (Values.GetAmmoReserve()[Values.GetCurrentWeapon()] == 0)) 
                && Values.GetCurrentWeapon() != Values.WeaponEnum.Knife)
            {
                //Audio is Played
                Audio.SoundManager.PlaySoundEffect(Audio.SoundManager.SoundEffects.ReloadFail);

                TimerController.ResetTimer(TimerController.RELOADTIME_K);
            }
            
            if (TimerController.GetCurrentTime()[TimerController.RELOADTIME_K] <= 0 && Values.GetIsReloading())
            {
                //Reloads at the end of timer
                int tempAmmoReserve = Values.GetAmmoReserve()[Values.GetCurrentWeapon()];

                if ((tempAmmoReserve >= Values.GetReloadAmount()[Values.GetCurrentWeapon()]) ||
                    (tempAmmoReserve - (Values.GetReloadAmount()[Values.GetCurrentWeapon()] - Values.GetCurrentAmmo()[Values.GetCurrentWeapon()])) >= 0)
                {
                    Values.DecrementAmmoReserve(Values.GetCurrentWeapon(), (Values.GetReloadAmount()[Values.GetCurrentWeapon()] - Values.GetCurrentAmmo()[Values.GetCurrentWeapon()]));
                    Values.IncrementCurrentAmmo(Values.GetCurrentWeapon(), (Values.GetReloadAmount()[Values.GetCurrentWeapon()] - Values.GetCurrentAmmo()[Values.GetCurrentWeapon()]));
                }
                else
                {
                    Values.DecrementAmmoReserve(Values.GetCurrentWeapon(), tempAmmoReserve);
                    Values.IncrementCurrentAmmo(Values.GetCurrentWeapon(), tempAmmoReserve);
                }

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
