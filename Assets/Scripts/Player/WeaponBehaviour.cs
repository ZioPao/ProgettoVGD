using UnityEngine;

namespace Player
{
    public class WeaponBehaviour : MonoBehaviour
    {
        
        /*Weapon Attributes*/

        [SerializeField] private bool isMelee = false;
        [SerializeField] private int damagePerShot;
        [SerializeField] private float shotSpread;
        
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
            
            switch (gameObject.name)
            {
                case "PlayerKnife":
                    KnifeAttack();
                    break;
                case "PlayerPistol":
                    PistolAttack();
                    Reload();
                    break;
                case "PlayerSMG":
                    SMGAttack();
                    Reload();
                    break;
                default:
                    break;
            }

            if ((Utility.TimerController.GetCurrentTime()[Utility.TimerController.TimerEnum.KnifeAttack] == 0) &&
                (Utility.TimerController.GetCurrentTime()[Utility.TimerController.TimerEnum.PistolAttack] == 0) &&
                (Utility.TimerController.GetCurrentTime()[Utility.TimerController.TimerEnum.SMGAttack] == 0))
            {
                Values.SetIsAttacking(false);
            }
            
        }

        private void KnifeAttack()
        {
            
            //Knife attacks once on the first frame the left mouse key is pressed

            if (Input.GetMouseButtonDown(0) && !Values.GetIsRunning() && (Utility.TimerController.GetCurrentTime()[Utility.TimerController.TimerEnum.KnifeAttack] == 0))
            {
                //When an attack goes through the attack cooldown is reset
                
                Values.SetIsAttacking(true);
                Utility.TimerController.ResetTimer(Utility.TimerController.TimerEnum.KnifeAttack);

                //Make the actual attack
                
                MeleeHit();
            }

            //The cooldown depletes over time
            Utility.TimerController.RunTimer(Utility.TimerController.TimerEnum.KnifeAttack);
            
        }

        private void PistolAttack()
        {
            
            //Pistol attacks once on the first frame the left mouse key is pressed
            
            if (Input.GetMouseButtonDown(0) && !Values.GetIsRunning() && !Values.GetIsReloading() && (Utility.TimerController.GetCurrentTime()[Utility.TimerController.TimerEnum.PistolAttack] == 0))
            {
                //When an attack goes through the attack cooldown is reset
                
                Values.SetIsAttacking(true);
                Utility.TimerController.ResetTimer(Utility.TimerController.TimerEnum.PistolAttack);
                
                //Make the actual attack

                ShootProjectile();
            }
            
            //The cooldown depletes over time
            Utility.TimerController.RunTimer(Utility.TimerController.TimerEnum.PistolAttack);
            
        }

        private void SMGAttack()
        {
            
            //SMG keeps shooting on cooldown as long as the left mouse key is pressed
            
            if (Input.GetMouseButton(0) && !Values.GetIsRunning() && !Values.GetIsReloading() && (Utility.TimerController.GetCurrentTime()[Utility.TimerController.TimerEnum.SMGAttack] == 0))
            {
                //When an attack goes through the attack cooldown is reset
                
                Values.SetIsAttacking(true);
                Utility.TimerController.ResetTimer(Utility.TimerController.TimerEnum.SMGAttack);

                //Make the actual attack
                
                ShootProjectile();
            }

            //The cooldown depletes over time
            Utility.TimerController.RunTimer(Utility.TimerController.TimerEnum.SMGAttack);
            
        }

        
        public void MeleeHit()
        {
            if (Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out RaycastHit hit, Values.GetMeleeDistance(), LayerMask.GetMask("EnemyHitbox")))
            {
                Destroy(hit.transform.parent.gameObject);
            }
        }
        
        public void ShootProjectile()
        {
            if (Values.GetCurrentAmmo()[Values.GetCurrentWeapon()] > 0)
            {
                Values.DecrementCurrentAmmo(Values.GetCurrentWeapon(), 1);
                
                if (Physics.Raycast(cameraMain.transform.position, cameraMain.transform.forward, out RaycastHit projectile, Values.GetProjectileDistance(), LayerMask.GetMask("EnemyHitbox")))
                {
                    Destroy(projectile.transform.parent.gameObject);
                }
            }
        }

        
        private void Reload()
        {
            if (Input.GetKeyDown("r") && !Values.GetIsRunning() &&!Values.GetIsAttacking() && (Utility.TimerController.GetCurrentTime()[Utility.TimerController.TimerEnum.ReloadTime] == 0))
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
